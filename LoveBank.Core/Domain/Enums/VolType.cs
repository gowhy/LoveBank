using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum VolType
    {
        [EnumItemDescription("一般志愿者")]
        一般志愿者 = 0,

        [EnumItemDescription("党员志愿者")]
        党员志愿者 = 1,

        [EnumItemDescription("技术志愿者")]
        技术志愿者 = 2,

        [EnumItemDescription("文化志愿者")]
        文化志愿者 = 3,

        [EnumItemDescription("专业志愿者")]
        专业志愿者 = 4,

        [EnumItemDescription("特殊志愿者")]
        特殊志愿者 = 5,
        [EnumItemDescription("复退军人")]
        复退军人 = 6,
        [EnumItemDescription("其他")]
        其他 = -1
    }
}
