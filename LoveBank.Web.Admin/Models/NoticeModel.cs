using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class NoticeModel
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string DepId { get; set; }
        public string DepName { get; set; }

        public string ImgUrl { get; set; }
        public string Des { get; set; }
        public string Title { get; set; }
        public string AddUser { get; set; }
        public int State { get; set; }
        public string UploadHtmlFile { get; set; }
        public string LinkSocSerUrl { get; set; }

        public virtual IList<SourceFile> SourceFileList { get; set; }
    }
}