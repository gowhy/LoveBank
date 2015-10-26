using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{

    public enum TeamLevel
    {
        [EnumItemDescription("一星志愿者")]
        一星 = 0,

        [EnumItemDescription("二星志愿者")]
        二星 = 1,

        [EnumItemDescription("三星志愿者")]
        三星 = 2,

        [EnumItemDescription("四星志愿者")]
        四星 = 3,

        [EnumItemDescription("五星志愿者")]
        五星 = 4

    }
}
