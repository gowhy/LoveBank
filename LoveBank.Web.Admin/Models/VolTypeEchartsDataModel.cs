using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Common;

namespace LoveBank.Web.Admin.Models
{
    public class VolTypeEchartsDataModel
    {

        public List<VolTypeEchartsData> VolTypeEchartsDataList { get; set; }

        public string DepName { get; set; }
        public string Total { get; set; }


    }

    public class VolTypeEchartsData
    {

        public string value { get; set; }

        private string _Oname;

        public string Oname
        {
            get
            {
                return _Oname;
                //return this.VolType.ToLocalizable();
                //return  (new VolType()).ToLocalizable();

            }
            set { _Oname = value; }
        }

        private string _name;

        public string name
        {
            get
            {
                return _name;
                //return this.VolType.ToLocalizable();
                //return  (new VolType()).ToLocalizable();

            }
            set { _name = value; }
        }

        public VolType VolType { get; set; }

    }
}