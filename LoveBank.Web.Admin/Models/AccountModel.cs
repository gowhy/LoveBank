using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using LoveBank.Common;

namespace LoveBank.Web.Admin.Models
{
    public class LogOnModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool rememberme, string cookiePath);
        void SignIn(string userName, bool rememberMe);
        void SignOut();
    }


    public class DefaultFormsAuthentication : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool rememberme, string cookiePath)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            if (string.IsNullOrWhiteSpace(cookiePath)) cookiePath = "/";

            FormsAuthentication.SetAuthCookie(userName, rememberme, cookiePath);
        }
        public void SignIn(string userName, bool rememberMe=false)
        {
            SignIn(userName, rememberMe, string.Empty);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}