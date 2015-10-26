

namespace LoveBank.Core.Domain {
    /// <summary>
    /// 短信接口
    /// </summary>
    public class SmServer:Entity {
        /// <summary>
        /// 通道名
        /// </summary>
        public string ServerName { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 接口名
        /// </summary>
        public string ClassName { set; get; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerUrl { set; get; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { set; get; }
        /// <summary>
        /// 配置信息
        /// </summary>
        public string ConfigInfo { set; get; }
    }
}