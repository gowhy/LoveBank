using System.Collections.Generic;
using Newtonsoft.Json;
using LoveBank.Common;
using LoveBank.Common.Plugins;

namespace LoveBank.Core.Payments {
    public class PaymentInfo : Entity {
        /// <summary>
        /// 第三方支付类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 是否是在线支付
        /// </summary>
        public bool OnlinePay { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 统计字段，该支付方式总的支付金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 配置信息(XML字符串)
        /// </summary>
        public string SerializarConfig { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 手续费类型（0：定额   1：比率）
        /// </summary>
        public int FeeType { get; set; }


        /// <summary>
        /// 手续费，根据FeeType来决定
        /// </summary>
        public decimal FeeAmount { get; set; }

        ///// <summary>
        ///// 配置信息
        ///// </summary>
        //public Dictionary<string, PaymentInfo.ConfigInfo> GetConfig() {

        //    return !string.IsNullOrEmpty(Config)
        //        ? JsonConvert.DeserializeObject<Dictionary<string, PaymentInfo.ConfigInfo>>(Config) : null;

        //}

        public IDictionary<string, PaymentConfig> Config {

            get {

                var data=JsonConvert.DeserializeObject<IDictionary<string, PaymentConfig>>(SerializarConfig);

                foreach (var config in data) {
                    
                    if(config.Value.InputType==InputType.Select) {
                        config.Value.Values = JsonConvert.DeserializeObject<CheckBoxGroup>(config.Value.Values.ToString());
                    }
                }

                return data;
            }

            set {

                SerializarConfig=JsonConvert.SerializeObject(value);
            }
        }
    }
}