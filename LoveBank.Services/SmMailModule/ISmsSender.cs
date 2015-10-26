/*==============================================================
 * ISmsSender.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/20 16:48:04.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using QDT.Common.Plugins;

namespace QDT.Services.SmMailModule {
    public interface ISmsSender {
        SmsSendResult SendSm(string phoneNo, string content);
        string CheckFee();
    }
}