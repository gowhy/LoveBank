
using LoveBank.Common;

namespace LoveBank.Core.Domain.Enums {
    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendType {
        /// <summary>
        /// 按会员组
        /// </summary>
        [EnumItemDescription("会员组")]
        MemberGroup=0,
        /// <summary>
        /// 按自定义
        /// </summary>
        [EnumItemDescription("发送自定义")]
        Custom=2
    }
}