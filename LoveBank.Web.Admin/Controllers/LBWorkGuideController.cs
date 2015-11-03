using System.Linq;
using System;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Services.AdminModule;
using LoveBank.Common.Data;
using LoveBank.Web.Admin.Models;
using LoveBank.MVC.Security;
using LoveBank.Core.MSData;
using LoveBank.Core.Domain.Enums;
using System.IO;
using System.Web;
using LoveBank.Services;
using LoveBank.Core.SerializerHelp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "工作指南")]
    public class LBWorkGuideController : BaseController
    {
        private static int PageSize = 20;
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

        

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var work = db.T_WorkGuide;

                var list = from w in work select w;

                list = list.Where(x => x.DepId == AdminUser.DeptId);

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var work = db.T_WorkGuide;

                var list = from w in work select w;
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }
        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var list = db.T_Department.AsQueryable<Department>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(WorkGuide entity)
        {
         
            entity.DepId = AdminUser.DeptId;
            entity.AddUser = AdminUser.UserName;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, "workGuide/" + Dir, FtpUser, FtpPassWord);

            FileInfo file2 = new FileInfo(file.FileName);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.ID.ToString() + file2.Extension;

            ftp.Upload(file, fileName);

            SocSerImgEntity img = new SocSerImgEntity();
            img.FTPUrl = ftp.FtpURI;
            img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
            img.Name = file.FileName;
            img.Module = "保存服务";
            img.AddTime = DateTime.Now;

            return Json(img);
        }
        public ActionResult ImgFile()
        {
            string action = Request["action"];
            if (action == "config")
            {
                string json = System.IO.File.ReadAllText(HttpContext.Server.MapPath("../config.json"));
                return Content(json);
            }

            if (action == "uploadimage")
            {
                var file = Request.Files["upfile"];

                //接收上传后的文件
                // System.Web.HttpPostedFileBase file = Request.Files["imgFile"];

                string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
                string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
                string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
                string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

                string Dir = DateTime.Now.ToString("yyyyMMdd");
                FTPHelper ftp = new FTPHelper(FtpServer, "workGuide/" + Dir, FtpUser, FtpPassWord);

                FileInfo file2 = new FileInfo(file.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.ID.ToString() + file2.Extension;

                ftp.Upload(file, fileName);

                string imgUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
                return Json(new
                {
                    state = "SUCCESS",
                    url = imgUrl,
                    title = fileName,
                    original = file.FileName,
                    error = ""
                }, "text/html");
                //return Json(new { url = img.HttpUrl, error = 0 }, "text/html");
            }
            return Json(new
            {
                state = "Error",
                error = ""
            }, "text/html");
        }

        public ActionResult Delete(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                WorkGuide notice = db.T_WorkGuide.Find(id);
                db.Delete<WorkGuide>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult View(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                WorkGuide work = db.T_WorkGuide.Find(id);
                return View(work);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(WorkGuide entity)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                WorkGuide work = db.T_WorkGuide.Find(entity.Id);

                work.AddTime = DateTime.Now;
                work.Des = entity.Des;
                work.AddUser = entity.AddUser;
                work.ImgUrl = entity.ImgUrl;
                work.Title = entity.Title;
                work.LinkSocSerUrl = entity.LinkSocSerUrl;
                work.UploadHtmlFile = entity.UploadHtmlFile;
                db.Update(work);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult AppView(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                WorkGuide work = db.T_WorkGuide.Find(id);
                return Json(work);
            }
        }
        /// <summary>
        /// App推送消息标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PushMsg(int id)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                WorkGuide notice = db.T_WorkGuide.Find(id);
                if (notice.State >= 1)
                {
                    return Json("已经推送,不能再推送");
                }
                notice.State = 1;

                db.Update<WorkGuide>(notice);
                db.SaveChanges();
                db.Dispose();
                return Json("操作成功");
            }
        }

        public ActionResult SaveUploadHtmlFile()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


            SocSerImgEntity res = UploadFileInstance.SaveFileOld(file, "WorkGuidHtmlFile", "");

            return Json(res);
        }
    }
}