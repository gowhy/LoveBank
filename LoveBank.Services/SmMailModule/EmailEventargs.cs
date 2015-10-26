/*==============================================================
 * EmailEventargs.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/18 16:18:43.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using QDT.Core.Domain;

namespace QDT.Services.SmMailModule {
    public class EmailEventargs:EventArgs {
        public EmailEventargs(PromoteMsg data) {
            Data = data;
        }

        public PromoteMsg Data { set; get; } 
    }
}