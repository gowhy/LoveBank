using LoveBank.Common;

namespace LoveBank.Core.Domain.Enums {
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType {
        /// <summary>
        /// 短信
        /// </summary>
        [EnumItemDescription("短信")]
        ShortMsg = 0,
        /// <summary>
        /// 邮件
        /// </summary>
        [EnumItemDescription("邮件")]
        Email = 1
    }
}