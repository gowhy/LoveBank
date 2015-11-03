using LoveBank.Common;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class VolAddSocreRecordeSelectModel
    {
        public AuditingState AuditingState { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string NFC { get; set; }
        public IPagedList<VolAddSocreRecordeModel> VolAddSocreRecordeList { get; set; }
    }
}