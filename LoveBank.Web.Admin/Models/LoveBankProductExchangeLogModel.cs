using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class LoveBankProductExchangeLogModel
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
        /// LoveBankProductId
        /// </summary>		
        public int LoveBankProductId { get; set; }

        /// <summary>
        /// CostScore
        /// </summary>		
        public int CostScore { get; set; }

        /// <summary>
        /// Address
        /// </summary>		
        public string Address { get; set; }

        /// <summary>
        /// MacineCode
        /// </summary>		
        public string MacineCode { get; set; }

        /// <summary>
        /// Source
        /// </summary>		
        public string Source { get; set; }

        public string ProductName { get; set; }

        public int ExChangeCount { get; set; }

        public ExchangeType Type { get; set; }
    }
}