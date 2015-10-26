using LoveBank.Core.Domain.Enums;
using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class VolAddSocreRecordeModel
    {

        public int ID { get; set; }
        public string Msg { get; set; }
        public int VolID { get; set; }
        public int AddScore { get; set; }
        public int AddUserId { get; set; }

        public int State { get; set; }
        public string AuditingMsg { get; set; }
        public AuditingState AuditingState { get; set; }
        public string DeptId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime? AuditingTime { get; set; }
        public string TeamProjectName { get; set; }
        public Vol Vol { get; set; }
    }
}