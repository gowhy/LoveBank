using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using QDT.Common;
using QDT.Common.Plugins;

namespace QDT.Plugins.Payment
{
    public class GuofubaoQuickPayment : AbstractPayment 
    {
        private const string NAME = "国付宝直连支付";

        public GuofubaoQuickPayment()
        {
            OnlinePaly = true;
            InitConfig();
        }

        private void InitConfig()
        {
            Config = new Dictionary<string, PaymentConfig>{
                {"merchant_id",new PaymentConfig("商户ID",InputType.Text)},
                {"virCardNoIn",new PaymentConfig("国付宝帐号",InputType.Text)},
                {"VerficationCode",new PaymentConfig("商户识别码",InputType.Text)},
                {"guofubao_gateway",new PaymentConfig("支持的银行",InputType.Select){
                    Values=new CheckBoxGroup()
                    .Add("ICBC","中国工商银行")
                    .Add("CMB","招商银行")
                    .Add("CCB","中国建设银行")
                    .Add("ABC","中国农业银行")
                    .Add("BOC","中国银行")
                    .Add("BOCOM","交通银行")
                    .Add("HXBC","华夏银行")
                    .Add("CIB","兴业银行")
                    .Add("CMBC","中国民生银行")
                    .Add("GDB","广东发展银行")
                    .Add("SPDB","上海浦东发展银行")
                    .Add("CITIC","中信银行")
                    .Add("CEB","光大银行")
                    .Add("SDB","深圳发展银行")
                    .Add("PSBC","邮政储蓄银行")
                }}
            };
            
        }

        #region Overrides of AbstractPayment

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public override string Key { get { return GetType().Name; } }

        /// <summary>
        /// 名字
        /// </summary>
        public override string Name { get { return NAME; } }

        /// <summary>
        /// 取得插件在前端显示的HTML
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayHtml() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送支付请求到支付网关
        /// </summary>
        /// <param name="PaymentOrder">支付订单信息</param>
        /// <returns></returns>
        public override void SendRequest(PaymentOrder order) {
            throw new NotImplementedException();
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
        public override void VerifyNotify(NameValueCollection form) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
