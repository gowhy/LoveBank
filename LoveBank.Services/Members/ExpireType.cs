using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common;

namespace LoveBank.Services.Members
{
    public enum ExpireType
    {
        /// <summary>
        /// 借款人
        /// </summary>
        [EnumItemDescription("借款人")]
        Borrower = 0,
        /// <summary>
        /// 投资人
        /// </summary>
        [EnumItemDescription("投资人")]
        Lender = 1,
    }
}
