using System;
using System.Net.Mail;

namespace LoveBank.Common.Plugins.Email {

    public class EmailSender : IEmailSender {

        private string _host;
        private int _port;
        private string _from;
        private string _name;
        private string _account;
        private string _password;
        private bool _withSsl;

        public EmailSender(string host,int port,string name,string account,string password,bool ssl) {

            _host = host;
            _port = port;
            _from = "service@mailmax.qiandt.com";
            _name = name;
            _account = account;
            _password = password;
            _withSsl = ssl;

        }

        private MailMessage BuildMessageWith(string toAddress, string subject, string body) {
            var message = new MailMessage {
                                              From = new MailAddress(_from, _name),
                                              Subject = subject,
                                              Body = body,
                                              IsBodyHtml = false,
                                          };

            string[] tos = toAddress.Split(';');

            foreach (string to in tos) {
                message.To.Add(new MailAddress(to));
            }

            return message;
        }

        private void SendMail(MailMessage message, object userToken, CompletedCallback callback) {

            try {

                var smtp = new Smtp(_host, _port, _withSsl, _account, _password);

                //委托中转
                if (callback != null) {
                    smtp.SmtpClient.SendCompleted += (sender, e) => callback(sender, e);
                }

                //发送异步邮件
                smtp.SmtpClient.SendAsync(message, userToken);

            }
            catch (Exception ex) {
                //Log.Exception(ex)
            }
        }

        #region Implementation of IEmailSender

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址,必须是有效的Email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">内容</param>
        public void SendMail(string email, string title, string body) {
            SendMail(email, title, body, null, null);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址,必须是有效的Email</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="userToken">传递的标识对象</param>
        /// <param name="callback">回调函数</param>
        public void SendMail(string email, string title, string body, object userToken, CompletedCallback callback) {

            var message = BuildMessageWith(email, title, body);

            SendMail(message, userToken, callback);
        }

        #endregion
    }
}