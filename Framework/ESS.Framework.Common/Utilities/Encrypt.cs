#region

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace ESS.Framework.Common.Utilities
{
    public class Encrypt
    {
        /// <summary>
        ///     用户加密函数
        /// </summary>
        public static String EncryptData(string password)
        {
            var clearBytes = new UnicodeEncoding().GetBytes(password);
            var hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}