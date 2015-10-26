using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Plugins.Sms
{
    public interface ISmsSender {

        /// <summary>
        /// 获得短信的基本配置信息
        /// </summary>
        /// <returns></returns>
        SMSAttribute GetConfig();

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="content"></param>
        void Send(string phoneNo, string content);

        /// <summary>
        /// 发送异步短信
        /// </summary>
        /// <param name="phoneNo">发送的手机号</param>
        /// <param name="content">发送的内容</param>
        /// <param name="userToken">回调标识对象</param>
        /// <param name="callback">回调函数</param>
        void Send(string phoneNo, string content, object userToken,CompletedCallback callback);

        /// <summary>
        /// 检查短信剩余条数
        /// </summary>
        /// <returns></returns>
        string CheckRemain();
    }
}
