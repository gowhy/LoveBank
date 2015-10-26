using LoveBank.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public enum QRCodeType
    {

        [EnumItemDescription("扫码登陆")]
        登陆 = 0,

        [EnumItemDescription("扫码兑换")]
        兑换 = 1

    }
}
