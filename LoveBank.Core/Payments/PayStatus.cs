using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Core.Payments
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PayStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        Unpaid=0,

        /// <summary>
        /// 部分支付
        /// </summary>
        Partial=1,

        /// <summary>
        /// 全部支付
        /// </summary>
        All=2

    }
}
