using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace LoveBank.Common.Plugins
{
    public abstract class AbstractPayment:IPlugins, IPayment
    {
        protected AbstractPayment()
        {
            Config = new Dictionary<string,PaymentConfig>();
        }
        /// <summary>
        /// 交易完成事件
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// 验证失败事件
        /// </summary>
        public event EventHandler NotifyVerifyFaild;

        /// <summary>
        /// 支付完成事件（支付完成往往不等于交易完成）
        /// </summary>
        public event EventHandler Payment;

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// 名字
        /// </summary>
        public abstract string Name {get; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否是在线支付
        /// </summary>
        public bool OnlinePaly { get;protected set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 手续费计算方式（0：定额  1：比率）
        /// </summary>
        public int FeeType { get; set; }

        /// <summary>
        /// 手续费值
        /// </summary>
        public decimal FeeAmount { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public IDictionary<string,PaymentConfig> Config { set; get; }

        /// <summary>
        /// 取得插件在前端显示的HTML
        /// </summary>
        /// <returns></returns>
        public abstract string GetDisplayHtml();

        /// <summary>
        /// 发送支付请求到支付网关
        /// </summary>
        /// <param name="PaymentOrder">支付订单信息</param>
        /// <returns></returns>
        public abstract void SendRequest(PaymentOrder order);

        /// <summary>
        /// 获得支付请求的Form表单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public abstract string GetPaymentForm(PaymentOrder order);

        /// <summary>
        /// 验证交易是否被篡改
        /// </summary>
        /// <param name="form">第三方支付传递的参数</param>
        /// <returns></returns>
        public abstract bool VerifyNotify(NameValueCollection form);

        /// <summary>
        /// 验证交易是否被篡改，并返回第三方字符的文本,主要用于自动Notice
        /// </summary>
        /// <param name="form"></param>
        /// <param name="returnContent">返回的内容</param>
        /// <returns></returns>
        public abstract bool VerifyNotify(NameValueCollection form,out string returnContent);

        protected virtual void OnFinished(PaymentOrder order)
        {
            if (this.Finished != null)
            {
                this.Finished(order,null);
            }
        }

        protected virtual void OnNotifyVerifyFaild(PaymentOrder order)
        {
            if (this.NotifyVerifyFaild != null)
            {
                this.NotifyVerifyFaild(order, null);
            }
        }

        protected virtual void OnPayment(PaymentOrder order)
        {
            if (this.Payment != null)
            {
                this.Payment(order, null);
            }
        }

        private const string FormFormat = "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>";
        private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

        protected virtual string CreateField(string name, string strValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[] { name, strValue });
        }

        protected virtual string CreateForm(string content, string action)
        {
            content = content + "<input type=\"submit\" value=\"在线支付\" style=\"display:none;\">";
            return string.Format(CultureInfo.InvariantCulture, "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[] { action, content });
        }

        protected virtual void SubmitPaymentForm(string formContent)
        {
            string s = formContent + "<script>document.forms['payform'].submit();</script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }
        
    }
}
