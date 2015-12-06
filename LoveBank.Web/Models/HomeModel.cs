using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Core.Domain;

namespace LoveBank.Web.Models
{
    public class HomeModel
    {
        public string DeptIdName { get; set; }
        public List<WebFirstAdPicModel> WebFirstAdPicModelList { get; set; }
        public List<WebSitNoticeModel> WebSitNoticeList { get; set; }
        public UnitInfoAbout UnitInfoAbout { get; set; }
        public List<WebLoveShow> WebLoveShowList { get; set; }
        public List<LoveFund> LoveFundList { get; set; }
        public List<Vol> VolList { get; set; }
        public List<Team> TeamList { get; set; }
        public List<ProductModel> ProductList { get; set; }
    }
}