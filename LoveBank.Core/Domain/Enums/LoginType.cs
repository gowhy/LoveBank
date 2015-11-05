using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    public enum LoginType
    {
        [EnumItemDescription("手机号登陆")]
        手机 = 1,

        [EnumItemDescription("二维码登陆")]
        二维码登陆 = 2,

        [EnumItemDescription("NFC登陆")]
        NFC登陆 = 3
    }
}
