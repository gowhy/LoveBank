using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{


    public enum LoveFundType
    {
        [EnumItemDescription("爱心基金")]
        未知 = 0,
        [EnumItemDescription("组成")]
        组成 = 1,
        [EnumItemDescription("管理")]
        管理 = 2,
        [EnumItemDescription("使用及公开")]
        使用及公开 = 3,
        [EnumItemDescription("爱心人士/单位名单")]
        爱心人士单位名单 = 4

    }
}
