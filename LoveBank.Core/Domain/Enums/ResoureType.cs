using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    /// <summary>
    /// 共用的资料枚举，以后还继续扩展
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 用户上传的资源类型为图片
        /// </summary>
        图片 = 0,
        /// <summary>
        /// 用户上传资源为视频
        /// </summary>
        视频 = 1
    }
}
