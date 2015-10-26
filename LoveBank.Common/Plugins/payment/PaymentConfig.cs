using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Plugins
{
    /// <summary>
    /// 配置信息类
    /// </summary>
    [Serializable]
    public class PaymentConfig
    {
        public PaymentConfig() { }

        public PaymentConfig(string title, int input)
            : this(title, input, null)
        {
        }

        public PaymentConfig(string title, int input, object value)
        {
            this.Title = title;
            this.InputType = input;
            this.Values = value;
        }
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
        public object Values { set; get; }

    }
}
