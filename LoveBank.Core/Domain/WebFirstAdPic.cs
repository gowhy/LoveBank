using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class WebFirstAdPic:Entity
    {
        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// AddUserId
        /// </summary>		
        public int AddUserId { get; set; }

        /// <summary>
        /// Postion
        /// </summary>		
        public int Postion { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public string DeptId { get; set; }

        /// <summary>
        /// State
        /// </summary>		
        public RowState State { get; set; }

        /// <summary>
        /// Guid
        /// </summary>		
        public string Guid { get; set; }

        /// <summary>
        /// Desc
        /// </summary>		
        public string Desc { get; set; }

        /// <summary>
        /// LinkUrl
        /// </summary>		
        public string LinkUrl { get; set; }

        /// <summary>
        /// AddUserDeptId
        /// </summary>		
        public string AddUserDeptId { get; set; }      
    }
}
