

using System.Collections.Generic;
using LoveBank.Core.Payments;
using LoveBank.Common.Plugins;

namespace LoveBank.Services.Payments {

    public interface IPaymentService {

        /// <summary>
        /// 获得系统所有的支付方式接口
        /// </summary>
        /// <returns></returns>
        ICollection<IPayment> GetPayments();

        /// <summary>
        /// 取得所有有效的支付方式
        /// </summary>
        /// <returns></returns>
        ICollection<IPayment> GetEffectPayments();

        /// <summary>
        /// 安装支付接口
        /// </summary>
        /// <param name="payment"></param>
        void Install(IPayment payment);

        /// <summary>
        /// 更新支付接口配置
        /// </summary>
        /// <param name="payment"></param>
        void Update(IPayment payment);

        /// <summary>
        /// 卸载支付接口
        /// </summary>
        /// <param name="paymentInfoID"></param>
        void Uninstall(int paymentInfoID);

        /// <summary>
        /// 根据支付接口的安装ID
        /// </summary>
        /// <param name="paymentInfoId"></param>
        /// <returns></returns>
        IPayment GetPayment(int paymentInfoId);

        /// <summary>
        /// 根据支付接口的Key,获得支付接口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPayment GetPayment(string key);

        
    }
}