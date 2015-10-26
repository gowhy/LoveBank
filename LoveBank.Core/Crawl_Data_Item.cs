using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Core
{
  public  class Crawl_Data_Item:Entity
    {
        public string Url { get; set; }
        public string SourceUrl { get; set; }
        public string Title { get; set; }
      
        public DateTime PublicDate { get; set; }
        public string Source { get; set; }
        public string Area { get; set; }
        public int Crawl_Data_Item_Selector_Id { get; set; }
        public int State { get; set; }

        public DateTime AddTime { get; set; }
     
    }
}
