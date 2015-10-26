using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public enum QRCodeState
    {

        [EnumItemDescription("待授权登陆")]
        待授权 = 0,

        [EnumItemDescription("已授权登陆")]
        已授权 = 1,

        [EnumItemDescription("已作废")]
        已作废 = 2
    }
}
