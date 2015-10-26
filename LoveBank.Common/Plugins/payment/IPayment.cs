using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LoveBank.Common.Plugins
{
    public interface IPayment
    {
        /// <summary>
        /// 交易完成事件
        /// </summary>
        event EventHandler Finished;

        /// <summary>
        /// 验证失败事件
        /// </summary>
        event EventHandler NotifyVerifyFaild;

        /// <summary>
        /// 支付完成事件（支付完成往往不等于交易完成）
        /// </summary>
        event EventHandler Payment;

        /// <summary>
        /// 识别Key,一般为类名
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 名字
        /// </summary>
        string Name { get;}

        /// <summary>
        /// 是否有效
        /// </summary>
        bool IsEffect { get; set; }

        /// <summary>
        /// 是否是在线支付
        /// </summary>
        bool OnlinePaly { get;}

        /// <summary>
        /// Logo
        /// </summary>
        string Logo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 手续费计算方式（0：定额  1：比率）
        /// </summary>
        int FeeType { get; set; }

        /// <summary>
        /// 手续费值
        /// </summary>
        decimal FeeAmount { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        IDictionary<string, PaymentConfig> Config { get; set; }

        /// <summary>
        /// 取得插件在前端显示的HTML
        /// </summary>
        /// <returns></returns>
        string GetDisplayHtml();

        /// <summary>
        /// 发送支付请求到支付网关
        /// </summary>
        /// <param name="PaymentOrder">支付订单信息</param>
        /// <returns></returns>
        void SendRequest(PaymentOrder order);

        /// <summary>
        /// 获得支付请求的Form表单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        string GetPaymentForm(PaymentOrder order);

        /// <summary>
        /// 验证交易是否被篡改
        /// </summary>
        /// <param name="form">第三方支付传递的参数</param>
        /// <returns></returns>
        bool VerifyNotify(NameValueCollection form);

        /// <summary>
        /// 验证交易是否被篡改，并返回第三方字符的文本,主要用于自动Notice
        /// </summary>
        /// <param name="form"></param>
        /// <param name="returnContent">返回的内容</param>
        /// <returns></returns>
        bool VerifyNotify(NameValueCollection form, out string returnContent);
    }
}
