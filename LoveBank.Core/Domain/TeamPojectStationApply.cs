using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
   public class TeamPojectStationApply
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// AddTime
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// TeamPojectId
        /// </summary>		
        public int TeamPojectId { get; set; }

        /// <summary>
        /// TeamPojectStationId
        /// </summary>		
        public int TeamPojectStationId { get; set; }

        /// <summary>
        /// State
        /// </summary>		
        public int State { get; set; }

        /// <summary>
        /// AuditorId
        /// </summary>		
        public int AuditorId { get; set; }

        /// <summary>
        /// Msg
        /// </summary>		
        public string Msg { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime AuditingTime { get; set; }  
    }
}
