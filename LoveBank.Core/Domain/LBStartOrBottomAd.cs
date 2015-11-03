using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class LBStartOrBottomAd : Entity
    {

      
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }
        public string Guid { get; set; }
        public string LinkUrl { get; set; }

        public string Title { get; set; }

        public string AddUserDeptId { get; set; }
     
        /// <summary>
        /// 添加人所属社区
        /// </summary>
        public string DeptId { get; set; }
        public string Desc { get; set; }

        /// <summary>
        ///数据状态
        /// </summary>
        //public RowState State { get; set; }

        public LBStartOrBottomAdPostion Postion { get; set; }


    }
}
