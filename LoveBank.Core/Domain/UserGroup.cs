using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDT.Core.Domain
{
    /// <summary>
    /// 会员组
    /// </summary>
    public class UserGroup : Entity, IAggregeRoot
    {
        /// <summary>
        /// 会员组名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级积分
        /// </summary>
        public int Score { get; set; }
    }
}
