using System;
using System.Collections.Generic;
using System.ComponentModel;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using LoveBank.Common.Plugins.Sms;

namespace LoveBank.Plugins.Sms {
    public class InoLinkSender : SMSSender {
        public const string Name = "北京同创凌凯";

        private const string _queryBlance = "SelSum.aspx";
        private const string _sendMsgMethod = "BatchSend.aspx";
        private const string _serverUrl = "http://inolink.com/ws/";
        private string _pwd;
        private string _username;

        #region Implementation of ISmsSender

        protected override void InitConfig(SMSAttribute attribute) {
            _username = attribute.SmsAccount;
            _pwd = attribute.SmsPassword;
        }

        public override SMSAttribute GetConfig() {
            var attr = new SMSAttribute {
                                            Name = Name, TypeName = GetType().FullName, Author = "", Config = new DynamicConfig[] {
                                                                                                                                     }
                                        };
            return attr;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="content"></param>
        public override void Send(string phoneNo, string content) {
            string url = _serverUrl + "/" + _sendMsgMethod;
            var request = new HttpPost(url)
            {
                ContentEncoding = "gb2312"
            };
            request.Params.Add("CorpID", _username);
            request.Params.Add("Pwd", _pwd);
            request.Params.Add("Mobile", phoneNo);
            request.Params.Add("Content", content);
            request.Params.Add("Cell", "");
            request.Params.Add("SendTime","");


            string result = request.Request();

            AsyncCompletedEventArgs args = GetEventArgs(result, null);

            if (args.Error != null) {
                throw args.Error;
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo">发送的手机号</param>
        /// <param name="content">发送的内容</param>
        /// <param name="userToken">回调标识对象 </param>
        /// <param name="callback">回调函数</param>
        public override void Send(string phoneNo, string content, object userToken, CompletedCallback callback) {
            string url = _serverUrl + "/" + _sendMsgMethod;
            var request = new HttpPost(url) {
                                                ContentEncoding="gb2312"
                                            };
            request.Params.Add("CorpID", _username);
            request.Params.Add("Pwd", _pwd);
            request.Params.Add("Mobile", phoneNo);
            request.Params.Add("Content", content);
            request.Params.Add("Cell", "");
            request.Params.Add("SendTime","");

            try {
                string result = request.Request();

                callback(request, GetEventArgs(result, userToken));
            }
            catch (Exception ex) {
                callback(request, new AsyncCompletedEventArgs(ex, false, userToken));
            }
        }

        /// <summary>
        /// 检查短信剩余条数
        /// </summary>
        /// <returns></returns>
        public override string CheckRemain() {
            string url = _serverUrl + "/" + _queryBlance;
            var request = new HttpPost(url) {
                                                ContentEncoding = "UTF-8"
                                            };
            request.Params.Add("CorpID", _username);
            request.Params.Add("Pwd", _pwd);

            string result = request.Request();

            if (result != " ") {
                return "还剩" + result + "条";
            }
            if (result == "-1") {
                return "帐号未注册！";
            }
            if (result == "-2") {
                return "其他错误！";
            }
            if (result == " -3") {
                return "帐号密码不匹配！";
            }

            if (string.IsNullOrWhiteSpace(result)) {
                throw new ApplicationException("调用远程短信发送接口，返回未知的状态错误。");
            }
            return result;
        }

        #endregion

        //private string UserName = "TCLK02864";
        //private string Pwd = "qiandt.com";

        private AsyncCompletedEventArgs GetEventArgs(string result, object userToken) {
            if (string.IsNullOrWhiteSpace(result)) {
                return new AsyncCompletedEventArgs(new ApplicationException("调用远程短信发送接口，返回未知的状态错误"), false, userToken);
            }

            int index;

            if (int.TryParse(result, out index)) {
                var info = new ResultInfo();
                if (index == 1) {
                    return new AsyncCompletedEventArgs(null, false, userToken);
                }
                return new AsyncCompletedEventArgs(new ApplicationException(info.GetErrorInfo(index)), false, userToken);
            }

            return new AsyncCompletedEventArgs(new ApplicationException("未知错误"), false, userToken);
        }

        #region Nested type: ResultInfo

        public class ResultInfo {
            public const string NoRegister = "账号未注册";
            public const string Success = "发送成功";
            public const string OtherError = "其它错误";
            public const string AccountError = "帐号或密码错误";
            public const string NoMoney = "余额不足,请充值";
            public const string TimeFormatError = "定时发送时间不是有效的时间格式";
            public const string NoSignature = "提交信息末尾未签名，请添加中文的企业签名【 】";
            public const string ContentError = "发送内容需在1到300字之间";
            public const string PhoneNoNull = "发送号码为空";
            public const string TimeError = "定时时间不能小于系统当前时间";
            public const string LimitIp = "限制IP访问";
            private readonly Dictionary<int, string> _message;

            public ResultInfo() {
                _message = new Dictionary<int, string> {
                                                           {
                                                               1, Success
                                                           }, {
                                                                  -1, NoRegister
                                                              }, {
                                                                     -2, OtherError
                                                                 }, {
                                                                        -3, AccountError
                                                                    }, {
                                                                           -5, NoMoney
                                                                       }, {
                                                                              -6, TimeFormatError
                                                                          }, {
                                                                                 -7, NoSignature
                                                                             }, {
                                                                                    -8, ContentError
                                                                                }, {
                                                                                       -9, PhoneNoNull
                                                                                   }, {
                                                                                          -10, TimeError
                                                                                      }, {
                                                                                             -100, LimitIp
                                                                                         }
                                                       };
            }

            public string GetErrorInfo(int index) {
                return _message[index];
            }
        }

        #endregion
    }
}