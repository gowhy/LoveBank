using System.Web.Mvc;
using LoveBank.Web.Code;

namespace LoveBank.Web.Controllers
{
    public class ImageController : Controller
    {

        public ActionResult GetImage(string file, string sign = "e5RPVsWJ")
        {
            if (!sign.Equals("e5RPVsWJ") && !LoveBankContext.Current.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "Account", new { ReturnUrl = Request["ReturnUrl"] });
            }
            var path = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"];
            var context = HttpContext;
            var dirPath = path + "image";
            var filePath = dirPath + file;
            Response.ContentType = "image/jpg";
            context.Response.WriteFile(filePath);
            return null; 
        }

        public ActionResult GetThumb(string file)
        {
            var path = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"];
            var context = HttpContext;
            var dirPath = path + "thumb";
            var filePath = dirPath + file;
            Response.ContentType = "image/jpg";
            context.Response.WriteFile(filePath);
            return null; 
        }

    }
}
