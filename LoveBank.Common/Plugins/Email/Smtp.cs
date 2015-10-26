using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace LoveBank.Common.Plugins.Email
{

    public class Smtp {
        /// <summary>
        /// 返回内部构造的SmtpClient实例
        /// </summary>
        public SmtpClient SmtpClient { get; set; }

        #region   构造函数

        #region SMTP服务器 需要身份验证凭据

        /// <summary>
        /// 创建 Smtp 实例
        /// </summary>
        /// <param name="host">设置 SMTP 主服务器</param>
        /// <param name="port">端口号</param>
        /// <param name="enableSsl">指定 SmtpClient 是否使用安全套接字层 (SSL) 加密连接。</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public Smtp(string host, int port, bool enableSsl, string userName, string password)
        {
            Check.Argument.IsNotEmpty(host,"host");
            Check.Argument.IsNotEmpty(userName,"userName");
            Check.Argument.IsNotEmpty(password,"password");

            SmtpClient = new SmtpClient(host, port);

            SmtpClient.EnableSsl = enableSsl;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.Credentials = new NetworkCredential(userName, password);
            SmtpClient.Timeout = 100000;
        }

        /// <summary>
        /// 创建 Smtp 实例
        /// </summary>
        /// <param name="host">设置 SMTP 主服务器</param>
        /// <param name="port">端口号</param>
        /// <param name="enableSsl">指定 SmtpClient 是否使用安全套接字层 (SSL) 加密连接。</param>
        /// <param name="credential">设置用于验证发件人身份的凭据。</param>
        public Smtp(string host, int port, bool enableSsl, NetworkCredential credential)
        {
            Check.Argument.IsNotEmpty(host, "host");
            Check.Argument.IsNotNull(credential, "credential");

            SmtpClient = new SmtpClient(host, port);
            SmtpClient.EnableSsl = enableSsl;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.Credentials = credential;
            SmtpClient.Timeout = 100000;
        }
        #endregion

        #region SMTP服务器 根据 useDefaultCredentials 参数决定，SMTP服务器是否传输系统默认凭证

        // useDefaultCredentials
        // false：则连接到服务器时会将 Credentials 属性中设置的值用作凭据。
        //        如果UseDefaultCredentials属性设置为 false 并且尚未设置 Credentials 属性，则将邮件以匿名方式发送到服务器。
        //        若SMTP 服务器要求在验证客户端的身份则会抛出异常。。
        // true：System.Net.CredentialCache.DefaultCredentials （应用程序系统凭证）会随请求一起发送。

        /// <summary>
        /// 创建 Smtp 实例
        /// </summary>
        /// <param name="host">设置 SMTP 主服务器</param>
        /// <param name="port">端口号</param>
        /// <param name="enableSsl">指定 SmtpClient 是否使用安全套接字层 (SSL) 加密连接。</param>
        /// <param name="useDefaultCredentials">SMTP服务器是否传输系统默认凭证。</param>
        public Smtp(string host, int port, bool enableSsl, bool useDefaultCredentials)
        {
            Check.Argument.IsNotEmpty(host, "host");

            SmtpClient.Host = host;
            SmtpClient.Port = port;
            SmtpClient.EnableSsl = enableSsl;
            SmtpClient.UseDefaultCredentials = useDefaultCredentials;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.Timeout = 100000;
        }

        #endregion

        #endregion

        /// <summary>
        /// 设置SmtpClient.Send() 调用的超时时间。
        /// SmtpClient默认 Timeout =  （100秒=100*1000毫秒）。
        /// 应当根据“邮件大小、附件大小、加密耗时”等因素进行调整
        /// </summary>
        public Smtp SetTimeout(int timeout)
        {
            if (timeout > 0)
            {
                SmtpClient.Timeout = timeout;                
            }
            return this;
        }

        /// <summary>
        /// 设置 SmtpClient 如何处理待发的电子邮件。
        /// </summary>
        /// <param name="deliveryMethod">
        /// 0、Network（默认）：电子邮件通过网络发送到 SMTP 服务器。
        /// 1、SpecifiedPickupDirectory：将电子邮件复制到 SmtpClient.PickupDirectoryLocation 属性指定的目录，然后由外部应用程序传送。
        /// 2、PickupDirectoryFromIis：将电子邮件复制到拾取目录，然后通过本地 Internet 信息服务 (IIS) 传送。
        /// </param>
        public Smtp SetDeliveryMethod(int deliveryMethod)
        {
            if (deliveryMethod < 0 || deliveryMethod > 2)
                deliveryMethod = 0;     //  Network（默认）

            SmtpClient.DeliveryMethod = (SmtpDeliveryMethod)deliveryMethod;

            return this;
        }

        /// <summary>
        /// 添加建立安全套接字层 (SSL) 连接的证书
        /// </summary>
        public Smtp AddClientCertificate(X509Certificate certificate)
        {
            Check.Argument.IsNotNull(certificate, "certificate");

            SmtpClient.EnableSsl = true;
            SmtpClient.ClientCertificates.Add(certificate);

            return this;
        } 
    }
}