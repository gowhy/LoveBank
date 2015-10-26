using System.Web.Mvc;
using System.Web.Routing;
using LoveBank.Common;
using LoveBank.Common.Config;
using LoveBank.Common.Data;
using LoveBank.Common.Events;
using LoveBank.Core.MSData;
using LoveBank.MVC;
using LoveBank.Services.Members;
using LoveBank.Services.SmMailModule;
using log4net;

namespace LoveBank.Web
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

            routes.MapRoute("Guarantee", "Guarantee", new {controller = "Article", action = "Guarantee"});

            routes.MapRoute("HelpCenter", "helpcenter", new { controller = "Article", action = "HelpCenter" });

            routes.MapRoute("Notice", "notice/id-{id}",
                            new { controller = " Article", action = "ShowDetail", id = UrlParameter.Optional });

            routes.MapRoute("Help", "help/id-{id}",
                            new {controller = "Help", action = "Index", id = UrlParameter.Optional});

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        public override void RegisterSrevice(IContainerAdapter container)
        {
            container.RegisterAsPerRequest<IUnitOfWork>(x => new DbProvider("Conn_Crawl_db"));


            //注册Provider
            container.RegisterAsPerRequest<IConfigProvider>(x => new SqlSettingsProvider("Conn_Crawl_db"));

            container
                .RegisterAsSingleton<IFormsAuthenticationService,DefaultFormsAuthentication>()
                .RegisterAsSingleton<IMsgService, MsgService>();

            EventConfig.Configure(config =>
            {
                config.UsingUnitOfWorkFactory(container.Resolve<IUnitOfWork>);
                config.UsingDefaultEventDispatcher(typeof(Core.Entity).Assembly);
            });

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            Log.SetLog(LogManager.GetLogger("Loger"));
        }



    }
}