using System;
using System.ComponentModel.DataAnnotations;
using LoveBank.Common;

namespace LoveBank.Core.Members
{
    /// <summary>
    /// 用户资金信息类
    /// </summary>
    public class UserAccount:Entity
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

        /// <summary>
        /// 投资人数据过期
        /// </summary>
        public bool LenderOver { get; set; }

        /// <summary>
        /// 借款人数据过期
        /// </summary>
        public bool BorrowerOver { get; set; }


        public byte[] TimeStamp { get; set; }

        public UserAccount()
        {
            Money = 0;
            Point = 0;
            Quota = 0;
        }
        
    }
}
