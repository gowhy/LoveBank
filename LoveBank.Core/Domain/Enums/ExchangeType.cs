using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{

    public enum ExchangeType
    {
        [EnumItemDescription("账号登陆兑换")]
        账号登陆兑换 = 0,

        [EnumItemDescription("二维码登陆兑换")]
        二维码登陆兑换 = 1,
        [EnumItemDescription("NFC兑换")]
        NFC兑换 = 2
    }
}
