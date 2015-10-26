using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace LoveBank.Core.Domain
{
    public class AdminUser : Entity, IAggregeRoot
    {
        public AdminUser()
        {
            
        }
        public AdminUser(string name,string password)
        {
            UserName = name;
            Password = password;
            //IsEffect = true;
            //IsDelete = false;
            LoginTime = DateTime.Now;
            LoginIP = "0.0.0.0";
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        //public bool IsEffect { get; set; }

        //public bool IsDelete { get; set; }

        /// <summary>
        /// 是否是默认管理员，默认管理员拥有超级权限
        /// </summary>
        public bool IsDefaultAdmin { get; set; }

        public int RoleID { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginIP { get; set; }

        public string DeptId { set;get; }
        /// <summary>
        /// 
        /// </summary>
        public string RealName { set; get; }
        public string Phone { set; get; }

        public int? SocOrgId { set; get; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }
       

        public void ChangePwd(string newPassword)
        {
            Password = newPassword;
        }
    }
}
