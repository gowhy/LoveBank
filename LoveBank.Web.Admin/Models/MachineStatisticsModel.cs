using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class MachineStatisticsModel
    {  /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>		
        public int ModuleId { get; set; }

        /// <summary>
        /// ModuleCode
        /// </summary>		
        public string ModuleCode { get; set; }

        /// <summary>
        /// AddTime
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// MachineCode
        /// </summary>		
        public string MachineCode { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public string DeptId { get; set; }
    }
}