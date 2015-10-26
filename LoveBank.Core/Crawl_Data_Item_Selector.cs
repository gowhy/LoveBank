using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Core
{
   public class Crawl_Data_Item_Selector : Entity
    {
        public string Url { get; set; }
        public string TitleSelector { get; set; }
        public string GOUrlSelector { get; set; }
        public string PublicDateSelector { get; set; }
        public string Source { get; set; }
        public string Area { get; set; }
        public int AddUserId { get; set; }
        public int State { get; set; }
        public string Encoding { get; set; }
        public string PublicDateFormat { get; set; }
        public DateTime AddTime { get; set; }
    }
}
