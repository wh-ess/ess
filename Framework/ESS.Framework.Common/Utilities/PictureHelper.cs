#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

#endregion

namespace ESS.Framework.Common.Utilities
{
    public class PictureHelper
    {
        public static void addWaterMark(string newFilePath, string oldFilePath, int fontSize, string fontName, string watermarkText, int xPosition,
            int yPosition, Color fontColor, FontStyle fontStyle, StringAlignment fontAlignment)
        {
            var image = Image.FromFile(oldFilePath);
            var b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
            var g = Graphics.FromImage(b);

            try
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, image.Width, image.Height);

                //水印代码
                var wmSize = new SizeF();
                var wmFont = new Font(fontName, fontSize, fontStyle);

                wmSize = g.MeasureString(watermarkText, wmFont);

                var sfm = new StringFormat();
                sfm.Alignment = fontAlignment;

                var sb = new SolidBrush(fontColor);
                g.DrawString(watermarkText, wmFont, sb, xPosition + 1, yPosition + 1, sfm);
                sb.Dispose();

                //完成 
                //image.Dispose();
                b.Save(newFilePath);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                g.Dispose();
                b.Dispose();
                image.Dispose();
            }
        }
    }
}