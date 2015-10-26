
using System.Collections.Generic;
using System.Linq;

namespace LoveBank.Web.Admin.Models {
    public class NavModel {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Url { set; get; }
        public bool IsBlank { set; get; }
        public int Sort { set; get; }
        public bool IsEffect { set; get; }
        public string UModule { set; get; }
        public string UAction { set; get; }
        public int UId { set; get; }
        public string UParam { set; get; }
        public bool IsShop { set; get; }
        public string AppIndex { set; get; }
        public int? ParentId { set; get; }
        public bool HasChild { get { return Child.Any(); } }
        public IList<NavModel> Child { set; get; }
 
    }
}