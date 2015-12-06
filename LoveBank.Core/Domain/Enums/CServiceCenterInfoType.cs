using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum CServiceCenterInfoType
    {
        [EnumItemDescription("简介")]
        简介 = 1,
        [EnumItemDescription("服务项目介绍")]
        服务项目介绍 = 2,
        [EnumItemDescription("社工风采")]
        社工风采 = 3
    }
}
