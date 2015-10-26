using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LoveBank.Core.Members;

namespace LoveBank.Web.Models
{
    public class UserRegisterModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "输入电子邮箱格式不正确，请重新输入！")]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        //[Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }

        public string Validate { get; set; }

    }

    public class UserRegModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "输入电子邮箱格式不正确，请重新输入！")]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

    }

    public class UserRegPlusModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        //[Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required]
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IdCard { get; set; }
    }

    public class UserLoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Validate { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class IsLoginModel
    {
        public int UserId { get; set; }
        public bool IsLogin { get; set; }
        public string UserName { get; set; }

        public IsLoginModel(bool isLogin, User user)
        {
            if(user!=null){
                IsLogin = isLogin;
                UserId = isLogin ? user.ID : 0;
                UserName = isLogin ? user.UserName : "";
            }else {
                IsLogin = false;
            }
        }
    }
}