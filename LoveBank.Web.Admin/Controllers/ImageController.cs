using System.Web.Mvc;

namespace LoveBank.Web.Admin.Controllers
{
    public class ImageController : Controller
    {

        public ActionResult GetImage(string file)
        {
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
