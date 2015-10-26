/*==============================================================
 * SmsPluginService.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/20 10:33:18.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using QDT.Common.Plugins;

namespace QDT.Services.SmMailModule {
    public class SmsPluginService:PluginService<ISmsPlugins> {
        public override void InstallPlugins(string key) {
            throw new System.NotImplementedException();
        }

        public override void UninstallPlugins(string key) {
            throw new System.NotImplementedException();
        }
    }
}