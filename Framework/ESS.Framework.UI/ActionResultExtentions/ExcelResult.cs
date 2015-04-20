#region

using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace ESS.Framework.UI.ActionResultExtentions
{
    public class ExcelResult : ActionResult
    {
        private readonly DataContext _dataContext;
        private readonly string _fileName;
        private readonly TableItemStyle _headerStyle;
        private readonly IDictionary<string, string> _headerTexts;
        private readonly TableItemStyle _itemStyle;
        private readonly IEnumerable _rows;
        private readonly TableStyle _tableStyle;
        private string[] _headers;

        public ExcelResult(DataContext dataContext, IEnumerable rows, string fileName)
            : this(dataContext, rows, fileName, null, null, null, null, null)
        {
        }

        public ExcelResult(DataContext dataContext, string fileName, IEnumerable rows, string[] headers)
            : this(dataContext, rows, fileName, headers, null, null, null, null)
        {
        }

        public ExcelResult(DataContext dataContext, IEnumerable rows, string fileName, string[] headers,
            IDictionary<string, string> headerText, TableStyle tableStyle, TableItemStyle headerStyle,
            TableItemStyle itemStyle)
        {
            _dataContext = dataContext;
            _rows = rows;
            _fileName = fileName;
            _headers = headers;
            _headerTexts = headerText;
            _tableStyle = tableStyle;
            _headerStyle = headerStyle;
            _itemStyle = itemStyle;

            // provide defaults
            if (_tableStyle == null)
            {
                _tableStyle = new TableStyle();
                _tableStyle.BorderStyle = BorderStyle.Solid;
                _tableStyle.BorderColor = Color.Black;
                _tableStyle.BorderWidth = Unit.Parse("2px");
            }
            if (_headerStyle == null)
            {
                _headerStyle = new TableItemStyle();
                _headerStyle.BackColor = Color.LightGray;
            }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public IEnumerable Rows
        {
            get { return _rows; }
        }

        /// HTML效率不高 测试CVS纯文本方式
        public override void ExecuteResult(ControllerContext context)
        {
            // Create HtmlTextWriter
            var sw = new StringWriter();
            var tw = new HtmlTextWriter(sw);

            // Build HTML Table from Items
            if (_tableStyle != null)
                _tableStyle.AddAttributesToRender(tw);
            tw.RenderBeginTag(HtmlTextWriterTag.Table);

            var isQueryable = false;
            if (_rows is IQueryable)
            {
                isQueryable = true;
            }
            // Generate headers from table
            if (_headers == null)
            {
                if (isQueryable)
                {
                    var r = _rows.AsQueryable();
                    _headers =
                        _dataContext.Mapping.GetMetaType(r.ElementType)
                            .PersistentDataMembers.Select(m => m.Name)
                            .ToArray();
                }
                else
                {
                    _headers = ((IDictionary<string, object>) (((IList) _rows)[0])).Keys.ToArray();
                }
            }


            // Create Header Row
            tw.RenderBeginTag(HtmlTextWriterTag.Thead);
            foreach (var header in _headers)
            {
                if (_headerStyle != null)
                    _headerStyle.AddAttributesToRender(tw);
                tw.RenderBeginTag(HtmlTextWriterTag.Th);
                if (_headerTexts.ContainsKey(header))
                {
                    tw.Write(string.IsNullOrEmpty(_headerTexts[header]) ? header : _headerTexts[header]);
                }
                else
                {
                    tw.Write(header);
                }
                tw.RenderEndTag();
            }
            tw.RenderEndTag();


            // Create Data Rows
            tw.RenderBeginTag(HtmlTextWriterTag.Tbody);
            foreach (var row in _rows)
            {
                tw.RenderBeginTag(HtmlTextWriterTag.Tr);
                foreach (var header in _headers)
                {
                    object s;
                    if (isQueryable)
                    {
                        s = row.GetType().GetProperty(header).GetValue(row, null);
                    }
                    else
                    {
                        s = ((IDictionary<string, object>) row)[header];
                    }

                    var strValue = s == null ? "" : s.ToString();
                    strValue = ReplaceSpecialCharacters(strValue);
                    if (_itemStyle != null)
                        _itemStyle.AddAttributesToRender(tw);
                    tw.RenderBeginTag(HtmlTextWriterTag.Td);
                    tw.Write(HttpUtility.HtmlEncode(strValue));
                    tw.RenderEndTag();
                }
                tw.RenderEndTag();
            }
            tw.RenderEndTag(); // tbody

            tw.RenderEndTag(); // table
            WriteFile(_fileName, "application/ms-excel", sw.ToString());
        }


        private static string ReplaceSpecialCharacters(string value)
        {
            value = value.Replace("’", "'");
            value = value.Replace("“", "\"");
            value = value.Replace("”", "\"");
            value = value.Replace("–", "-");
            value = value.Replace("…", "");
            return value;
        }

        private static void WriteFile(string fileName, string contentType, string content)
        {
            var context = HttpContext.Current;
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            context.Response.Charset = "gb2312";
            context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = contentType;
            context.Response.Write(content);
            context.Response.End();
        }
    }
}