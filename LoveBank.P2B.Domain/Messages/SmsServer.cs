using System.Collections.Generic;
using LoveBank.Common.Plugins;
using LoveBank.Core;

namespace LoveBank.P2B.Domain.Messages
{
    public class SmsServer:Entity,IAggregeRoot
    {
        public SmsServer()
        {
        }
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
        /// 账号
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string ConfigInfo { private set; get; }

        /// <summary>
        /// 动态配置信息
        /// </summary>
        public IDictionary<string,DynamicConfig> Config {
            get { return DynamicConfig.ConvertObject(ConfigInfo); }
            set { ConfigInfo = DynamicConfig.ConvertJson(value); }
        } 

    }
}
