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

     

        

        private List<LargeEffectsDataModel> runList;

        public List<LargeEffectsDataModel> RunList
        {
            get 
            {
                if (runList==null)
                {
                    runList = new List<LargeEffectsDataModel>();
                }
                return runList;
            }
            set
            {
                if (value == null)
                {
                    runList = new List<LargeEffectsDataModel>();

                }
                runList = value;
            }
        }

        private List<LargeEffectsDataModel> noRunList;

        public List<LargeEffectsDataModel> NoRunList
        {
            get
            {
                if (noRunList == null)
                {
                    noRunList = new List<LargeEffectsDataModel>();
                }
                return noRunList;
            }
            set 
            {
                if (value==null)
                {
                    noRunList = new List<LargeEffectsDataModel>();

                }
                noRunList = value; 
            }
        }
    

    }

    public class LargeEffectsDataModel
    {
        public string name { get; set; }
        public string Lat { get; set; }

        public string Lon { get; set; }
        public int? State { get; set; }
        public string MachineCode { get; set; }

    }
}