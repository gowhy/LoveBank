using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common;
using System.ComponentModel.DataAnnotations.Schema;
using LoveBank.Core.Domain;

namespace LoveBank.Core.Members
{
    /// <summary>
    /// 用户基本信息类
    /// </summary>
    public class User : Entity, IAggregeRoot
    {
        public User() {

        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get;  set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get;  set; }
      
        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }

        public string LoginToken { get; set; }
        public string Msg { get; set; }

        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime AddTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? RoleId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        //[ForeignKey("Department")]
        public string DeptId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? State { set; get; }
 
        public Role Role { get; set; }

        public Department Department { get; set; }
        public string Phone { get; set; }
    }
}
