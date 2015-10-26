using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LoveBank.Common;
using LoveBank.MVC.Security;
using LoveBank.Services.AdminModule;

namespace LoveBank.Web.Admin.Code
{
    public class AdminAuthorizeAttribute : RoleAuthorizeAttribute
    {
        protected override void HandleRoleAuthorized(AuthorizationContext filterContext)
        {
            if(!IsPermission(filterContext)){
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action = "NoPermission", Controller = "Account" }));
            }
        }

        protected override bool IsPermission(AuthorizationContext filterContext)
        {
            return IoC.Resolve<IAdminService>().GetAdmin(User.Name).IsDefaultAdmin || base.IsPermission(filterContext);
        }

    }
}