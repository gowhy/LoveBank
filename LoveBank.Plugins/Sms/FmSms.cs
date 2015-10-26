
using System.Collections.Generic;
using LoveBank.Common.Plugins;

namespace LoveBank.Plugins.Sms {
    /// <summary>
    /// 方维短信平台
    /// </summary>
    public class FmSms:ISmsPlugins {

        private readonly SmsInfo smsInfo;

        public FmSms(params dynamic[] info) {
            if(info.Length >0) {
                smsInfo = info[0] as SmsInfo;
            }
        }

        public FmSms()
        {
            smsInfo = new SmsInfo
            {
                Config = new Dictionary<string, SmsInfo.SmsConfig> {
                    {
                        "bizCode", new SmsInfo.SmsConfig {
                            InputType = 0, Value = 1000
                        }
                    }
                },
                SmsLang = new Dictionary<string, string>() {
                    {
                        "bizCode", "业务代码"
                    }
                },
                ClassName =  this.GetType().Name,
                ServerName = "短信接口测试平台",
                ServerUrl = "http://222.77.181.24/sms/sendSMS.do"
            };
        }

        public SmsSendResult SendSm(string phoneNo, string content)
        {
            return null;
        }

        public SmsInfo GetSmsInfo() {
            return smsInfo;
        }

        public string CheckFee() {
            throw new System.NotImplementedException();
        }

        public void ModifyPwd(string oldPwd, string newPwd) {
            throw new System.NotImplementedException();
        }
    }
}