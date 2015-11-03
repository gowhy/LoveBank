using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum LBStartOrBottomAdPostion
    {
        
        [EnumItemDescription("开机启动广告")]
        开机启动广告 = 1,

        [EnumItemDescription("底部广告")]
        底部广告 = 2
    }
}
