namespace QDT.P2B.Domain.UserModule
{
    /// <summary>
    /// 用户资金信息类
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// 资金余额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 冻结资金
        /// </summary>
        public decimal LockMoney { get; set; }

        /// <summary>
        /// 信用积分
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// 信用额度
        /// </summary>
        public decimal Quota { get; set; }
    }
}
