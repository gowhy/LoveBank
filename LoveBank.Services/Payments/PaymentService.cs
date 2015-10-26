using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;
using LoveBank.Core.Payments;
using LoveBank.P2B.Domain.Config;
using LoveBank.P2B.Domain.Messages;

namespace LoveBank.Services.Payments {
    public class PaymentService : ServiceBase, IPaymentService {
    

        /// <summary>
        /// 获得系统所有的支付方式接口
        /// </summary>
        /// <returns></returns>
        public ICollection<IPayment> GetPayments() {
            var paymentManager = new PaymentManager();

            IList<IPayment> payments = paymentManager.LoadPlugins();

            return payments;
        }

        /// <summary>
        /// 取得所有有效的支付方式
        /// </summary>
        /// <returns></returns>
        public ICollection<IPayment> GetEffectPayments() {
            IQueryable<PaymentInfo> paymentInfos = DbProvider.D<PaymentInfo>().Where(x => x.IsEffect).OrderByDescending(x=>x.Sort);

            var paymentManager = new PaymentManager();

            var result = new List<IPayment>();

            foreach (PaymentInfo paymentInfo in paymentInfos) {
                IPayment payment = paymentManager.GetPlugins(paymentInfo.ClassName);

                if (payment == null) continue;

                payment.Logo = paymentInfo.Logo;
                payment.Sort = paymentInfo.Sort;
                payment.FeeType = paymentInfo.FeeType;
                payment.FeeAmount = paymentInfo.FeeAmount;
                payment.Config = paymentInfo.Config;

                result.Add(payment);
            }

            return result;
        }

        /// <summary>
        /// 安装支付接口
        /// </summary>
        /// <param name="payment"></param>
        public void Install(IPayment payment) {

            Check.Argument.IsNotNull(payment, "payment");

            if(DbProvider.D<PaymentInfo>().Count(x=>x.ClassName==payment.Key)>0) throw new ApplicationException("{0} 已经安装.".FormatWith(payment.Name));

            var paymentInfo = new PaymentInfo();

            PaymentToInfo(payment, paymentInfo);

            DbProvider.Add(paymentInfo);
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 更新支付接口配置
        /// </summary>
        /// <param name="payment"></param>
        public void Update(IPayment payment) {

            Check.Argument.IsNotNull(payment, "payment");

            var paymentInfo = DbProvider.D<PaymentInfo>().FirstOrDefault(x=>x.ClassName==payment.Key);

            if (paymentInfo == null) return;

            PaymentToInfo(payment, paymentInfo);

            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 卸载支付接口
        /// </summary>
        /// <param name="paymentInfoID">接口的安装信息ID</param>
        public void Uninstall(int paymentInfoID) {
            var paymentInfo = DbProvider.GetByID<PaymentInfo>(paymentInfoID);

            if (paymentInfo == null) return;

            DbProvider.Delete(paymentInfo);

            DbProvider.SaveChanges();
        }


        /// <summary>
        /// 根据支付接口的安装ID获得支付插件
        /// </summary>
        /// <param name="paymentInfoId"></param>
        /// <returns></returns>
        public IPayment GetPayment(int paymentInfoId) {
            var paymentInfo = DbProvider.GetByID<PaymentInfo>(paymentInfoId);

            if (paymentInfo == null) return null;

            var paymentManager = new PaymentManager();

            IPayment payment = paymentManager.GetPlugins(paymentInfo.ClassName);

            if (payment == null) return null;

            //InfoToPayment(paymentInfo, payment);

            return payment;
        }

        private static  object _asnyLock = new object();

        /// <summary>
        /// 根据支付接口的Key,获得支付接口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPayment GetPayment(string key) {

            lock (_asnyLock)
            {
                var paymentInfo = DbProvider.D<PaymentInfo>().FirstOrDefault(x => x.ClassName == key);

                if (paymentInfo == null) return null;

                var paymentManager = new PaymentManager();

                IPayment payment = paymentManager.GetPlugins(paymentInfo.ClassName);

                if (payment == null) return null;

                //InfoToPayment(paymentInfo, payment);

                return payment;
            }
            
        }

        private void PaymentToInfo(IPayment payment,PaymentInfo info) {
            info.ClassName = payment.Key;
            info.Config = payment.Config;
            info.Description = payment.Description;
            info.FeeAmount = payment.FeeAmount;
            info.FeeType = payment.FeeType;
            info.IsEffect = payment.IsEffect;
            info.Logo = payment.Logo;
            info.Name = payment.Name;
            info.OnlinePay = payment.OnlinePaly;
            info.Sort = payment.Sort;
        }

     

       

  
    }
}