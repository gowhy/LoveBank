using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.MVC.Security;

using LoveBank.P2B.Domain.Config;
using LoveBank.Services;
using LoveBank.Services.ConfigModule;

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "系统管理")]
    public class ConfigController : BaseController
    {
        protected IConfigService ConfigService; 

        public ConfigController(IConfigService configService)
        {
            Check.Argument.IsNotNull(configService,"configService");

            ConfigService = configService;
        }

        [SecurityNode(Name = "邮件配置")]
        public ActionResult Index()
        {
            var msgConfig = SettingManager.Get<MessageConfig>();
            return View(msgConfig);
        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult PostEdit(FormCollection form)
//        {
//            var configs = DbProvider.D<Config>().Where(x=>x.IsEffect&&x.IsConfig).ToList();
//            foreach (var config in configs)
//            {
//                config.Value = form[config.Name];
//                ConfigService.SaveConfig(config);
//            }
//            return Success("操作成功");
//        }

        [HttpPost]
        [SecurityNode(Name = "编辑邮件配置")]
        public ActionResult PostMessage(bool MessageOpen, bool MailOpen, bool SmsOpen, bool AutoBid)
        {
            var config=SettingManager.Get<MessageConfig>();

            config.Enable = MessageOpen;

            config.EmailOpen = MailOpen;
            config.SmsOpen = SmsOpen;
            config.AutoBid = AutoBid;

            SettingManager.Save(config);

            return Success("操作成功");
        }
    }
}
