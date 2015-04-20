#region

using System.Collections;
using System.Data.Linq;
using System.Web.Mvc;
using ESS.Framework.UI.ActionResultExtentions;

#endregion

namespace ESS.Framework.UI
{
    public class Result : ActionResult
    {
        private readonly object _data;
        private readonly ResultType _type;
        private DataContext _dataContext;

        public Result(object data)
        {
            _data = data;
        }

        public Result(object data, ResultType type)
        {
            _data = data;
            _type = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (_type == ResultType.Excel)
            {
                var excel = new ExcelResult(_dataContext, (IEnumerable) _data, "excel.xls");
                excel.ExecuteResult(context);
            }
            else
            {
                var jr = new JsonNetResult();
                jr.Data = _data;
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jr.ExecuteResult(context);
            }
        }
    }


    public enum ResultType
    {
        Grid,
        Excel,
        Json
    }
}