using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using LoveBank.Common.Plugins;
using LoveBank.Common;

namespace LoveBank.Plugins.Payment
{
    public class ChinaBankDirectPayment:AbstractPayment {

        private const string Gateway = "https://pay3.chinabank.com.cn/PayGate";
        private const string v_moneytype = "CNY";

        private const string NAME = "网银在线直连支付";

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public override string Key { get { return this.GetType().Name; }}

        /// <summary>
        /// 名字
        /// </summary>
        public override string Name { get { return NAME; } }

        public ChinaBankDirectPayment()
        {
            OnlinePaly = true;
            InitConfig();
        }

        private void InitConfig()
        {
            Config = new Dictionary<string, PaymentConfig>{
                {"chinabank_account",new PaymentConfig("商户编号",InputType.Text)},
                {"chinabank_key",new PaymentConfig("商户密钥",InputType.Text)},
                {"chinabank_gateway",new PaymentConfig("支持的银行",InputType.Select){
                    Values=new CheckBoxGroup()
                    .Add("1025","中国工商银行")
                    .Add("3080","招商银行")
                    .Add("105","中国建设银行")
                    .Add("103","中国农业银行")
                    .Add("104","中国银行")
                    .Add("301","交通银行")
                    .Add("311","华夏银行")
                    .Add("309","兴业银行")
                    .Add("305","中国民生银行")
                    .Add("306","广东发展银行")
                    .Add("307","平安银行")
                    .Add("314","上海浦东发展银行")
                    .Add("313","中信银行")
                    .Add("312","光大银行")
                    .Add("316","南京银行")
                    .Add("3230","邮政储蓄银行")
                    .Add("302","宁波银行")
                    .Add("324","杭州银行")
                    .Add("327","中国银联")
                }}
            };
            
        }


        public override string GetDisplayHtml()
        {
            var html = new StringBuilder();
            html.Append("<style type='text/css'>.chinabank_types{float:left; display:block; background:url(/Plugins/Content/banks.png) no-repeat top left; font-size:0px; width:155px; height:22px; text-align:left; padding:15px 0px;}");
            html.Append(".bk_type_306{ background-position: 25px 0;}"); //广东发展银行
            html.Append(".bk_type_312{ background-position: 25px -53px;}"); //光大银行
            html.Append(".bk_type_1025{background-position: 25px -106px;}"); //中国工商银行
            html.Append(".bk_type_324{background-position: 25px -159px;}"); //杭州银行
            html.Append(".bk_type_311{background-position: 25px -212px;}"); //华夏银行
            html.Append(".bk_type_105{background-position: 25px -265px;}"); //中国建设银行
            html.Append(".bk_type_301{background-position: 25px -318px;}"); //交通银行
            html.Append(".bk_type_305{background-position: 25px -371px;}"); //中国民生银行
            html.Append(".bk_type_302{background-position: 25px -424px;}"); //宁波银行
            html.Append(".bk_type_316{background-position: 25px -477px;}"); //南京银行
            html.Append(".bk_type_103{background-position: 25px -530px;}"); //中国农业银行
            html.Append(".bk_type_307{background-position: 25px -583px;}"); //平安银行
            html.Append(".bk_type_314{background-position: 25px -636px;}"); //上海浦东发展银行
            html.Append(".bk_type_3230{background-position: 25px -689px;}"); //邮政储蓄银行
            html.Append(".bk_type_327{background-position: 25px -742px;}"); //中国银联
            html.Append(".bk_type_309{background-position: 25px -795px;}"); //兴业银行
            html.Append(".bk_type_104{background-position: 25px -848px;}"); //中国银行
            html.Append(".bk_type_3080{background-position:25px -901px; }"); //招商银行
            html.Append(".bk_type_313{background-position:25px -954px; }"); //中信银行
            html.Append("</style>");

            html.Append("<script type='text/javascript'>function set_bank(bank_id)");
            html.Append("{");
            html.Append("$(\"input[name='bank_id']\").val(bank_id);");
            html.Append("}</script>");

            var checkBoxGroup = this.Config["chinabank_gateway"].Values as CheckBoxGroup;
            if (checkBoxGroup != null){
                foreach (var c in checkBoxGroup)
                {
                    if(c.Value.Checked){
                        html.Append("<label class='chinabank_types bk_type_" + c.Key + "'><input type='radio' name='payment' value='" + Key + "' rel='" + c.Key + "' onclick='set_bank(\"" + c.Key + "\")' /></label>");
                    }
                }
            }

            html.Append("<div style='clear:both;'></div>");
            html.Append("<input type='hidden' name='bank_id'/>");
            return html.ToString();
        }

        public override void SendRequest(PaymentOrder order) {

            var v_mid = Config["chinabank_account"].Values.ToString();
            var v_oid = order.PaymentNo;
            var v_amount = order.Money.ToString("0.00");
            var v_url = order.ReturnUrl;
            var v_autoReceive = "[url:={0}]".FormatWith(order.NoticeUrl);
            var key = Config["chinabank_key"].Values.ToString();

            //var v_mid = "1001";
            //var v_oid = "19990720-1001-000001234";
            //var v_amount = "0.01";
            //var v_url = "http://domain/chinabank/Receive.asp";
            //var key = Config["chinabank_key"].Values.ToString();
            
            var md5str = v_amount + v_moneytype + v_oid + v_mid + v_url + key;

            string v_md5info = md5str.Hash();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(this.CreateField("v_mid", v_mid));
            stringBuilder.Append(this.CreateField("v_oid", v_oid));
            stringBuilder.Append(this.CreateField("v_amount", v_amount));
            stringBuilder.Append(this.CreateField("v_moneytype", v_moneytype));
            stringBuilder.Append(this.CreateField("v_url",v_url));
            stringBuilder.Append(this.CreateField("remark1", "ChinaBankDirect"));
            stringBuilder.Append(this.CreateField("remark2", v_autoReceive));
            stringBuilder.Append(this.CreateField("v_md5info",v_md5info));
            stringBuilder.Append(this.CreateField("pmode_id", order.BankCode));

            this.SubmitPaymentForm(this.CreateForm(stringBuilder.ToString(), Gateway));
        }

        public override string GetPaymentForm(PaymentOrder order)
        {
            throw new NotImplementedException();
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

        public override bool VerifyNotify(System.Collections.Specialized.NameValueCollection form) {
            var v_oid = form["v_oid"];
            var v_pstatus = form["v_pstatus"];
            var v_pstring = form["v_pstring"];
            var v_pmode = form["v_pmode"];
            var v_md5str = form["v_md5str"];
            var v_amount = form["v_amount"];
            var moneytype = form["v_moneytype"];
            var remark1 = form["remark1"];
            var remark2 = form["remark2"];

            if (v_oid == null || v_pstatus == null || v_pstring == null || v_pmode == null || v_md5str == null || v_amount == null || moneytype == null )
            {
                this.OnNotifyVerifyFaild(new PaymentOrder(){PaymentNo = v_oid,Description = "返回参数为Null"});
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
                this.OnNotifyVerifyFaild(new PaymentOrder() { PaymentNo = v_oid, Money = decimal.Parse(v_amount),Description = "错误码["+v_pstatus+"]"});
                return false;
            }

            this.OnFinished(new PaymentOrder() { PaymentNo = v_oid, Money = decimal.Parse(v_amount),Description = "网银在线" });
            return true;
        }
    }
}
