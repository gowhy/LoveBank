/*==============================================================
 * SmsSender.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/20 16:47:36.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using System.Linq;
using QDT.Common.Plugins;
using QDT.Core;
using QDT.Core.Domain;
using QDT.Services.SmMailModule.helper;

namespace QDT.Services.SmMailModule {
    public class SmsSender:ServiceBase,ISmsSender {
        private readonly ISmsPlugins _smsPlugins;

        public SmsSender() {
            var smsInfo = DbProvider.D<SmServer>().FirstOrDefault(o => o.IsEffect);
            if(smsInfo == null) {
                throw new Exception("请先安装短信接口");
            }
            var sms = new SmsPluginService().GetPlugins(smsInfo.ClassName, smsInfo.ToSmsInfo());
            _smsPlugins = sms;
        }

        public SmsSendResult SendSm(string phoneNo, string content) {
           return  _smsPlugins.SendSm(phoneNo,content);
        }

        public string CheckFee() {
            return _smsPlugins.CheckFee();
        }
    }
}