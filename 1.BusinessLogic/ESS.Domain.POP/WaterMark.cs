using ESS.Domain.POP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ESS.Framework.Common.Utilities;

namespace ESS.Domain.POP
{

    public class WaterMark
    {
        public string text { get; set; }
        public int fontSize { get; set; }
        public string fontName { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public Color fontColor { get; set; }
        public FontStyle fontStyle { get; set; }
        public string newFilePath { get; set; }
        public string oldFilePath { get; set; }
        public StringAlignment fontAlignment { get; set; }
        public int order { get; set; }
        public int POPType { get; set; }

        public void addWaterMark()
        {
            PictureHelper.addWaterMark(this.newFilePath, this.oldFilePath, this.fontSize, this.fontName, this.text, this.xPosition,
                this.yPosition, this.fontColor, this.fontStyle, this.fontAlignment);
        }

        public WaterMark(string name, string value, Guid templateHeadGuid, string brandName, string emptyTemplateDir)
        {
            //构造函数根据配置文档读取当前模板定义内容
            ESS_POSDataContext db = new ESS_POSDataContext();
            TemplateBody tb = db.TemplateBody.Where(c => c.Name == name && c.HeadGuid == templateHeadGuid).SingleOrDefault();
            this.text = value;
            this.fontName = tb.FontName;
            this.fontSize = tb.FontSize;

            //获取字体
            if (tb.FontStyle == "bold")
            {
                this.fontStyle = FontStyle.Bold;
            }
            else
            {
                this.fontStyle = FontStyle.Regular;
            }

            //获取颜色
            if (tb.FontColor == "red")
            {
                this.fontColor = Color.Red;
            }
            else
            {
                this.fontColor = Color.Black;
            }

            //获取对齐方式
            switch (tb.FontAlignment)
            {
                case "left":
                    this.fontAlignment = StringAlignment.Near;
                    break;
                case "right":
                    this.fontAlignment = StringAlignment.Far;
                    break;
                case "center":
                    this.fontAlignment = StringAlignment.Center;
                    break;
                default:
                    this.fontAlignment = StringAlignment.Center;
                    break;
            }

            this.xPosition = tb.XPosition;
            this.yPosition = tb.YPosition;
            this.newFilePath = System.Web.HttpContext.Current.Server.MapPath(tb.NewFilePath) + "_" + brandName + ".jpg"; ;

            if (value == brandName)
            {
                //this.oldFilePath = System.Web.HttpContext.Current.Server.MapPath(wmc.OldFilePath);
                this.oldFilePath = System.Web.HttpContext.Current.Server.MapPath(emptyTemplateDir);
            }
            else
            {
                this.oldFilePath = System.Web.HttpContext.Current.Server.MapPath(tb.OldFilePath) + "_" + brandName + ".jpg";
            }
        }

    }
}
