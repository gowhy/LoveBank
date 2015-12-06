using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum ConvenienceServicesType
    {
        [EnumItemDescription("行政便民-办事所需资料及其他信息")]
        行政便民办事所需资料及其他信息 = 1,

        [EnumItemDescription("行政便民-最新福利")]
        行政便民最新福利 = 2,

        [EnumItemDescription("行政便民-救助等政策")]
        行政便民救助等政策 = 3,

        [EnumItemDescription("社区便民-社区商业资源介绍")]
        社区便民社区商业资源介绍 = 4,

        [EnumItemDescription("社区便民-爱心企业活动")]
        社区便民爱心企业活动 = 5,

        [EnumItemDescription("社区便民-其他")]
        社区便民其他 = 6
    }
}
