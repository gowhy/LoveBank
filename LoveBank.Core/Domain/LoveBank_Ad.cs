using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class LoveBank_Ad : Entity
    {

      
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }
        public string Guid { get; set; }
        public string LinkUrl { get; set; }

        public string Title { get; set; }
        //public DateTime AddTime { get; set; }

        //public int AddUserId { get; set; }
        public FileType AdType { get; set; }
  
        /// <summary>
        /// 机器唯一编码，一体机通过该编码找到属于该机器的广告信息
        /// </summary>
        public string MachineCode { get; set; }
        public int MachineId { get; set; }
        /// <summary>
        /// 添加人所属社区
        /// </summary>
        public string DeptId { get; set; }
        public string Desc { get; set; }

        /// <summary>
        ///数据状态
        /// </summary>
        //public RowState State { get; set; }
         
  

    }
}
