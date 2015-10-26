using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Plugins
{
    public class PaymentOrder
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PaymentNo { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 支付银行码，网银直连的时候有用
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 支付描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 返回的URL
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 自动通知URL
        /// </summary>
        public string NoticeUrl { get; set; }
    }
}
