using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QDT.Common;

namespace QDT.Core.Domain
{
    /// <summary>
    /// 会员类
    /// </summary>
    public class User : Entity, IAggregeRoot
    {
        public User()
        {
            
        }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password.Hash();
            PayPassword = string.Empty;
            IsEffect = true;
            IsDelete = false;
            CreateTime = DateTime.Now;
            LoginTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            LoginIP = "0.0.0.0";

            Email = string.Empty;
            IDCard = string.Empty;
            RealName = string.Empty;
            Mobile = string.Empty;
            BindVerify = string.Empty;
        } 
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; private set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 用户组ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 是否无效
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 身份证是否通过验证
        /// </summary>
        public bool IDCardPassed { get; set; }

        /// <summary>
        /// 身份证验证通过时间
        /// </summary>
        public DateTime? IDCardPassedTime { get; set; }

        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 手机是否绑定
        /// </summary>
        public bool MobilePassed { get; set; }

        /// <summary>
        /// 手机绑定验证码
        /// </summary>
        public string BindVerify { get; set; }

        /// <summary>
        /// 手机绑定验证码有效期
        /// </summary>
        public DateTime? BindVerifyTime { get; set; }

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
        /// 推荐人ID
        /// </summary>
        public int PID { get; set; }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        public void ChangePwd(string password)
        {
            if(string.IsNullOrWhiteSpace(password)) throw new ApplicationException("密码不能为空");

            Password = password.Hash();
        }
    }
}
