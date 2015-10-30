using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class GridMemberModel
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string DeptId { get; set; }
        public string VDeptId { get; set; }
     
        public string VDeptName { get; set; }
        public string GridNo { get; set; }
        public string GridName { get; set; }

        public string GridPhone { get; set; }
        public string GridHeaderImg { get; set; }
        public int AddUserId { get; set; }
        public string Desc { get; set; }
        public  SourceFile SourceFile { get; set; }

        public virtual IList<SourceFile> SourceFileList { get; set; }
    }
}