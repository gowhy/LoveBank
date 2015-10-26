using LoveBank.Common;

namespace LoveBank.P2B.Domain.Messages
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType {
        /// <summary>
        /// 短信
        /// </summary>
        [EnumItemDescription("短信")]
        SMS = 0,
        /// <summary>
        /// 邮件
        /// </summary>
        [EnumItemDescription("邮件")]
        Email = 1
    }
}