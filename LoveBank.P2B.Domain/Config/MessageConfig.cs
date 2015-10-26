using System;
using LoveBank.Common.Config;

namespace LoveBank.P2B.Domain.Config
{
    [Serializable]
    public class MessageConfig : IConfig {

        private int _dueTime = 5000;
        private bool _enable = true;
        private bool _autoBid = false;

        /// <summary>
        /// 开启消息服务
        /// </summary>
        public bool Enable { get { return _enable; } set { _enable = value; } }

        /// <summary>
        /// 消息间隔(毫秒)
        /// </summary>
        public int DueTime { get { return _dueTime; } set { _dueTime = value; } }

        /// <summary>
        /// 开启SMS
        /// </summary>
        public bool SmsOpen { get; set; }

        /// <summary>
        /// 开启邮件
        /// </summary>
        public bool EmailOpen { get; set; }


        /// <summary>
        /// 默认的短信服务
        /// </summary>
        public string SmsType { get; set; }

        /// <summary>
        /// 自动投标
        /// </summary>
        public bool AutoBid { get { return _autoBid; } set { _autoBid = value; } }
        
    }

}
