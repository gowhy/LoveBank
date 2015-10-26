using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using LitJson;
using LoveBank.Common.Plugins;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;
using LoveBank.Core.Payments;
using LoveBank.MVC.SiteMap;
using LoveBank.Web.Admin.Code;
using SiteMapNode = LoveBank.MVC.SiteMap.SiteMapNode;

namespace LoveBank.Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Default/
        //[OutputCache(Duration = 6000)]
        public ActionResult Index()
        {


            var list = DbProvider.D<MenuEntity>().ToList();

            //var rolelist = db.RolePermission.AsQueryable().Where(x => x.RoleId == AdminUser.RoleId).ToList();
            var rolelist = DbProvider.D<RoleAccess>().Where(x => x.RoleId == AdminUser.RoleID).ToList(); ;


            List<MenuEntity> userMenuList = new List<MenuEntity>();
            foreach (var ritem in rolelist)
            {
                if (!string.IsNullOrEmpty(ritem.Module))
                {

                    userMenuList.AddRange(list.Where(x => x.Module.ToLower() == ritem.Module.ToLower()));
                }
                else
                {
                    userMenuList.AddRange(list.Where(x => x.file.Trim().ToLower() == "/" + ritem.Node.Trim().ToLower().Replace('_', '/')));
                }
            }

            List<MenuEntity> Root = userMenuList.Where(x => x.pId == 0).ToList();

            foreach (var item in Root)
            {
                item.ChildList = userMenuList.Where(x => x.pId == item.ID).ToList();
            }

            return View(Root);
        }




        public ActionResult RemoveCache()
        {
            var url = Url.Action("Main", "Home");
            if (url != null) HttpResponse.RemoveOutputCacheItem(url);
            return RedirectToAction("Main");
        }

        public ActionResult Drag()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return View();
        }

        public ActionResult UploadImage()
        {
            var context = HttpContext;
            var path = System.Configuration.ConfigurationManager.AppSettings["ImageSavePath"];
            var savePath = path;
            //var saveUrl = savePath;
            var saveUrl = "/";

            var extTable = new Hashtable
                {
                    {"image", "gif,jpg,jpeg,png,bmp"},
                    {"flash", "swf,flv"},
                    {"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                    {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
                };

            const int maxSize = 1000000;

            var imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                return Content("请选择文件。");
            }

            var dirPath = savePath;

            if (!Directory.Exists(dirPath))
            {
                return Content("上传目录不存在。");
            }

            var dirName = context.Request.QueryString["dir"];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return Content("目录名不正确。");
            }

            var fileName = imgFile.FileName;
            var fileExt = Path.GetExtension(fileName).ToLower();

            var imageStream = imgFile.InputStream;

            if (imageStream == null || imageStream.Length > maxSize)
            {
                return Content("上传文件大小超过限制。");
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Content("上传文件扩展名是不允许的扩展名。\n只允许" + ((string)extTable[dirName]) + "格式。");
            }

            dirPath += dirName + "/";
            //saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var filePath = dirPath + newFileName;

            var fileUrl = "/Image/GetImage?file=" + saveUrl + newFileName;

            var image = WaterImage.AddWaterStringToImage(Image.FromStream(imageStream), "", "微软雅黑", 32, Color.White, 10, 10);

            //保存加水印过后的图片
            image.Save(filePath);
            image.Dispose();

            var hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();

            return null;
        }
    }
}
