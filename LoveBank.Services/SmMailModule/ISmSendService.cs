/*==============================================================
 * ISmSendService.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/03/10 10:52:17.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using QDT.Common.Plugins;

namespace QDT.Services.SmMailModule {
    public interface ISmSendService {
        /// <summary>
        /// 业务短信队列发送
        /// </summary>
        void BussinessSmQueuSend();
        /// <summary>
        /// 业务短信发送
        /// </summary>
        SmsSendResult BusinessSmSend(int id);
        /// <summary>
        /// 推广短信队列发送
        /// </summary>
        void PromoteSmQueueSend();
        /// <summary>
        /// 推广短信发送
        /// </summary>
        SmsSendResult PromoteSmSend(int id);
        /// <summary>
        /// 发送测试短信
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        SmsSendResult SendDemo(string phoneNo);
    }
}