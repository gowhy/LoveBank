
using System.Collections.Generic;

namespace LoveBank.Common.Plugins {
    public class SmsInfo {
        public Dictionary<string, string> SmsLang { set; get; }
        public Dictionary<string, SmsConfig> Config { set; get; }
        public string ServerUrl { set; get; }
        public string ServerName { set; get; }
        public string UserName { set; get; }
        public string Pwd { set; get; }
        public string ClassName { set; get; }
        public class SmsConfig {
            public int InputType { set; get; }
            public dynamic Value { set; get; }
        }
    }
}