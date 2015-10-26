
using LoveBank.Common;

namespace LoveBank.Core.Domain.Enums {
    /// <summary>
    /// 短信发送状态
    /// </summary>
    public enum SendStatus {
        /// <summary>
        /// 未发送
        /// </summary>
         [EnumItemDescription("未发送")]
         Normal=0,
        /// <summary>
        /// 发送中
        /// </summary>
        [EnumItemDescription("发送中")]
         Sending=1,
        /// <summary>
        /// 已发送
        /// </summary>
        [EnumItemDescription("已发送")]
         Sended=2

    }
}