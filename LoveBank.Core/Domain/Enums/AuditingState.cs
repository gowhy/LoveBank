using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum AuditingState
    {
        [EnumItemDescription("全部")]
        全部 = -1,

        [EnumItemDescription("待审核")]
        待审核 = 0,

        [EnumItemDescription("审核通过")]
        审核通过 = 1,

        [EnumItemDescription("审核不通过")]
        审核不通过 = 2
    }
}
