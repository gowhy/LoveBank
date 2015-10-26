using System;
using System.Web.Mvc;

namespace LoveBank.Web.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpsRequireAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            var httpsRequire = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Https"]); ;
            if (httpsRequire)
                base.HandleNonHttpsRequest(filterContext);
        }
    }
}