using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Netcool.Core.Helpers
{
    public static class Encrypt
    {
        #region Md5

        public static string Md5By16(string value)
        {
            return Md5By16(value, Encoding.UTF8);
        }

        public static string Md5By16(string value, Encoding encoding)
        {
            return Md5(value, encoding, 4, 8);
        }

        private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex == null
                    ? BitConverter.ToString(hash)
                    : BitConverter.ToString(hash, startIndex.SafeValue(), length.SafeValue());
            }
            finally
            {
                md5.Clear();
            }

            return result.Replace("-", "");
        }

        public static string Md5By32(string value)
        {
            return Md5By32(value, Encoding.UTF8);
        }


        public static string Md5By32(string value, Encoding encoding)
        {
            return Md5(value, encoding, null, null);
        }

        #endregion

        #region HmacSha256

        public static string HmacSha256(string value, string key)
        {
            return HmacSha256(value, key, Encoding.UTF8);
        }

        public static string HmacSha256(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
                return string.Empty;
            var sha256 = new HMACSHA256(encoding.GetBytes(key));
            var hash = sha256.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(t => t.ToString("x2")).ToArray());
        }

        #endregion
    }
}