using System;
using QDT.Common;

namespace QDT.P2B.Domain.UserModule
{
    /// <summary>
    /// 用户基本信息类
    /// </summary>
    public class User : Entity, IAggregeRoot
    {
        public User() {

            Sex = UserSex.Normal;

            Password = string.Empty;

            Email = string.Empty;

            Mobile = string.Empty;

            IDCard = string.Empty;

            SafePassword = string.Empty;

            CreateTime = DateTime.Now;

            UpdateTime = DateTime.Now;

            LoginTime = DateTime.Now;

            LoginIP = "0.0.0.0";

            UserAccount=new UserAccount();
        }

        /// <summary>
        /// 构建用户
        /// </summary>
        /// <param name="group">用户组</param>
        /// <param name="username">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">用户密码</param>
        /// <param name="isEffect">是否激活</param>
        /// <param name="islender">是出借人</param>
        /// <param name="isborrower">是借款人</param>
        public User(UserGroup group, string username, string email, string password, bool isEffect = true, bool islender = true, bool isborrower = false)
            : this()
        {

            IsLender = islender;

            IsBorrower = isborrower;

            //如果组ID不存在，表示未分组
            GroupID = group==null ? 0 :group.ID;

            UserName = username;

            Email = email;

            Password = password.Hash();

            IsEffect = isEffect;

        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 出生年份
        /// </summary>
        public int BirthYear { get; set; }

        /// <summary>
        /// 出生月份
        /// </summary>
        public int BirthMonth { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public int BirthDay { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

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
        /// 最后登录IP
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
        /// 是否绑定邮箱
        /// </summary>
        public bool EmailPassed { get; private set; }

        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 是否实名认证
        /// </summary>
        public bool IDCardPassed { get; private set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get;set; }

        /// <summary>
        /// 是否绑定手机
        /// </summary>
        public bool MobilePassed { get; private set; }

        /// <summary>
        /// 安全密码
        /// </summary>
        public string SafePassword { get; private set; }

        /// <summary>
        /// 是否设置安全密码
        /// </summary>
        public bool SafePasswordPassed { get; private set; }

        /// <summary>
        /// 是否是出借人
        /// </summary>
        public bool IsLender { get; set; }

        /// <summary>
        /// 是否是借款人
        /// </summary>
        public bool IsBorrower { get; set; }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        public int RecommendID { get; set; }

        /// <summary>
        /// 用户账户信息
        /// </summary>
        public UserAccount UserAccount { get; private set; }


        /// <summary>
        /// 修改密码，旧密码必须要等于新密码，否则返回False
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <exception cref="ArgumentNullException">密码输入为null或者空字符串</exception>
        /// <exception cref="UserPasswordException">密码修改错误</exception>
        public void ChangePassword(string oldPwd, string newPwd)
        {
            if (string.IsNullOrEmpty(oldPwd))
                throw new ArgumentNullException("oldPwd", "密码不能为null或者空字符串.");

            if (string.IsNullOrEmpty(newPwd))
                throw new ArgumentNullException("newPwd", "密码不能为null或者空字符串.");

            if (oldPwd.Hash() != this.Password)
                throw new UserPasswordException(UserPasswordException.InvaildOldPassword);

            this.Password = newPwd.Hash();
        }

        /// <summary>
        /// 修改密码，谨慎使用此接口
        /// </summary>
        /// <param name="newPwd">新密码</param>
        public void ChangePassword(string newPwd)
        {
            if (string.IsNullOrEmpty(newPwd)) throw new ArgumentNullException("newPwd", "密码不能为null或者空字符串.");

            this.Password = newPwd.Hash();

        }

        private const string randomChars = "ABCDEFGHJKMPQRTVWXYabcdefjhjkmnprstvwxy2346789";
        private const int randomLength = 8;

        /// <summary>
        /// 重设随机密码，并返回密码字符串
        /// </summary>
        /// <returns></returns>
        public string ResetPassword()
        {
            string password = string.Empty;

            int randomNum;

            Random random = new Random();
            for (int i = 0; i < randomLength; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }

            this.Password = password;

            return password;
        }

        /// <summary>
        /// 修改安全密码
        /// </summary>
        /// <param name="oldPwd">旧安全密码</param>
        /// <param name="newPwd">新的安全密码</param>
        /// <exception cref="ArgumentNullException">密码输入为null或者空字符串</exception>
        /// <exception cref="UserPasswordException">密码修改错误</exception>
        public void ChangeSafePassword(string oldPwd, string newPwd)
        {
            if (string.IsNullOrEmpty(newPwd)) throw new ArgumentNullException("newPwd", "密码不能为null或者空字符串.");

            //安全密码还没有被激活过
            if (!this.SafePasswordPassed) throw new UserPasswordException(UserPasswordException.UnactivatedSafePassword);

            //旧密码错误
            if (oldPwd.Hash() != this.SafePassword) throw new UserPasswordException(UserPasswordException.InvaildOldPassword);


            this.SafePassword = newPwd.Hash();

        }

        /// <summary>
        /// 初次绑定安全密码，在SafePasswordPassed为False有效，如果已经设置，请用ChangeSafePassword修改或者用ClearSafePassword移除
        /// </summary>
        /// <param name="password"></param>
        /// <exception cref="ArgumentNullException">密码输入为null或者空字符串</exception>
        /// <exception cref="UserPasswordException">安全密码已经设置或者设置无效</exception>
        public void BindSafePassword(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password", "密码不能为null或者空字符串.");

            //安全密码已经激活，不能重复设置
            if (this.SafePasswordPassed) throw new UserPasswordException(UserPasswordException.DuplicateSafePassword);


            this.SafePassword = password.Hash();
            this.SafePasswordPassed = true;
        }

        /// <summary>
        /// 移除安全密码设置
        /// </summary>
        public void ClearSafePassword()
        {
            this.SafePasswordPassed = false;
            this.SafePassword = string.Empty;
        }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="mobile"></param>
        public void BindMobile(string mobile)
        {
            this.Mobile = mobile;
            this.MobilePassed = true;
        }

        /// <summary>
        /// 清除手机绑定
        /// </summary>
        public void ClearMobile()
        {
            this.MobilePassed = false;
            this.Mobile = string.Empty;
        }

        /// <summary>
        /// 绑定身份实名认证
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="realname"></param>
        public void BindIDCard(string idcard, string realname)
        {
            this.RealName = realname;
            this.IDCard = idcard;
            this.IDCardPassed = true;
        }

        /// <summary>
        /// 清楚身份绑定
        /// </summary>
        public void ClearIDCard()
        {
            this.IDCardPassed = false;
        }
    }
}
