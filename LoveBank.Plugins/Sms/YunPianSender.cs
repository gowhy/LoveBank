using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using LoveBank.Common.Plugins.Sms;

namespace LoveBank.Plugins.Sms
{
    public class YunPianSender : SMSSender
    {

        public const string Name = "云片网络";

        private const string SendMsgMethod = "send.json";
        private const string ServerUrl = "http://yunpian.com/v1/sms/";
        private string _apiKey;

        protected override void InitConfig(SMSAttribute attribute)
        {
            _apiKey = attribute.SmsAccount;
        }

        public override SMSAttribute GetConfig()
        {
            var attr = new SMSAttribute
            {
                Name = Name,
                TypeName = GetType().FullName,
                Author = "~",
                Config = new DynamicConfig[] {
                    }
            };
            return attr;
        }

        public override void Send(string phoneNo, string content)
        {
            const string url = ServerUrl + SendMsgMethod;
            var request = new HttpPost(url)
            {
                ContentEncoding = "UTF-8"
            };
            request.Params.Add("apikey", _apiKey);
            request.Params.Add("mobile", phoneNo);
            request.Params.Add("text", "【】" + content);

            var result = request.Request();

            var args = GetEventArgs(result, null);

            if (args.Error != null)
            {
                throw args.Error;
            }
        }

        public override void Send(string phoneNo, string content, object userToken, CompletedCallback callback)
        {
            const string url = ServerUrl + SendMsgMethod;
            var request = new HttpPost(url)
            {
                ContentEncoding = "UTF-8"
            };
            request.Params.Add("apikey", _apiKey);
            request.Params.Add("mobile", phoneNo);
            request.Params.Add("text",  "【】" + content);

            request.Request();
        }

        public override string CheckRemain()
        {
            return "~";
        }

        private AsyncCompletedEventArgs GetEventArgs(string result, object userToken)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return new AsyncCompletedEventArgs(new ApplicationException("调用远程短信发送接口，返回未知的状态错误"), false, userToken);
            }

            int index;

            var resultcode = JObject.Parse(result)["code"].Value<string>();

            if (int.TryParse(resultcode, out index))
            {
                var info = new ResultInfo();
                if (index == 0)
                {
                    return new AsyncCompletedEventArgs(null, false, userToken);
                }
                return new AsyncCompletedEventArgs(new ApplicationException(info.GetErrorInfo(index)), false, userToken);
            }

            return new AsyncCompletedEventArgs(new ApplicationException("未知错误"), false, userToken);
        }

        public class ResultInfo
        {
            public const string Success = "发送成功";
            public const string LimitIp = "限制IP访问";
            public const string NoMoney = "账户余额不足";
            public const string ErrorWord = "关键词屏蔽";
            public const string SameContent = "同一手机号重复提交相同的内容";
            public const string CanNotSend = "营销短信暂停发送";
            public const string OtherError = "其它错误";
            private readonly Dictionary<int, string> _message;

            public ResultInfo()
            {
                _message = new Dictionary<int, string> {
                                                           {
                                                               0, Success
                                                           },{
                                                                 -3,LimitIp
                                                              },{
                                                                    3,NoMoney
                                                                },{
                                                                      4,ErrorWord
                                                                   },{
                                                                         8,SameContent
                                                                      },{
                                                                            9,SameContent
                                                                        },{
                                                                              13,CanNotSend
                                                                          },
                                                       };
            }

            public string GetErrorInfo(int index)
            {
                return _message[index];
            }
        }

    }
}
