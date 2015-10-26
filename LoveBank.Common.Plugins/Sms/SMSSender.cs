using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LoveBank.Common.Plugins.Sms
{
    public abstract class SMSSender:ISmsSender
    {
        public static SMSSender CreateInstance(string type) {

            var plugin = SMSPlugins.Instance().GetPlugin(type);

            var sender = Activator.CreateInstance(plugin) as SMSSender;

            return sender;
        }

        public static SMSSender CreateInstance(SMSAttribute configAttr)
        {
            if (configAttr==null)
            {
                return null;
            }
            var plugin = SMSPlugins.Instance().GetPlugin(configAttr.TypeName);
            if (plugin == null)
            {
                return null;
            }

            var sender = Activator.CreateInstance(plugin) as SMSSender;

            if ((sender != null))
            {
                sender.InitConfig(configAttr);
            }
            return sender;
        }

        protected abstract void InitConfig(SMSAttribute attribute);


        #region Implementation of ISmsSender

        /// <summary>
        /// 获得短信的基本配置信息
        /// </summary>
        /// <returns></returns>
        public abstract SMSAttribute GetConfig();

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="content"></param>
        public abstract void Send(string phoneNo, string content);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo">发送的手机号</param>
        /// <param name="content">发送的内容</param>
        /// <param name="userToken">回调用户标识</param>
        /// <param name="callback">回调函数</param>
        public abstract void Send(string phoneNo, string content,object userToken, CompletedCallback callback);

        /// <summary>
        /// 检查短信剩余条数
        /// </summary>
        /// <returns></returns>
        public abstract string CheckRemain();

        #endregion
    }
}
