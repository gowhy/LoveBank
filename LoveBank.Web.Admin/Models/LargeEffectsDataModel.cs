using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class LargeEffectsDataModelPage
    {
        public string name { get; set; }
        public SortedList geoCoord { get; set; }

    }

    public class LargeEffectsDataModel
    {
        public string name { get; set; }
        public string Lat { get; set; }

        public string Lon { get; set; }

    }
}