using System.Linq;

namespace LoveBank.P2B.Domain.Messages {
    public class MsgTemplateService : BaseService {

        private const string CaptchCodeKey = "CaptchCodeKey";
        private const string ChargeSuccessKey = "ChargeSuccessKey";
        private const string BidKey = "BidKey";
        private const string BackMoneyKey = "BackMoney";
        private const string MoneyCarryKey = "MoneyCarryKey";

        public MsgTemplate CaptchaCode() {
            var temp = DbProvider.D<MsgTemplate>().FirstOrDefault(x => x.Key == CaptchCodeKey);

            if (temp != null) return temp;

            temp = new MsgTemplate {
                                       MsgType = MsgType.SMS,
                                       Key = CaptchCodeKey,
                                       IsHtml = false,
                                       MsgTip = "", 
                                       ParamString = "code", 
                                       Content = "验证码:{code},如非本人操作,请忽略本短信", 
                                       Title = ""
                                   };

            return temp;
        }

        /// <summary>
        /// 充值成功短信模版，参数{money}充值金额,{orderNo}订单号
        /// </summary>
        /// <returns></returns>
        public MsgTemplate ChargeSuccess() {

            var temp = DbProvider.D<MsgTemplate>().FirstOrDefault(x => x.Key == ChargeSuccessKey);

            if (temp != null) return temp;

            temp = new MsgTemplate
            {
                MsgType = MsgType.SMS,
                Key = ChargeSuccessKey,
                IsHtml = false,
                MsgTip = "",
                ParamString = "money,orderNo",
                Content = "亲爱的用户,您成功充值{money}元,订单号为:{orderNo}",
                Title = ""
            };

            return temp;
        }

        public MsgTemplate Bid() {

            var temp = DbProvider.D<MsgTemplate>().FirstOrDefault(x => x.Key == BidKey);

            if (temp != null) return temp;

            temp = new MsgTemplate
            {
                MsgType = MsgType.SMS,
                Key = BidKey,
                IsHtml = false,
                MsgTip = "",
                ParamString = "money,projectID",
                Content = "亲爱的用户,您成功投标{money}元,项目编号为:{projectID}",
                Title = ""
            };

            return temp;
        }

        public MsgTemplate MoneyCarry() {
            var temp = DbProvider.D<MsgTemplate>().FirstOrDefault(x => x.Key == BackMoneyKey);

            if (temp != null) return temp;

            temp = new MsgTemplate
            {
                MsgType = MsgType.SMS,
                Key = MoneyCarryKey,
                IsHtml = false,
                MsgTip = "",
                ParamString = "bank,card,money",
                Content = "亲爱的用户,您向{bank}银行(尾号{card})成功体现{money}元,请注意查收",
                Title = ""
            };

            return temp;
        }

        /// <summary>
        /// 回款模版，参数{money}回款金额,{projectID}回款项目ID
        /// </summary>
        /// <returns></returns>
        public MsgTemplate BackMoney() {
            var temp = DbProvider.D<MsgTemplate>().FirstOrDefault(x => x.Key == BackMoneyKey);

            if (temp != null) return temp;

            temp = new MsgTemplate
            {
                MsgType = MsgType.SMS,
                Key = BackMoneyKey,
                IsHtml = false,
                MsgTip = "",
                ParamString = "code",
                Content = "亲爱的用户,本期回款{money}元，项目编号为:{projectID}",
                Title = ""
            };

            return temp;
        }

    }
}