using System;
using System.Configuration;

namespace Crawl.UserDataStatistics.Function
{
    public class Base
    {
        protected string SqlServer = ConfigurationManager.ConnectionStrings["QdtDB"].ConnectionString;
        protected DateTime StartTime = DateTime.Now.AddDays(-1).Date;
        protected DateTime EndTime = DateTime.Now.Date;
    }
}
