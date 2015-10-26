using System;
using QDT.Common;

namespace QDT.P2B.Domain.UserModule
{
    /// <summary>
    /// 用户操作日志信息类
    /// </summary>
    public class UserAccountRecord:Entity,IAggregeRoot
    {
        public UserAccountRecord()
        {
        }

        public UserAccountRecord(User user, string info)
            : this(user,info, 0, 0)
        {
        }

        public UserAccountRecord(User user, string info, decimal money, decimal lockmoney)
            : this(user,info, money, lockmoney, 0)
        {
        }

        public UserAccountRecord(User user,string info, decimal money, decimal lockmoney, int admin) {
            Check.Argument.IsNotNull(user,"user");
            UserID = user.ID;
            Info = info;
            Money = money;
            LockMoney = lockmoney;
            OperateAdminID = admin;
            Time = DateTime.Now;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; private set; }

        /// <summary>
        /// 业务备注
        /// </summary>
        public string Info { get; private set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// 资金变动
        /// </summary>
        public decimal Money { get; private set; }

        /// <summary>
        /// 冻结资金
        /// </summary>
        public decimal LockMoney { get;  private set; }

        /// <summary>
        /// 操作用户ID
        /// </summary>
        public int OperateUserID { get; private set; }

        /// <summary>
        /// 操作管理员ID
        /// </summary>
        public int OperateAdminID { get; private set; }

        public enum RecordType {
            /// <summary>
            /// 可用金充值
            /// </summary>
            Charge=0,

            /// <summary>
            /// 可用金扣除
            /// </summary>
            Deduct,


        }
    }
}
