using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
   public class VolAddScoreRecorde:Entity
    {
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
        public int? TeamProjectId { get; set; }
        public int Auditor { get; set; }
       
    }
}
