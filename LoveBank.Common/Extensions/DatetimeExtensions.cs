using System;

namespace LoveBank.Common
{
    public static class DatetimeExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static int GetDay(this DateTime target)
        {
            var now = DateTime.Now;
            return (target - now).Days;
        }

        /// <summary>
        /// 获得一天的开始时间
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime target)
        {
            return Convert.ToDateTime(target.ToString("yyyy-MM-dd 00:00:00"));
        }

        /// <summary>
        /// 获得一天的结束时间
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime target)
        {
            return Convert.ToDateTime(target.ToString("yyyy-MM-dd 23:59:59"));
        }

        /// <summary>
        /// 计算剩余时间
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemainTime(this DateTime target) {
            var timeSpan = target - DateTime.Now;
            if (timeSpan.Ticks<0) return "已结束";
            return "{0}天{1}时{2}分".FormatWith(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
        }

        /// <summary>
        /// 将C# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }
    }
}
