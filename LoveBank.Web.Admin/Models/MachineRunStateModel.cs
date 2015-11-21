using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class MachineRunStateModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 机器唯一编码，一体机通过该编码找到属于该机器的广告信息
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 机器所属社区
        /// </summary>
        public string DeptId { get; set; }

        public string DeptIdName { get; set; }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string Name { get; set; }

        public string Address { get; set; }
        public string State { get; set; }
        
    }
}