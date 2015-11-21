using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class IncomeAndPayDetail
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

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
        public int State { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public string DeptId { get; set; }

        /// <summary>
        /// Sort
        /// </summary>		
        public int Sort { get; set; }

        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// FlowTo
        /// </summary>		
        public string FlowTo { get; set; }

        /// <summary>
        /// Content
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// Type
        /// </summary>		
        public int Type { get; set; }
    }
}
