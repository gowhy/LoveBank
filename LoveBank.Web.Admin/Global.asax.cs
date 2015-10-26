using System.Web.Mvc;
using System.Web.Routing;
using LoveBank.Common;
using LoveBank.Common.Config;
using LoveBank.Common.Data;
using LoveBank.Common.Events;
using LoveBank.Core.MSData;
using LoveBank.MVC;
using LoveBank.MVC.SiteMap;

using LoveBank.Services.AdminModule;

using LoveBank.Services.ConfigModule;

using LoveBank.Services.LogMoudle;
using LoveBank.Services.Members;

using LoveBank.Services.Payments;

using LoveBank.Services.SmMailModule;
using log4net;
using DefaultFormsAuthentication = LoveBank.Web.Admin.Models.DefaultFormsAuthentication;
using IFormsAuthenticationService = LoveBank.Web.Admin.Models.IFormsAuthenticationService;

namespace LoveBank.Web.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : BaseMvcApplication
    {
        public override void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        public override void RegisterSrevice(IContainerAdapter container)
        {
            container.RegisterAsPerRequest<IUnitOfWork>(x => new DbProvider("conn_love_bank_db"));

            //注册Provider
            container.RegisterAsPerRequest<IConfigProvider>(x => new SqlSettingsProvider("conn_love_bank_db"));

            container.RegisterAsSingleton<IFormsAuthenticationService, DefaultFormsAuthentication>()
                //.RegisterAsSingleton<IMsgService, MsgService>()
                //.RegisterAsSingleton<IConfigService, ConfigService>()
                //.RegisterAsSingleton<IPaymentService, PaymentService>()
                .RegisterAsSingleton<IAdminService, AdminService>()
                //.RegisterAsSingleton<IProjectService, ProjectService>()
                //.RegisterAsSingleton<IDealService, DealService>()
                //.RegisterAsSingleton<IProjectTypeService, ProjectTypeService>()
                .RegisterAsSingleton<IAdminLogService, AdminLogService>();
                //.RegisterAsSingleton<IOrderService, OrderService>()
                //.RegisterAsSingleton<IAssignmentService, AssignmentService>()
                //.RegisterAsSingleton<IQureyService, QureyService>();
                


            //注册领域事件服务
            EventConfig.Configure(config =>
            {
                config.UsingUnitOfWorkFactory(container.Resolve<IUnitOfWork>);
                config.UsingDefaultEventDispatcher(typeof(LoveBank.Core.Entity).Assembly);
            });

            //配置基于MVC的相关配置
            LoveBankMvcConfig.Configure(config =>
                                       {
                                            //config.SiteMap(maps => maps.Register<XmlSiteMap>("default", x => x.Load()));
                                            config.Sercurity(typeof (MvcApplication).Assembly,x=>x.RegisterService<AdminService>());
                                            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
                                            //Log.SetLog(LogManager.GetLogger("Loger"));
                                       }
                );

          
        }
    }
}