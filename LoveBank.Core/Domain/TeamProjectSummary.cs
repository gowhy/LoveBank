using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class TeamProjectSummary
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
        /// State
        /// </summary>		
        public RowState State { get; set; }

        /// <summary>
        /// AddUserId
        /// </summary>		
        public int AddUserId { get; set; }

        /// <summary>
        /// TeamProjectId
        /// </summary>		
        public int TeamProjectId { get; set; }

        /// <summary>
        /// Guid
        /// </summary>		
        public string Guid { get; set; }

        /// <summary>
        /// Desc
        /// </summary>		
        public string Desc { get; set; }

        /// <summary>
        /// SubTitle
        /// </summary>		
        public string SubTitle { get; set; }   
    }
}
