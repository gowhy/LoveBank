using LoveBank.Common;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Models
{
    public class ConvenienceServicesModel
    {

        public int ID { get; set; }
        /// <summary>
        /// AddUserId
        /// </summary>		
        public int AddUserId { get; set; }

        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// State
        /// </summary>		
        public RowState State { get; set; }

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
        /// Content
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// Type
        /// </summary>		
        public ConvenienceServicesType Type { get; set; }
        public IPagedList<ConvenienceServicesModel> ConvenienceServicesModelList { get; set; }
    }
}