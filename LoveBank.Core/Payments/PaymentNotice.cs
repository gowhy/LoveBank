using System;
using LoveBank.Common;

namespace LoveBank.Core.Payments
{
    /// <summary>
    /// 支付通知类,与第三方支付完全由此进行交互
    /// </summary>
    public class PaymentNotice : Entity
    {
        public PaymentNotice() {
            
        }

    

        /// <summary>
        /// Notice编号
        /// </summary>
        public string NoticeSn { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 是否支付
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 支付接口Key
        /// </summary>
        public string PaymentKey { get; set; }


        /// <summary>
        /// 备忘录
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 外部的Notice编号
        /// </summary>
        public string OuterNoticeSn { get; set; }

        public static string GenerateOrderNumber(string orderType)
        {
            var date = DateTime.Now;
            var ds = (int)date.TimeOfDay.Milliseconds;
            var rd = new Random().Next(1, 999);
            var number = "{3}{0}{1}{2}".FormatWith(date.ToString("yyMMddHHmmss"), ds.ToString("000"), rd.ToString("000"),orderType);
            return number;
        }
    }
}
