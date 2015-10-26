/*==============================================================
 * PaymentInfo.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/21 17:20:01.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System.Collections.Generic;

namespace QDT.Common.Plugins {
    public class PaymentInfo {
        /// <summary>
        /// 配置信息
        /// </summary>
        public Dictionary<string, ConfigInfo> Config { set; get; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { set; get; }
        /// <summary>
        /// 支付方式：1：在线支付；0：线下支付 
        /// </summary>
        public int OnlinePaly { set; get; }
        /// <summary>
        /// 注册地址
        /// </summary>
        public string RegUrl { set; get; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }

        public class SelectItem {
            public SelectItem(string display, string value, bool isSelected)
            {
                Display = display;
                Value = value;
                IsSelected = isSelected;
            }

            public SelectItem(string display, string value):this(display,value,false) {}

            public string Display { set; get; }
            public string Value { set; get; }
            public bool IsSelected { set; get; }
        }

        public class ConfigInfo {
            /// <summary>
            /// 显示名
            /// </summary>
            public string Title { set; get; }
            /// <summary>
            /// 控件类型
            /// </summary>
            public int InputType { set; get; }
            /// <summary>
            /// 值
            /// </summary>
            public dynamic Values { set; get; }
        }
    }
}