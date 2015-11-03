using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class TeamProjectModel
    {
        public int ID { get; set; }
        public string Guid { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }

        public int? TeamId { get; set; }

        public int? Type { get; set; }

        public DateTime RecruitStartDate { get; set; }

        public DateTime RecruitEndDate { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ServiceDate { get; set; }

        public string Address { get; set; }

        public string ServiceObject { get; set; }

        public string HtmlUrl { get; set; }

        public string LinkMan { get; set; }

        public string LinkPhone { get; set; }
        public int Score { get; set; }
        public int CommentSocre { get; set; }

        public int GoodScore { get; set; }
        public int ShareScore { get; set; }


        public virtual IList<SourceFile> SourceFileList { get; set; }
    }
}