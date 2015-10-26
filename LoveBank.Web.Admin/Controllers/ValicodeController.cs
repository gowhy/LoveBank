using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins;

namespace LoveBank.Web.Controllers
{
    public class ValicodeController : Controller
    {

        public ActionResult Image()
        {
            var vCode = new ValidateImage();
            var code = vCode.CreateValidateCode(4);
            var cookie = new HttpCookie("valicode") { Value = code.Hash() };
            Response.AppendCookie(cookie);
            var bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

    }
}
