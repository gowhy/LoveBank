using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveBank.Web.Models
{
    public class AccountSettingModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 旧的密码
        /// </summary>
        public string PasswordOld { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认新密码
        /// </summary>
        //[Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }
    }

    public class SafePasswordModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 旧的安全密码
        /// </summary>
        public string SafePasswordOld { get; set; }
        /// <summary>
        /// 新安全密码
        /// </summary>
        [Required]
        public string SafePassword { get; set; }
        /// <summary>
        /// 确认新安全密码
        /// </summary>
        [Required]
        //[Compare("SafePassword", ErrorMessage = "两次密码输入不一致")]
        public string SafeConfirmPassword { get; set; }
    }

    public class IdcardSettingModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [Required] 
        public string RealName { get; set; }
        /// <summary>
        /// 用户身份证号
        /// </summary>
        [Required]
        public string IdCard { get; set; }
    }

}