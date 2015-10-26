using System;
using System.Collections.Generic;
using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Core;
using LoveBank.Core.Members;

namespace LoveBank.P2B.Domain.Messages {
    public class MsgQueueFactory {

        public MsgQueue CreateBidMsg() {
            throw new NotImplementedException();
        }

        public MsgQueue CreateBindBankMsg(string phone, string bankCard)
        {
            return new MsgQueue(phone, 0)
            {
                Type = MsgType.SMS,
                Content = "尊敬的用户，您成功绑定了一张银行卡，尾号：{0}".FormatWith(bankCard),
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        public MsgQueue CreateMailTest(string email) {
            return new MsgQueue(email,0)
            {
                Type = MsgType.Email,
                Title = "测试邮件",
                Content = "这是测试邮件，不用回复",
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        public MsgQueue CreateValidatorMsg(string email, string title, string content)
        {
            return new MsgQueue(email, 0)
            {
                Type = MsgType.Email,
                Title = title,
                Content = content,
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        public MsgQueue CreateSmsTest(string phone) {
            return new MsgQueue(phone, 0)
            {
                Type = MsgType.SMS,
                Content = "尊敬的用户，你成功还款20元，感谢您的关注和支持。【】",
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        //public MsgQueue CreateRepayMsg(User user, decimal money,int projectID) {

        //    var moneyKey = new KeyValuePair<string, string>("money", money.ToString("0.00"));
        //    var projectIDKey = new KeyValuePair<string, string>("projectID", projectID.ToString());

        //    if(string.IsNullOrWhiteSpace(user.Mobile)) return null;;

        //    return new MsgQueue(user.Mobile, 0)
        //    {
        //        Type = MsgType.SMS,
        //        Content = new MsgTemplateService().BackMoney().FormatContent(moneyKey,projectIDKey),
        //        IsSend = false,
        //        IsSuccess = false,
        //        Result = string.Empty
        //    };
        //}

        public MsgQueue CreateInchargeMsg(string phone, decimal money,string orderNo)
        {
            var moneyKey = new KeyValuePair<string, string>("money", money.ToString("0.00"));
            var orderNoKey = new KeyValuePair<string, string>("orderNo", orderNo);

            return new MsgQueue(phone, 0)
            {
                Type = MsgType.SMS,
                Content = new MsgTemplateService().ChargeSuccess().FormatContent(moneyKey, orderNoKey),
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        public MsgQueue CreateValidatorMsg(string phone, string validator) {
            var code = new KeyValuePair<string, string>("code", validator);
            return new MsgQueue(phone, 0)
            {
                Level = MsgQueue.Level_Hight,
                Type = MsgType.SMS,
                Content = new MsgTemplateService().CaptchaCode().FormatContent(code),
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        public MsgQueue CreateMoneyCarryMsg(string phone,string bank,string card,decimal money) {
            var bankKey = new KeyValuePair<string, string>("bank", bank);
            var cardKey = new KeyValuePair<string, string>("card", card.Substring(card.Length - 4));
            var moneyKey = new KeyValuePair<string, string>("money", money.ToString("0.00"));
            return new MsgQueue(phone, 0)
            {
                Type = MsgType.SMS,
                Content = new MsgTemplateService().MoneyCarry().FormatContent(bankKey,cardKey,moneyKey),
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
        }

        #region Private Method

        private IDbProvider DbProvider { get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; } }

        #endregion
    }
}