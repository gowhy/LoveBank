namespace QDT.Common.Plugins.Sms
{
    public interface ISMSAttribute
    {
        /// <summary>
        /// 短信名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// 开发作者
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// 短信接口帐号
        /// </summary>
        string SmsAccount { get; set; }

        /// <summary>
        /// 短信接口密码
        /// </summary>
        string SmsPassword { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 动态属性部分
        /// </summary>
        DynamicConfig[] Config { get; set; } 
    }
}
