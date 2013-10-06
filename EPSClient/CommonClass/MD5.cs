using System;
using System.Security.Cryptography;
using System.Text;

namespace HRSeat.CommonClass
{
    /// <summary>
    /// MD5加密
    /// </summary>
    class MD5
    {
        /// <summary>
        /// 获取MD5加密后的字符串,字母全部转换成大写
        /// </summary>
        /// <param name="ConvertString">原字符串</param>
        /// <returns></returns>
        public static string GetCapString(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
            result = result.Replace("-", "");
            return result;
        }
    }
}
