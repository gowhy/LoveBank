using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.MVC.Security
{
    public interface ISecurityService
    {
        /// <summary>
        /// 返回用户的的Permissions集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        PermissionCollection GetPermissions(string userName);

    }
}
