using QDT.Common;

namespace QDT.P2B.Domain.ProjectModule
{
    public enum ProjectStatus
    {
        /// <summary>
        /// 等待材料
        /// </summary>
        [EnumItemDescription("等待材料")]
        Wait = 0,
        /// <summary>
        /// 进行中
        /// </summary>
        [EnumItemDescription("进行中")]
        Progress = 1,

        /// <summary>
        /// 满标
        /// </summary>
        [EnumItemDescription("满标")]
        Full = 2,

        /// <summary>
        /// 流标
        /// </summary>
        [EnumItemDescription("流标")]
        Bad = 3,

        /// <summary>
        /// 还款中
        /// </summary>
        [EnumItemDescription("还款中")]
        Repaying = 4,

        /// <summary>
        /// 已还清
        /// </summary>
        [EnumItemDescription("已还清")]
        Repaid = 5
    }
}
