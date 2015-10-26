
using LoveBank.Common;

namespace LoveBank.Core.Domain.Enums {
    /// <summary>
    /// 短信类型
    /// </summary>
    public enum SmsType {
        /// <summary>
        /// 普通推广短信
        /// </summary>
        [EnumItemDescription("普通推广短信")]
        Promote=0,
        /// <summary>
        /// 贷款通知短信
        /// </summary>
        [EnumItemDescription("借款通知")]
        Notice=1
    }
}