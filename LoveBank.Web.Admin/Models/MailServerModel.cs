

using LoveBank.Core.Domain;
namespace LoveBank.Web.Admin.Models {
    public class MailServerModel {
        /// <summary>
        /// 种子标识
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// Smtp服务地址
        /// </summary>
        public string SmtpServer { set; get; }
        /// <summary>
        /// 账号
        /// </summary>
        public string SmtpName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string SmtpPassword { set; get; }
        /// <summary>
        /// 是否使用SSL
        /// </summary>
        public bool IsSsl { set; get; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int SmtpPort { set; get; }
        /// <summary>
        /// 可用次数
        /// </summary>
        public int UseLimit { set; get; }
        /// <summary>
        /// 总使用次数
        /// <remarks>
        /// 可用次数为0时表示无限次数使用
        /// </remarks>
        /// </summary>
        public int TotalUse { set; get; }
        /// <summary>
        /// 是否自动清零
        /// <remarks>在设置可用次数时，当发数用完后将清空已发送的次数，可使该邮箱重新被使用</remarks>
        /// </summary>
        public bool IsReset { set; get; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { set; get; }
        /// <summary>
        /// 是否需要验证身份
        /// </summary>
        public bool IsVerify { set; get; }

        public MailServer ToEntity() {
            return new MailServer {
               ID = Id,
               IsEffect = IsEffect,
               IsReset = IsEffect,
               IsSsl = IsSsl,
               IsVerify =  IsVerify,
               SmtpName = SmtpName,
               SmtpPassword = SmtpPassword,
               SmtpPort = SmtpPort,
               SmtpServer = SmtpServer,
               TotalUse = TotalUse
            };
        }
    }
}