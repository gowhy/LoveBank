using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;

namespace LoveBank.MVC.Security
{
    public class RoleAuthorizeAttribute : DefaultAuthorizeAttribute
    {
        protected override void HandleRoleAuthorized(AuthorizationContext filterContext)
        {
            if(!IsPermission(filterContext))
            {
                HandleUnauthorized(filterContext);
            }
        }
        protected virtual bool IsPermission(AuthorizationContext filterContext)
        {
            if (!User.IsAuthenticated) return false;

       

            var permissions = SecurityManager.Instance.SecurityService.GetPermissions(User.Name);

            var node = SecurityManager.CreateNode(filterContext.ActionDescriptor);
            if (node == null)//如果节点没有安全节点属性，表示不做Role安全控制
            {
                return true;
            }

            return permissions.IsPermission(node);
        }
    }
}
