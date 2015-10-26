using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Services.Members
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool rememberme, string cookiePath);
        void SignIn(string userName, bool rememberMe);
        void SignOut();
    }
}
