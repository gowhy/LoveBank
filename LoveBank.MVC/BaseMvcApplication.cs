using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using LoveBank.Common;
using LoveBank.Common.Unity;
using LoveBank.MVC.ModelBinder;

namespace LoveBank.MVC
{
    public abstract class BaseMvcApplication : HttpApplication
    {
        public abstract void RegisterGlobalFilters(GlobalFilterCollection filters);

        public abstract void RegisterRoutes(RouteCollection routes);

        public abstract void RegisterSrevice(IContainerAdapter container);

        protected void Application_Start()
        {
            ModelBinders.Binders.DefaultBinder=new CustomModelBinder();

            IUnityContainer container = new UnityContainer();

            container.RegisterType<HttpContextBase, HttpContextWrapper>(new InjectionFactory(c => new HttpContextWrapper(HttpContext.Current)));

            IoC.SetAdapter(new UnityAdapter(container));

            RegisterSrevice(IoC.Current);

            DependencyResolver.SetResolver(new ExtendDependencyResolver(IoC.Current));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
