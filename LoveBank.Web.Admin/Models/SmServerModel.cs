
using LoveBank.Common.Plugins;
using LoveBank.P2B.Domain.Messages;
using System.Collections.Generic;


namespace LoveBank.Web.Admin.Models {

    public class SmsServerModel {

        public SmsServerModel(SmsServer sms) {
            Id = sms.ID;
            ServerName = sms.ServerName;
            Description = sms.Description;
            ClassName = sms.ClassName;
            UserName = sms.UserName;
            Password = sms.Password;
            Config = sms.Config;
            IsInstall = true;
        }
        public SmsServerModel() {
            IsInstall = false;
        }

        public int Id { set; get; }
        /// <summary>
        /// 通道名
        /// </summary>
        public string ServerName { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 接口名
        /// </summary>
        public string ClassName { set; get; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerUrl { set; get; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsDefault { set; get; }
        /// <summary>
        /// 是否安装
        /// </summary>
        public bool IsInstall { set; get; }

        public Dictionary<string, string> SmsLang { set; get; }

        public IDictionary<string, DynamicConfig> Config { set; get; }

        public SmsServer ToEntity() {
            return new SmsServer {
                ClassName = ClassName,
                ID =Id,
                ServerName = ServerName,
                Description = Description,
                Password = Password,
                UserName = UserName
            };
        }
    }
}