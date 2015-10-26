namespace LoveBank.Common.Plugins.Email
{
    /// <summary>
    /// 发送电子邮件，默认都是异步发送，目前为实现同步方式
    /// </summary>
    public interface IEmailSender {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址,必须是有效的Email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">内容</param>
        void SendMail(string email,string title, string body);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址,必须是有效的Email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="userToken">传递的标识对象</param>
        /// <param name="callback">回调函数</param>
        void SendMail(string email, string title, string body, object userToken, CompletedCallback callback);

    }
}
