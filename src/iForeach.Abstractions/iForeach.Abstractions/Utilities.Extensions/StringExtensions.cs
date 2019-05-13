using System;
using System.Linq;

namespace org.iForeach
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string hex)
        {
            switch(hex?.Length ?? -1)
            {
                case -1:
                    return null;
                case 0:
                    return new byte[0];
                case var length when length % 2 == 1:
                    throw new ArgumentException("十六进制字符串长度必须是偶数", nameof(hex));
                case var length:
                    return Enumerable.Range(0, length)
                                     .Where(x => 0 == x % 2)
                                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                     .ToArray();
            }
        }
    }
}