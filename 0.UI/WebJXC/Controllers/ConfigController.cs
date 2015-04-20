using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ESS.Domain.JXC;
using ESS.Domain.JXC.Models;
using ESS.Framework.Data;
using ESS.Framework.UI;
using System.Collections;
using ESS.Framework.Common;
using ESS.Framework.UI.Attribute;
using Newtonsoft.Json;
using System.IO;
using ESS.Domain.Foundation;

namespace WebUI.Controllers
{
    [Log]
    public class ConfigController : Controller
    {
        private ESS_ERPContext db = new ESS_ERPContext();

        public ActionResult ModuleInfo(string id)
        {
            if (CurrentUserInfo.CurrentUserId == 0)
            {
                return new Result(SystemConfig.ModuleInfo[id]);
            }
            var priv = db.Privileges.Where(c => c.RoleId == CurrentUserInfo.CurrentRoleId).Select(c => c.ModuleNo + c.ActionName).ToArray();
            var modules = SystemConfig.ModuleInfo[id].Where(c => priv.Contains(c.ModuleNo + c.Action)).OrderBy(c => c.Order);
            return new Result(modules);
        }

        public ActionResult ModuleTree()
        {
            ArrayList list = new ArrayList();
            var moduleinfo = SystemConfig.ModuleInfo;

            foreach (var m in db.Menus)
            {
                if (!string.IsNullOrEmpty(m.MenuNo) && moduleinfo.ContainsKey(m.MenuNo))
                {
                    var o = new { ModuleNo = m.MenuNo, MenuId = m.MenuId, ParentId = m.MenuParentId, Action = "", Text = m.Title, children = moduleinfo[m.MenuNo] };
                    list.Add(o);
                }
                else
                {
                    var o = new { ModuleNo = m.MenuNo, MenuId = m.MenuId, ParentId = m.MenuParentId, Action = "", Text = m.Title };
                    list.Add(o);
                }

            }
            return new Result(list);
        }

        public ActionResult ActionInfo(string id)
        {
            return new Result(SystemConfig.ActionInfo[id]);
        }

        public ActionResult ProcInfo(string id)
        {
            var sql = "select * from v_procinfo where moduleno='{0}' union all select * from Foundation.columns where moduleno='default' order by [index]";

            return new Result(new SqlHelper().GetSqlResult(string.Format(sql, id)));
        }

        /// <summary>
        /// 20130623 modi by hh
        /// 这个地方从前台传上来字符串以前是没有带MODULENO的，
        /// 这样如果在多表（即模块）设置里面同名，系统无法区分，典型的是图片路径在各模块都存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Select(string id)
        {
            string[] argus = id.Split('_');
            //原树形处理方法只有1个参数到后台，不具备分割条件
            if (argus.Count() == 1)
            {
                return new Result(new SqlHelper().GetSqlResult("foundation.p_GetDdl '" + argus[0] + "'"));
            }
            else
            {   //增加分支
                string moduleName = argus[0];
                string name = string.IsNullOrEmpty(argus[1]) ? "" : argus[1];
                Column cl = db.Columns.Where(c => c.ModuleNo == moduleName && c.Name == name).SingleOrDefault();
                //包含FS 表示是读取文件内容的 做文件夹递归读取 由于前台不认识. 改成_
                if (cl.SourceTableName.Contains("fs_"))
                {
                    Config cf = db.Configs.Where(c => c.Name == cl.SourceTableName).SingleOrDefault();
                    string filePath = this.Server.MapPath(cf.Val);
                    DirectoryInfo fileDir = new DirectoryInfo(filePath);
                    List<DropdownList> fileNameList = new List<DropdownList>();
                    foreach (FileSystemInfo fsi in fileDir.GetFileSystemInfos())
                    {
                        DropdownList ddlc = new DropdownList();
                        ddlc.id = fsi.Name.Replace('.', '_'); ;
                        ddlc.text = fsi.Name.Replace('.', '_'); ;
                        fileNameList.Add(ddlc);
                    }
                    return Json(fileNameList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //张飞的代码暂时没有改 后面如果在配置表NAME重名 传过来也会有问题，需要调整PROC
                    return new Result(new SqlHelper().GetSqlResult("foundation.p_GetDdl '" + name + "'"));
                }

            }
        }


        #region ColumnConfig
        public ActionResult ColumnConfig()
        {
            return View();
        }

        public ActionResult Columns(string id, bool isQuery = false)
        {
            if (isQuery)
            {
                return new Result(db.Columns.Where(c => c.ModuleNo == id).
                    Concat(db.Columns.Where(c => c.ModuleNo == "default")).OrderBy(c => c.Index));
            }
            if (db.Columns.Count(c => c.ModuleNo == id) > 0)
            {
                return ColumnsWithoutDefault(id);
            }
            else
            {
                return new Result(db.Columns.Where(c => c.ModuleNo == "default"));
            }
        }

        public ActionResult ColumnsWithoutDefault(string id)
        {
            return new Result(db.Columns.Where(c => c.ModuleNo == id).OrderBy(c => c.Index));
        }

        public ActionResult ColumnsIds()
        {
            var sql = @"SELECT DISTINCT ModuleNo,Title FROM Foundation.Columns a LEFT JOIN Foundation.Menu b
                        on a.ModuleNo = b.MenuNo";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        public ActionResult AddColumns(string moduleNo, string Name)
        {
            string[] names = Name.Split(',');

            foreach (var s in names)
            {
                Column c = new Column();
                c.ModuleNo = moduleNo;
                c.Name = s;
                c.Text = db.Columns.Where(d => d.Name == s).Max(d => c.Text);
                db.Columns.Add(c);
            }
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        public ActionResult ColumnSave(string moduleNo, string data)
        {
            IList<Column> list = JsonConvert.DeserializeObject<List<Column>>(data);
            foreach (var item in list)
            {
                var s = db.Columns.First(c => c.ModuleNo == item.ModuleNo && c.Name == item.Name);
                AutoMapper.Mapper.DynamicMap(item, s);
                db.SaveChanges();
            }

            return new Result(AjaxResult.Success());
        }
        #endregion

        #region Config
        public ActionResult Config()
        {
            return RedirectToAction("gridview", "manage", new { menuno = "Config" });
        }

        [Module("Config", "query")]
        public ActionResult GetConfig(string Name)
        {
            string sql = @"SELECT * from Foundation.Config  where ";
            sql += string.IsNullOrEmpty(Name) ? "1=1" : "Name like  '%" + Name + "%'";
            return new Result(new SqlHelper().GetSqlResult(sql));
        }

        [HttpPost]
        [Module("Config", "add")]
        public ActionResult AddConfig(FormCollection form)
        {
            Config m = new Config();
            TryUpdateModel(m);
            db.Configs.Add(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Config", "edit")]
        public ActionResult EditConfig(FormCollection form)
        {
            int cfid = Convert.ToInt32(form["ConfigID"]);
            Config m = db.Configs.First(c => c.ConfigId == cfid);
            TryUpdateModel(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }

        [HttpPost]
        [Module("Config", "del")]
        public ActionResult DelConfig(int ConfigID)
        {
            Config m = db.Configs.First(c => c.ConfigId == ConfigID);
            db.Configs.Remove(m);
            db.SaveChanges();
            return new Result(AjaxResult.Success());
        }
        #endregion

        #region ddl 读取辅助
        public JsonResult GetDDLData(string name)
        {
            IList<DdlControl> ddllist = db.DdlControls.Where(c => c.Name == name).ToList();
            return Json(ddllist, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
