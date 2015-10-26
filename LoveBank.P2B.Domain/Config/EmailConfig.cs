using System;
using LoveBank.Common.Config;

namespace LoveBank.P2B.Domain.Config
{
    [Serializable]
    public class EmailConfig : IConfig
    {

        private string name=""; //发送的Email地址

        private string smtp; //smtp 地址

        private int port = 25; //端口号

        private string username="username";  //邮件帐号

        private string password;  //邮件密码

        private string prefix = "【】"; //邮件标题前缀

        private bool isSSL = false; //是否启用SSL


        /// <summary>
        /// 发送的Email地址
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// SMTP服务器
        /// </summary>
        public string SmtpServer { get { return smtp; } set { smtp = value; } }

        /// <summary>
        /// SMTP服务器端口
        /// </summary>
        public int SmtpPort { get { return port; } set { port = value; } }

        /// <summary>
        /// 用户名
        /// </summary>
        public string SmtpUserName { get { return username; } set { username = value; } }

        /// <summary>
        /// 密码
        /// </summary>
        public string SmtpPassword { get { return password; } set { password = value; } }

        /// <summary>
        /// Email标题前缀
        /// </summary>
        public string SubjectPrefix { get { return prefix; } set { prefix = value; } }

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool IsSSL { get { return isSSL; } set { isSSL = value; } }

    }
}
