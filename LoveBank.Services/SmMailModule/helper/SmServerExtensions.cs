/*==============================================================
 * SmServerExtensions.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/20 17:25:37.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System.Collections.Generic;
using QDT.Common.Plugins;
using QDT.Core.Domain;
using Newtonsoft.Json;
namespace QDT.Services.SmMailModule.helper {
    public static class SmServerExtensions {
         public static SmsInfo ToSmsInfo(this SmServer smServer) {
             return new SmsInfo {
                 ClassName = smServer.ClassName,
                 Config =
                     string.IsNullOrEmpty(smServer.ConfigInfo)
                         ? null
                         : JsonConvert.DeserializeObject<Dictionary<string, SmsInfo.SmsConfig>>(smServer.ConfigInfo),
                 Pwd = smServer.Password,
                 ServerName = smServer.ServerName,
                 ServerUrl = smServer.ServerUrl,
                 SmsLang = null,
                 UserName = smServer.UserName

             };
         }
    }
}