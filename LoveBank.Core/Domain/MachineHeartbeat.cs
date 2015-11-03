using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class MachineHeartbeat
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// MachineCode
        /// </summary>		
        public string MachineCode { get; set; }

        /// <summary>
        /// AddTime
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 当前打开的模块页面
        /// </summary>		
        public string ModuleCode { get; set; }
    }
}
