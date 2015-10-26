namespace LoveBank.Common.Plugins.Sms
{
    public class SMSAttribute
    {
        /// <summary>
        /// 短信名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 开发作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 短信接口帐号
        /// </summary>
        public string SmsAccount { get; set; }

        /// <summary>
        /// 短信接口密码
        /// </summary>
        public string SmsPassword { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 动态属性部分
        /// </summary>
        public DynamicConfig[] Config { get; set; } 
    }
}
