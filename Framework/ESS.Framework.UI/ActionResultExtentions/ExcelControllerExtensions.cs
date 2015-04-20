#region

using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

#endregion

namespace ESS.Framework.UI.ActionResultExtentions
{
    public static class ExcelControllerExtensions
    {
        public static ActionResult Excel(this Controller controller, DataContext dataContext, IEnumerable rows, string fileName)
        {
            return new ExcelResult(dataContext, rows, fileName, null, null, null, null, null);
        }

        public static ActionResult Excel(this Controller controller, DataContext dataContext, IEnumerable rows, string fileName, string[] headers)
        {
            return new ExcelResult(dataContext, rows, fileName, headers, null, null, null, null);
        }

        public static ActionResult Excel(this Controller controller, DataContext dataContext, IEnumerable rows, string fileName, string[] headers,
            IDictionary<string, string> headerText, TableStyle tableStyle, TableItemStyle headerStyle, TableItemStyle itemStyle)
        {
            return new ExcelResult(dataContext, rows, fileName, headers, headerText, tableStyle, headerStyle, itemStyle);
        }
    }
}