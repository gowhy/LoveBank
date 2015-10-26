using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class TeamMembers
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// TeamId
        /// </summary>		
        public int TeamId { get; set; }

        /// <summary>
        /// App用户的Id
        /// </summary>		
        public int AppUserId { get; set; }

        /// <summary>
        /// 申请入加入团体时间
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// AddUserId
        /// </summary>		
        public int AddUserId { get; set; }

        /// <summary>
        /// State
        /// </summary>		
        public int State { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime AuditingTime { get; set; }

        /// <summary>
        /// AuditerId
        /// </summary>		
        public int AuditerId { get; set; }

        /// <summary>
        /// AuditingMsg
        /// </summary>		
        public string AuditingMsg { get; set; }     
    }
}
