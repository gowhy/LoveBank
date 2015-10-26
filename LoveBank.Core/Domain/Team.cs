using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class Team:Entity
    {
        /// <summary>
        /// Id
        /// </summary>		
        //public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public string DeptId { get; set; }

        /// <summary>
        /// Address
        /// </summary>		
        public string Address { get; set; }

        /// <summary>
        /// LinkMan
        /// </summary>		
        public string LinkMan { get; set; }

        /// <summary>
        /// LinkPhone
        /// </summary>		
        public string LinkPhone { get; set; }

        /// <summary>
        /// AddUserId
        /// </summary>		
        public int AddUserId { get; set; }

        /// <summary>
        /// AddTime
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// State
        /// </summary>		
        public TeamState State { get; set; }

        /// <summary>
        /// Desc
        /// </summary>		
        public string Desc { get; set; }

        /// <summary>
        /// WebSite
        /// </summary>		
        public string WebSite { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>		
        public DateTime EstabDate { get; set; }

        /// <summary>
        /// 用于和图片表关联，是团体的logo
        /// </summary>		
        public string Guid { get; set; }

        /// <summary>
        /// 团体等级
        /// </summary>		
        public TeamLevel Level { get; set; }    
    }
}
