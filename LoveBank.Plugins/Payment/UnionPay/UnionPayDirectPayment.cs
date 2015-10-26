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
    public class UnionPayDirectPayment : AbstractPayment
    {
        // 使用Dictionary保存参数
        private readonly Dictionary<string, string> param = new Dictionary<string, string>();

        private const string NAME = "银联在线直连支付";

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public override string Key{get { return this.GetType().Name; }}


        /// <summary>
        /// 名字
        /// </summary>
        public override string Name { get { return NAME; } }

        public UnionPayDirectPayment() {
            
            OnlinePaly = true;

            InitConfig();
        }

        private void InitConfig() {

            Config = new Dictionary<string, PaymentConfig> {
                {"unionpay_account", new PaymentConfig("商户编号", InputType.Text)}, 
                {"unionpay_key", new PaymentConfig("商户密钥", InputType.Password)},
                {"unionpay_gateway",new PaymentConfig("支持的银行",InputType.Select){
                    Values=new CheckBoxGroup()
                    .Add("ICBCD","中国工商银行")
                    .Add("ABCD","中国农业银行")
                    .Add("CCBD","建设银行")
                    .Add("CMBD","招商银行")
                    .Add("SPDBD","浦发银行")
                    .Add("GDBD","广发银行")
                    .Add("PSBCD","邮政储蓄银行")
                    .Add("CMBCD","民生银行")
                    .Add("CEBD","光大银行")
                    .Add("HXB","华夏银行")
                }}
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
            html.Append("<style type='text/css'>.unionbank_types{float:left; display:block; background:url(/Plugins/Content/union-banks.png) no-repeat top left; font-size:0px; width:155px; height:22px; text-align:left; padding:15px 0px;}");
            html.Append(".bk_type_ICBCD{ background-position: 25px 0;}"); //中国工商银行
            html.Append(".bk_type_ABCD{ background-position: 25px -53px;}"); //中国农业银行
            html.Append(".bk_type_BOCSH{background-position: 25px -106px;}"); //中国银行
            html.Append(".bk_type_CCBD{background-position: 25px -159px;}"); //建设银行
            html.Append(".bk_type_CMBD{background-position: 25px -212px;}"); //招商银行
            html.Append(".bk_type_SPDBD{background-position: 25px -265px;}"); //浦发银行
            html.Append(".bk_type_GDBD{background-position: 25px -318px;}"); //广发银行
            html.Append(".bk_type_BOCOM{background-position: 25px -371px;}"); //交通银行
            html.Append(".bk_type_PSBCD{background-position: 25px -424px;}"); //邮政储蓄银行
            html.Append(".bk_type_CNCB{background-position: 25px -477px;}"); //中信银行
            html.Append(".bk_type_CMBCD{background-position: 25px -530px;}"); //民生银行
            html.Append(".bk_type_CEBD{background-position: 25px -583px;}"); //光大银行
            html.Append(".bk_type_HXB{background-position: 25px -636px;}"); //华夏银行
            html.Append(".bk_type_CIB{background-position: 25px -689px;}"); //兴业银行
            html.Append(".bk_type_BOS{background-position: 25px -742px;}"); //上海银行
            html.Append(".bk_type_SRCB{background-position: 25px -795px;}"); //上海农商
            html.Append(".bk_type_PAB{background-position: 25px -848px;}"); //平安银行
            html.Append(".bk_type_BCCB{background-position:25px -901px; }"); //北京银行
            html.Append("</style>");

            html.Append("<script type='text/javascript'>function set_bank(bank_id)");
            html.Append("{");
            html.Append("$(\"input[name='bank_id']\").val(bank_id);");
            html.Append("}</script>");

            var checkBoxGroup = this.Config["unionpay_gateway"].Values as CheckBoxGroup;
            if (checkBoxGroup != null)
            {
                foreach (var c in checkBoxGroup)
                {
                    if (c.Value.Checked)
                    {
                        html.Append("<label class='unionbank_types bk_type_" + c.Key + "'><input type='radio' name='payment' value='" + Key + "' rel='" + c.Key + "' onclick='set_bank(\"" + c.Key + "\")' /></label>");
                    }
                }
            }

            html.Append("<div style='clear:both;'></div>");
            html.Append("<input type='hidden' name='bank_id'/>");
            return html.ToString();
        }

        public override void SendRequest(PaymentOrder order)
        {
            param["transType"] = UPOPSrv.TransType.CONSUME;                         // 交易类型，前台只支持CONSUME 和 PRE_AUTH
            param["orderNumber"] = order.PaymentNo;                                 // 订单号，必须唯一
            param["orderAmount"] = Math.Round(order.Money * 100).ToString();        // 交易金额
            param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                          // 币种
            param["orderTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");           // 交易时间
            param["frontEndUrl"] = order.ReturnUrl;                                 // 前台回调URL
            param["backEndUrl"] = order.NoticeUrl;                                  // 后台回调URL
            param["merId"] = Config["unionpay_account"].Values.ToString();          // 商户编号
            param["customerIp"] = GetIP();

            //以下两个参数为网银直连支付参数
            param["defaultPayType"] = "CSPay";
            param["defaultBankNumber"] = order.BankCode;

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

                this.OnFinished(new PaymentOrder() { PaymentNo = resp.Fields["orderNumber"], Money = TrancMoney(resp.Fields["orderAmount"]),Description = "银联网银直连"});
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

        private decimal TrancMoney(string orderAmount)
        {

            return decimal.Parse(orderAmount) / 100;

        }
    }
}
