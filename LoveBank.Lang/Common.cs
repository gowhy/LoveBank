using System.Collections.Generic;

namespace LoveBank.Lang
{
    public class Common:Dictionary<string,string>
    {
        public Common()
        {
            AddConfig();
        }

        protected Common T
        {
            get { return this; }
        }

        protected void AddConfig()
        {
            //组信息
            T["CONF_GROUP_1"] = "基础设置";
            T["CONF_GROUP_2"] = "图片配置";
            T["CONF_GROUP_3"] = "站点设置";
            T["CONF_GROUP_4"] = "会员相关设置";
            T["CONF_GROUP_5"] = "邮件与短信";
            T["CONF_GROUP_6"] = "贷款配置";

            //站点配置
            T["CONF_SITE_TITLE"] = "站点标题";
            T["CONF_SITE_FOOTER"] = "站点底部信息";

            //邮件与短信
            T["CONF_MAIL_ON"] = "开启邮件功能";
            T["CONF_MAIL_ON_0"] = "否";
            T["CONF_MAIL_ON_1"] = "是";
        }

    }

}
