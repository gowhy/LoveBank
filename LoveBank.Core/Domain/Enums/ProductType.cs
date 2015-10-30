using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public enum ProductType
    {
        [EnumItemDescription("实物")]
        实物 = 0,

        [EnumItemDescription("服务")]
        服务 = 1,

        [EnumItemDescription("代金券")]
        代金券 = 2

    }
}
