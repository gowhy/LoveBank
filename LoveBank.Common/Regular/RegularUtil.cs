using System;
using System.Text.RegularExpressions;

namespace LoveBank.Common
{
    public static class RegularUtil
    {
        public static Regex Email = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex WebUrl = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?|(http|https)://localhost:\d{2,}[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex Password = new Regex(@"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 6~12 char and the letter(_) start
        /// </summary>
        public static Regex UserName = new Regex(@"^[A-Za-z_]\w{3,16}$", RegexOptions.Singleline | RegexOptions.Compiled);
        public static Regex Phone = new Regex(@"/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
