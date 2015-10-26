using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using LoveBank.Common;
using LoveBank.Common.Plugins;

namespace LoveBank.Plugins.Payment
{
    public class ChinaBankPayment : AbstractPayment 
    {
        private const string Gateway = "https://pay3.chinabank.com.cn/PayGate";
        private const string v_moneytype = "CNY";

        private const string NAME = "网银在线支付";

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public override string Key { get { return this.GetType().Name; } }

        /// <summary>
        /// 名字
        /// </summary>
        public override string Name { get { return NAME; } }

        public ChinaBankPayment()
        {
            OnlinePaly = true;
            InitConfig();
        }

        private void InitConfig() {
            Config = new Dictionary<string, PaymentConfig> {
                {"chinabank_account", new PaymentConfig("商户编号", InputType.Text)}, 
                {"chinabank_key", new PaymentConfig("商户密钥", InputType.Text)},
                                                           };
        }

        /// <summary>
        /// 取得插件在前端显示的HTML
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayHtml() {
            var html = new StringBuilder();
            html.AppendFormat("<input id= \"payment_{0}\" type=\"radio\" name=\"payment\" value=\"{0}\" class=\"radio-hide\" />", Key);
            html.AppendFormat("<label for=\"payment_{0}\">", Key);
            html.AppendFormat("<img src=\"/Plugins/Content/chinabank.jpg\" />");
            html.AppendFormat("<i class=\"iconfont\">&#x3439;</i>");
            html.Append("</label>");
            return html.ToString();
        }

        /// <summary>
        /// 发送支付请求到支付网关
        /// </summary>
        /// <returns></returns>
        public override void SendRequest(PaymentOrder order) {
            var v_mid = Config["chinabank_account"].Values.ToString();
            var v_oid = order.PaymentNo;
            var v_amount = order.Money.ToString("#.00");
            var v_url = order.ReturnUrl;
            var v_autoReceive = "[url:={0}]".FormatWith(order.NoticeUrl);
            var key = Config["chinabank_key"].Values.ToString();

            var md5str = v_amount + v_moneytype + v_oid + v_mid + v_url + key;

            string v_md5info = md5str.Hash();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(this.CreateField("v_mid", v_mid));
            stringBuilder.Append(this.CreateField("v_oid", v_oid));
            stringBuilder.Append(this.CreateField("v_amount", v_amount));
            stringBuilder.Append(this.CreateField("v_moneytype", v_moneytype));
            stringBuilder.Append(this.CreateField("v_url", v_url));
            stringBuilder.Append(this.CreateField("remark1", "ChinaBankDirect"));
            stringBuilder.Append(this.CreateField("remark2", v_autoReceive));
            stringBuilder.Append(this.CreateField("v_md5info", v_md5info));

            this.SubmitPaymentForm(this.CreateForm(stringBuilder.ToString(), Gateway));
        }


        /// <summary>
        /// 获得支付请求的Form表单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override string GetPaymentForm(PaymentOrder order) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证交易是否被篡改
        /// </summary>
        /// <param name="form">第三方支付传递的参数</param>
        /// <returns></returns>
        public override bool VerifyNotify(System.Collections.Specialized.NameValueCollection form)
        {
            var v_oid = form["v_oid"];
            var v_pstatus = form["v_pstatus"];
            var v_pstring = form["v_pstring"];
            var v_pmode = form["v_pmode"];
            var v_md5str = form["v_md5str"];
            var v_amount = form["v_amount"];
            var moneytype = form["v_moneytype"];
            var remark1 = form["remark1"];
            var remark2 = form["remark2"];

            if (v_oid == null || v_pstatus == null || v_pstring == null || v_pmode == null || v_md5str == null || v_amount == null || moneytype == null)
            {
                this.OnNotifyVerifyFaild(new PaymentOrder() { PaymentNo = v_oid, Description = "返回参数为Null" });
                return false;
            }

            var key = Config["chinabank_key"].Values.ToString();

            var md5str = (v_oid + v_pstatus + v_amount + moneytype + key).Hash();

            if (md5str != v_md5str)
            {
                this.OnNotifyVerifyFaild(new PaymentOrder() { PaymentNo = v_oid, Money = decimal.Parse(v_amount), Description = "MD5验证失败" });
                return false;
            }

            if (!v_pstatus.Equals("20"))
            {
                this.OnNotifyVerifyFaild(new PaymentOrder() { PaymentNo = v_oid, Money = decimal.Parse(v_amount), Description = "错误码[" + v_pstatus + "]" });
                return false;
            }

            this.OnFinished(new PaymentOrder() { PaymentNo = v_oid, Money = decimal.Parse(v_amount) });
            return true;
        }

        /// <summary>
        /// 验证交易是否被篡改，并返回第三方字符的文本,主要用于自动Notice
        /// </summary>
        /// <param name="form"></param>
        /// <param name="returnContent">返回的内容</param>
        /// <returns></returns>
        public override bool VerifyNotify(NameValueCollection form, out string returnContent) {
            var isOk = VerifyNotify(form);
            returnContent = isOk ? "ok" : "error";
            return isOk;
        }
    }
}
