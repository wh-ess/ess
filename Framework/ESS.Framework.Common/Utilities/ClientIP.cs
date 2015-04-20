#region

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace ESS.Framework.Common.Utilities
{
    /// <summary>
    ///     客户ip帮助
    /// </summary>
    public class ClientIP
    {
        /// <summary>
        ///     获取web客户ip
        /// </summary>
        public static string GetClientIP()
        {
            var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        ///     获取客户MAC
        /// </summary>
        /// <returns></returns>
        public static string GetClientMAC()
        {
            var IP = GetClientIP();
            var dirResults = "";
            var psi = new ProcessStartInfo();
            var proc = new Process();
            psi.FileName = "nbtstat";
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = true;
            psi.Arguments = "-A   " + IP;
            psi.UseShellExecute = false;
            proc = Process.Start(psi);
            dirResults = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            dirResults = dirResults.Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "");

            var reg = new Regex("Mac[   ]{0,}Address[   ]{0,}=[   ]{0,}(?<key>((.)*?))__MAC", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var mc = reg.Match(dirResults + "__MAC");

            if (mc.Success)
            {
                return mc.Groups["key"].Value;
            }
            reg = new Regex("Host   not   found", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            mc = reg.Match(dirResults);
            if (mc.Success)
            {
                return "Host   not   found!";
            }
            return "";
        }

        public static bool AuthIP()
        {
            //string[] zf = File.ReadAllLines(Variables.strAppPath + "sundayip.txt");
            //string[] blackip = File.ReadAllLines(Variables.strAppPath+"blackip.txt");
            //string clientip = GetClientIP();
            //foreach (string s in blackip)
            //{
            //    if (s == clientip)
            //    {
            //        return false;
            //    }
            //}
            //int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek) < 1 ? 7 : Convert.ToInt32(DateTime.Now.DayOfWeek);
            //if (dayOfWeek == 7)
            //{
            //    foreach (string j in zf)
            //    {
            //        if (clientip == j)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;

            //}

            return true;
        }
    }
}