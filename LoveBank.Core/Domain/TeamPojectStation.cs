using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class TeamPojectStation
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>		
        public string Name { get; set; }

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
        public RowState State { get; set; }

        /// <summary>
        /// TeamPojectId
        /// </summary>		
        public int TeamPojectId { get; set; }

        /// <summary>
        /// 需要的人数
        /// </summary>		
        public int CountUser { get; set; }

        /// <summary>
        /// 岗位条件
        /// </summary>		
        public string Condition { get; set; }

        /// <summary>
        /// 岗位描述
        /// </summary>		
        public string Desc { get; set; }    
    }
}
