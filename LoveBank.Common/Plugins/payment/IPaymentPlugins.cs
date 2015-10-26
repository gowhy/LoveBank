/*==============================================================
 * IPaymentPlugins.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/21 17:14:59.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

namespace QDT.Common.Plugins {
    public interface IPaymentPlugins:IPlugins {
        /// <summary>
        /// 获取支付码或提示信息
        /// </summary>
        /// <param name="paymentNoticeId">支付单号ID</param>
        /// <returns></returns>
        string GetPaymentCode(int paymentNoticeId);

        string GetDisplayCode();
        /// <summary>
        /// 取得插件信息
        /// </summary>
        /// <returns></returns>
        PaymentInfo GetInfo();

    }
}