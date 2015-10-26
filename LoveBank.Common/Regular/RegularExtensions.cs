using System.Text.RegularExpressions;
using System.Diagnostics;

namespace LoveBank.Common
{
    public static class RegularExtensions
    {
        [DebuggerStepThrough]
        public static bool Match(this string target, Regex regex)
        {
            return regex.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool MatchAndNotNull(this string target, Regex regex)
        {
            return !string.IsNullOrEmpty(target) && regex.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return target.MatchAndNotNull(RegularUtil.WebUrl);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return target.MatchAndNotNull(RegularUtil.Email);
        }
    }
}
