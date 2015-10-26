
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace LoveBank.Common
{
    public class AutheTicketManager
    {
        /// <summary>  
        /// 创建登录用户的票据信息  
        /// </summary>  
        /// <param name="strUserName"></param>  
       public static string CreateLoginUserTicket(string userId)
        {
            DateTime loginTime = DateTime.Now;//用户的登录时间
            //构造Form验证的票据信息  
            ///把登录时间和用户ID写进Cookie中，后面可以用于判断用户的登录时间间隔
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddMinutes(90),
                true, string.Format("{0}:{1}", userId, loginTime), FormsAuthentication.FormsCookiePath);

            string ticString = FormsAuthentication.Encrypt(ticket);

            //把票据信息写入Cookie和Session  
            //SetAuthCookie方法用于标识用户的Identity状态为true  
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserLoginCookieToken", ticString));
            FormsAuthentication.SetAuthCookie(userId, true);
            HttpContext.Current.Session["USER_LOGON_TICKET"] = ticString;

            //重写HttpContext中的用户身份，可以封装自定义角色数据；  
            //判断是否合法用户，可以检查：HttpContext.User.Identity.IsAuthenticated的属性值  
            string[] roles = ticket.UserData.Split(',');
            IIdentity identity = new FormsIdentity(ticket);
            IPrincipal principal = new GenericPrincipal(identity, roles);
            HttpContext.Current.User = principal;

            return ticString;//返回票据
        }


       /// <summary>  
       /// 创建登录用户的票据信息  
       /// 主要是App使用，只创建加密的票据信息，不做其他cookie验证，减少App流量
       /// </summary>  
       /// <param name="strUserName"></param>  
       public static string CreateAppLoginUserTicket(string userId)
       {
           DateTime loginTime = DateTime.Now;//用户的登录时间
           //构造Form验证的票据信息  
           ///把登录时间和用户ID写进Cookie中，后面可以用于判断用户的登录时间间隔
           FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddMinutes(90),
               true, string.Format("{0}:{1}", userId, loginTime), FormsAuthentication.FormsCookiePath);
           
           string ticString = FormsAuthentication.Encrypt(ticket);

           return ticString;//返回票据
       }

       /// <summary>
       /// 删除浏览器票据
       /// </summary>
        public static void Logout()
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserLoginCookieToken", ""));
          //  HttpContext.Current.Response.Cookies.Clear();
           
        }  
        /// <summary>
        /// 解密票据信息
        /// </summary>
        /// <param name="encryptTicket"></param>
        /// <returns></returns>
        public static bool ValidateUserTicket(string encryptTicket, out dynamic userInfo)
        {
            userInfo = new { UserId = 0, LoginTime = DateTime.Now, Msg = string.Empty };
            try
            {
             
                var userTicket = FormsAuthentication.Decrypt(encryptTicket);
                var userTicketData = userTicket.UserData;

                string[] userInfoArr = userTicketData.Split(':');
                //userInfo = new { UserId = userInfoArr[0], LoginTime = DateTime.Parse(userInfoArr[1] ?? "0") };
                userInfo.UserId = userInfoArr[0];
                ///
                userInfo.LoginTime = DateTime.Parse(userInfoArr[1] ?? "0");
               
                return true;
            }
            catch (Exception e)
            {
                userInfo.Msg = e.Message+e.StackTrace+e.TargetSite+e.Source;
                return false;
            }
            return false;
        }

    }
}
