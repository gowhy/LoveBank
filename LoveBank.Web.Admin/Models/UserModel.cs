using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace LoveBank.Web.Admin.Models
{
    /// <summary>
    /// 会员添加模型
    /// </summary>
    public class UserAddModel
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        [Required]
        public int UserType { get; set; }

        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        //[Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfrimPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 会员组ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 出身年份
        /// </summary>
        public int? BirthYear { get; set; }

        /// <summary>
        /// 出身月份
        /// </summary>
        public int? BirthMonth { get; set; }

        /// <summary>
        /// 出身日期
        /// </summary>
        public int? BirthDay { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { get; set; }

    }

    public class UserEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户类型，0：借款人，1：投资人
        /// </summary>
        [Required]
        public int UserType { get; set; }

        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string ConfrimPassword { get; set; }

        public string SafePassword { get; set; }

        //[Compare("SafePassword", ErrorMessage = "两次密码输入不一致")]
        public string ConfrimSafePassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 会员组ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 身份证号验证
        /// </summary>
        public bool IDCardPassed { get; set; }

        /// <summary>
        /// 出身年份
        /// </summary>
        public int? BirthYear { get; set; }

        /// <summary>
        /// 出身月份
        /// </summary>
        public int? BirthMonth { get; set; }

        /// <summary>
        /// 出身日期
        /// </summary>
        public int? BirthDay { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { get; set; }
    }
}