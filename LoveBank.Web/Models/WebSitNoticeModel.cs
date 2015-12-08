﻿using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Models
{
    public class WebSitNoticeModel
    {
        ///// <summary>
        ///// Id
        ///// </summary>		
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
        public RowState State { get; set; }

        /// <summary>
        /// Sort
        /// </summary>		
        public int Sort { get; set; }

        /// <summary>
        /// Title
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// Source
        /// </summary>		
        public string Source { get; set; }

        /// <summary>
        /// Content
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public string DeptId { get; set; }
        public List<WebSitNoticeModel> WebSitNoticeModelPageList { get; set; }
    }
}