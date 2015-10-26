using System.Web;

namespace LoveBank.MVC
{
    public static class Utility  
    {
        public static string GetIP()
        {
            var ip = "0.0.0.0";
            var request = HttpContext.Current.Request;
            if (request.ServerVariables["HTTP_VIA"] != null) //using proxy
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];  // Return real client IP.
            }
            else  //not using proxy or can't get the Client IP
            {
                ip = request.ServerVariables["REMOTE_ADDR"]; //While it can't get the Client IP, it will return proxy IP.
            }
            return ip;
        }
    }
}
