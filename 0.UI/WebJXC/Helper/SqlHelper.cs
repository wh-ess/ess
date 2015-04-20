using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using ESS.Domain.Car;
using ESS.Domain.JXC.Models;

public class SqlHelper
{
    private readonly ESS_ERPContext _db = new ESS_ERPContext();

    public DbCommand GetCMDBySql(string sql, DbContext db = null)
    {
        if (db == null) db = _db;
        DbCommand cmd = db.Database.Connection.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = sql;
        cmd.CommandTimeout = 3600;
        return cmd;
    }

    public IList<IDictionary<string, object>> GetSqlResult(string sql,DbContext db = null)
    {
        DbCommand cmd = GetCMDBySql(sql,db);
        return GetSqlResult(cmd);
    }


    public IList<IDictionary<string, object>> GetSqlResult(DbCommand cmd)
    {
        IList<IDictionary<string, object>> result = new List<IDictionary<string, object>>();

        cmd.Connection.Open();
        using (IDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                IDictionary<string, object> d = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    d.Add(reader.GetName(i), reader.GetValue(i));
                }
                result.Add(d);
            }
        }
        cmd.Connection.Close();
        return result;
    }

    public IList<IDictionary<string, object>> GetSqlResult(NameValueCollection form)
    {
        string p = form["p"];

        form.Remove("p");
        form.Remove("page");
        form.Remove("pagesize");
        form.Remove("url");

        DbCommand cmd = _db.Database.Connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = p;
        cmd.CommandTimeout = 3600;

        IEnumerable<string> keys =
            SystemConfig.Columns.Where(c => c.ModuleNo == p).Select(c => c.Name).Concat(form.AllKeys).Distinct();
        foreach (string key in keys)
        {
            if (!string.IsNullOrEmpty(form[key]))
            {
                cmd.Parameters.Add(new SqlParameter(key, form[key]));
            }
        }
        return GetSqlResult(cmd);
    }


    public string GetSqlString(string sql)
    {
        DbCommand cmd = GetCMDBySql(sql);
        cmd.Connection.Open();
        string s = cmd.ExecuteScalar().ToString();
        cmd.Connection.Close();
        return s;
    }

    public int SqlExecuteNonQuery(string sql)
    {
        DbCommand cmd = GetCMDBySql(sql);
        cmd.Connection.Open();
        int i = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return i;
    }
}