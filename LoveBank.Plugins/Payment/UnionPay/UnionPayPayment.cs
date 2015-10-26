using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using com.unionpay.upop.sdk;

namespace LoveBank.Plugins.Payment
{
    public class UnionPayPayment : AbstractPayment
    {
        // 使用Dictionary保存参数
        private readonly Dictionary<string, string> param = new Dictionary<string, string>();

        private const string NAME = "银联在线支付";

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public override string Key{get { return this.GetType().Name; }}


        /// <summary>
        /// 名字
        /// </summary>
        public override string Name { get { return NAME; } }

        public UnionPayPayment() {
            
            OnlinePaly = true;

            InitConfig();
        }

        private void InitConfig() {

            Config = new Dictionary<string, PaymentConfig> {
                {"unionpay_account", new PaymentConfig("商户编号", InputType.Text)}, 
                {"unionpay_key", new PaymentConfig("商户密钥", InputType.Password)}
                                                           };

            var strm=this.GetType().Assembly.GetManifestResourceStream("LoveBank.Plugins.Payment.UnionPay.UnionPay.config");

            UPOPSrv.LoadConf(strm);

            param["transType"] = UPOPSrv.TransType.CONSUME;  //支付方式，默认支付
            param["commodityUrl"] = "http://www.qiandt.com"; //商品的URL,充值订单都是首页
            param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;

        }

        private void SetOrderNumber(string order) {

            param["orderNumber"] = order;  

        }

        private void SetMoney(decimal money) {
            param["orderAmount"] = money.ToString();
        }


        public override string GetDisplayHtml()
        {
            var html = new StringBuilder();
            html.AppendFormat("<input id= \"payment_{0}\" type=\"radio\" name=\"payment\" value=\"{0}\" class=\"radio-hide\" />", Key);
            html.AppendFormat("<label for=\"payment_{0}\">", Key);
            html.AppendFormat("<img src=\"/Plugins/Content/unionpay.jpg\" />");
            html.AppendFormat("<i class=\"iconfont\">&#x3439;</i>");
            html.Append("</label>");
            return html.ToString();
        }

        public override void SendRequest(PaymentOrder order)
        {
            param["transType"] = UPOPSrv.TransType.CONSUME;                         // 交易类型，前台只支持CONSUME 和 PRE_AUTH
            param["orderNumber"] = order.PaymentNo;                                 // 订单号，必须唯一
            param["orderAmount"] = Math.Round(order.Money*100).ToString();                          // 交易金额
            param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                          // 币种
            param["orderTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");           // 交易时间
            param["frontEndUrl"] = order.ReturnUrl;                                 // 前台回调URL
            param["backEndUrl"] = order.NoticeUrl;                                  // 后台回调URL
            param["merId"] = Config["unionpay_account"].Values.ToString();          // 商户编号
            param["customerIp"] = GetIP();

            UPOPSrv.Config.securityKey = Config["unionpay_key"].Values.ToString(); //设置密钥

            // 创建前台交易服务对象
            var srv = new FrontPaySrv(param);

            

            HttpContext.Current.Response.ContentEncoding = srv.Charset; // 指定输出编码
            HttpContext.Current.Response.Write(srv.CreateHtml());
            HttpContext.Current.Response.End();
        }

        public override string GetPaymentForm(PaymentOrder order)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyNotify(NameValueCollection form)
        {
            UPOPSrv.Config.securityKey = Config["unionpay_key"].Values.ToString();

            try {
                // 使用Post过来的内容构造SrvResponse
                var resp = new SrvResponse(Util.NameValueCollection2StrDict(form));

                if (resp.ResponseCode != SrvResponse.RESP_SUCCESS){

                    this.OnNotifyVerifyFaild(new PaymentOrder() { PaymentNo = resp.Fields["orderNumber"], Money = TrancMoney(resp.Fields["orderAmount"]), Description = "错误码[" + resp.Fields["respCode"] + "]" });
                    return false;
                }

                this.OnFinished(new PaymentOrder() { PaymentNo = resp.Fields["orderNumber"], Money = TrancMoney(resp.Fields["orderAmount"]) ,Description = "银联在线"});
                return true;

            }catch(Exception ex) {
                Log.Error(ex);
                return false;
            }
        }

        public override bool VerifyNotify(NameValueCollection form, out string returnContent) {
            var isOK = VerifyNotify(form);
            returnContent = isOK ? "ok" : "error";
            return isOK;
        }

        public static string GetIP()
        {
            var ip = "0.0.0.0";
            var request = HttpContext.Current.Request;
            ip = request.ServerVariables["HTTP_VIA"] != null ? request.ServerVariables["HTTP_X_FORWARDED_FOR"] : request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }

        private decimal TrancMoney(string orderAmount) {

            return decimal.Parse(orderAmount)/100;

        }
    }
}
