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

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule( Name="统一Apk版本管理")]
    public class AppVerController : BaseController
    {
        // GET: AppVer
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
                var app = db.T_AppVer;

                var list = from a in app select a;

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
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

        public ActionResult PostAdd(AppVer entity)
        {
            entity.AddTime = DateTime.Now;
            entity.Type = AppVerType.爱心银行;
            
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
              
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

      

        public ActionResult Delete(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                AppVer img = db.T_AppVer.Find(id);
                db.Delete<AppVer>(img);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult ChangeStateAdImg(int id, int state)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                AppVer img = db.T_AppVer.Find(id);

                img.State = state;
                db.Update<AppVer>(img);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {

            if (file == null)
            {
                Error("请选择文件");
            }
            SourceFile res = UploadFileInstance.SaveFile(file, "zzbx_Apk", AdminUser.ID);
            return Json(res);


        }

    }
}