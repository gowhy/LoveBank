using System;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportTables
    {
        protected string MySql = "Database='qiandt_com';Data Source='localhost';User Id='root';Password=''";
        protected string SqlServer = "Data Source=.;Initial Catalog=qiandt_com;uid=sa;pwd=sa;";

        private static readonly DateTime StartTime = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1));

        protected static string GetTime(object time)
        {
            if (time == null || Convert.ToInt64(time)==0)
            {
                return "NULL";
            }
            return StartTime.AddSeconds(Convert.ToInt64(time)).AddDays(1).ToString();
        }
    }
}
