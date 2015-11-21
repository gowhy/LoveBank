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
   
    [SecurityModule(Name="启动和底部广告")]
    public class LBStartOrBottomAdController : BaseController
    { 

        /// <summary>
        /// 每页条数
        /// </summary>
        const int PageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int?  pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var ad = db.T_LBStartOrBottomAd;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from a in ad
                           join d in t_d on a.DeptId equals d.Id

                           select new LBStartOrBottomAdModel
                           {
                               AddTime = a.AddTime,
                               Title = a.Title,
                               ID = a.ID,
                               DeptId = a.DeptId,
                               Desc = a.Desc,
                               State = a.State,
                               AddUserId = a.AddUserId,
                               AddUserDeptId = a.AddUserDeptId,
                               Postion = a.Postion,
                               Department = d

                           };

                list = list.Where(x => x.State != RowState.删除);
                list = list.Where(x => x.AddUserDeptId.IndexOf(AdminUser.DeptId) > -1);

                return View(list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size));
            }
        }


        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_a = db.T_LBStartOrBottomAd;
                var t_s = db.T_SourceFile;
                var t_d = db.T_Department;

                var list = t_d.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
            }
        
            return View();
        }


        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(LBStartOrBottomAdModel parm)
        {
            #region 初始化参数
            LBStartOrBottomAd model = new LBStartOrBottomAd();

            model.AddTime = DateTime.Now;
            model.AddUserId = AdminUser.ID;
            model.State = RowState.有效;
            model.Guid = Guid.NewGuid().ToString();

            model.LinkUrl = parm.LinkUrl;     
            model.Title = parm.Title;
            model.Desc = parm.Desc;
            model.Postion = parm.Postion;
            model.DeptId = parm.DeptId;
            model.AddUserDeptId = AdminUser.DeptId;

            foreach (var item in parm.SourceFileList)
            {
                item.Guid = model.Guid;
                item.AddTime = DateTime.Now;

            }
            #endregion

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(model);
                db.SaveChanges();
                db.T_SourceFile.AddRange(parm.SourceFileList);
                db.SaveChanges();

                return Success("添加成功");

            }
        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_a = db.T_LBStartOrBottomAd;
                var t_s = db.T_SourceFile;
                var t_d = db.T_Department;

                var model = (from a in t_a
                             join d in t_d on a.DeptId equals d.Id
                             where a.ID == id
                             select new LBStartOrBottomAdModel
                             {
                                 AddTime = a.AddTime,
                                 Department = d,
                                 Title = a.Title,
                                 ID = a.ID,
                                 DeptId = a.DeptId,
                                 Desc = a.Desc,
                                 State = a.State, 
                                 Postion=a.Postion,
                                 LinkUrl = a.LinkUrl,
                                 SourceFileList = t_s.Where(x => x.Guid == a.Guid).ToList()
                             }).SingleOrDefault();

                var list = t_d.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
                return View(model);
            }
          
        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(LBStartOrBottomAdModel parm)
        {

          

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_m = db.T_Machine;
                var t_a = db.T_LBStartOrBottomAd;
                var t_s = db.T_SourceFile;

                #region 初始化参数
                LBStartOrBottomAd model = t_a.Find(parm.ID);

               
            
                model.Title = parm.Title;
                model.Desc = parm.Desc;
                model.LinkUrl = parm.LinkUrl;
                model.DeptId = parm.DeptId;
                model.Postion = parm.Postion;

             
                ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                var delSourceFile = from s in t_s where s.Guid == model.Guid select s;
                db.T_SourceFile.RemoveRange(delSourceFile);
                db.SaveChanges();
                #endregion


                          
                db.Update<LBStartOrBottomAd>(model);
                db.SaveChanges();

                foreach (var item in parm.SourceFileList)
                {
                    item.Guid = model.Guid;
                    item.AddTime = DateTime.Now;
                }

                db.T_SourceFile.AddRange(parm.SourceFileList);//重新绑定
                db.SaveChanges();

                return Success("修改成功");

            }

        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {
            var ad = DbProvider.D<LBStartOrBottomAd>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.RowState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }

        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {



            if (file == null)
            {
                Error("请选择文件");
            }

            SourceFile res = UploadFileInstance.SaveFile(file, "LBStartOrBottomAdImg", AdminUser.ID);
            //res.ClientFileId = id;
            return Json(res);


        }
    }
}