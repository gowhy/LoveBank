using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.UserDataStatistics.Common
{
    public static class StringExtensions
    {
        public static string FormatWith(this string target, params object[] args)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");
            return string.Format(target, args);
        }

        public static string FormatWith(this string target, IFormatProvider provider, params object[] args)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");
            return string.Format(provider, target, args);
        }
    }
}
