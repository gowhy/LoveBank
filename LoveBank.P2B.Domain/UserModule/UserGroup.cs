namespace QDT.P2B.Domain.UserModule
{
    /// <summary>
    /// 用户组类
    /// </summary>
    public class UserGroup : Entity, IAggregeRoot
    {
        /// <summary>
        /// 用户组名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级积分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 是否是系统设置组，系统设置组不允许修改
        /// </summary>
        public bool IsSystem { get; set; }
    }
}
