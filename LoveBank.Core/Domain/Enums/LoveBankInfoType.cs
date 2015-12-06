using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum LoveBankInfoType
    {
        [EnumItemDescription("简介")]
        简介 = 1,
        [EnumItemDescription("制度")]
        制度 = 2,
        [EnumItemDescription("组织架构")]
        组织架构 = 3,
        [EnumItemDescription("志愿服务类别及内容")]
        志愿服务类别及内容 = 4,

        [EnumItemDescription("回馈机制")]
        回馈机制 = 5
    }
}
