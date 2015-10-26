using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common;
using System.Web.Security;

namespace LoveBank.Services.Members
{
    public class DefaultFormsAuthentication:IFormsAuthenticationService
    {
        public void SignIn(string userName, bool rememberme, string cookiePath)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            if (string.IsNullOrWhiteSpace(cookiePath)) cookiePath = "/";

            FormsAuthentication.SetAuthCookie(userName, rememberme, cookiePath);
        }
        public void SignIn(string userName, bool rememberMe = false)
        {
            SignIn(userName, rememberMe, string.Empty);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
