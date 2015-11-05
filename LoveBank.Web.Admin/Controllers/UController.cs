using LoveBank.Core.Domain;
using LoveBank.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveBank.Web.Admin.Controllers
{
    public class UController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            
            SourceFile res = UploadFileInstance.SaveFile(file, "UploadImg", 0);
            return Json(res);
        }
    }
}