using LoveBank.Common;

namespace LoveBank.Core.Domain
{
    /// <summary>
    /// 系统配置文件对象
    /// </summary>
    public class Config : Entity, IAggregeRoot
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public int Group { get; set; }

        public int InputType { get; set; }

        public string ValueScope { get; set; }

        public bool IsEffect { get; set; }

        public bool IsConfig { get; set; }

        public int Sort { get; set; }

        public string Tip { get; set; }

        public string[] ValueScopes
        {
            get { return ValueScope.SplitNull(','); }
        }
    }

    public static class ConfigGroup
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public const int Base = 0;

        /// <summary>
        /// 图片配置
        /// </summary>
        public const int Image = 1;

        /// <summary>
        /// 站点配置
        /// </summary>
        public const int Site = 2;
        /// <summary>
        /// 会员相关配置
        /// </summary>
        public const int Member = 3;


        /// <summary>
        /// 邮件与短信
        /// </summary>
        public const int SMS = 3;
        /// <summary>
        /// 贷款配置
        /// </summary>
        public const int Deal = 3;
    }
}
