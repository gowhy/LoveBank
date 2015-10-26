
namespace LoveBank.Common.Plugins {
    /// <summary>
    /// 短信插件
    /// </summary>
    public interface ISmsPlugins : IPlugins {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="content"></param>
        SmsSendResult SendSm(string phoneNo, string content);

        /// <summary>
        /// 取得短信接口相关信息
        /// </summary>
        /// <returns></returns>
        SmsInfo GetSmsInfo();

        /// <summary>
        /// 短信余额查询
        /// </summary>
        /// <returns></returns>
        string CheckFee();

        /// <summary>
        /// 修改短信接口密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        void ModifyPwd(string oldPwd, string newPwd);
    }
}