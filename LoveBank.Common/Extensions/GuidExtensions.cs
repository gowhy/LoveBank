using System;

namespace LoveBank.Common
{
    public static class GuidExtension
    {
        /// <summary>
        /// 对Guid进行Base64压缩
        /// </summary>
        /// <param name="target"></param>
        /// <returns>压缩后的字符串</returns>
        public static string Shrink(this Guid target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
      
    }
}
