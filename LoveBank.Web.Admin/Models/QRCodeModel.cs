using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class QRCodeModel
    {
        public int UserId { get; set; }
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public QRCodeState State { get; set; }
        public string Ticket { get; set; }
        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime? GrantTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal GrantLon { get; set; }
        public decimal GrantLat { get; set; }

        public string GrantAddress { get; set; }
        public string MachineCode { get; set; }

        public QRCodeType Type { get; set; }

        public string QRCodeStringKey { get; set; }
        /// <summary>
        /// 存储其他多余数据
        /// </summary>
        public object Data { get; set; }
    }
}