using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Code.Enums
{
    public enum InvestRoute
    {
        /// <summary>
        /// 所有的投资
        /// </summary>
        AllInvestList = 0,
        /// <summary>
        /// 正在还款的投资
        /// </summary>
        RepayInvest = 1,
        /// <summary>
        /// 完成的投资
        /// </summary>
        CompletedInvest = 2,
    }
}