﻿using System.Linq;
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
       [SecurityModule(Name = "一体机显示板块配置")]
    public class MachineModuleController : BaseController
    {
        /// <summary>
        /// 每页条数 
        /// </summary>
        const int PageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(MachineModuleShowManageModel model,int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var ad = db.T_MachineModuleShowManage;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from a in ad
                           join d in t_d on a.DeptId equals d.Id
                           select new MachineModuleShowManageModel
                           {
                               LinkUrl = a.LinkUrl,
                               ModuleKey = a.ModuleKey,
                               Name = a.Name,
                               IconUrl = a.IconUrl,
                               Sort = a.Sort,
                               Guid = a.Guid,
                               Type = a.Type,
                               AddTime = a.AddTime,
                               ID = a.ID,
                               DeptId = a.DeptId,
                               Department = d,
                               State = a.State
                           };

                list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                if (!string.IsNullOrEmpty(model.DeptId)) list = list.Where(x => x.DeptId.IndexOf(model.DeptId) > -1);


                model.MachineModuleModelList = list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size);

                var list2 = t_d.Where(x => x.Level <= 6 && x.Id.IndexOf(AdminUser.DeptId) > -1).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list2);
                return View(model);
            }
        }


        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var dep = db.T_Department;

                //部门组织
                //var list = DbProvider.D<Department>().Where(x => x.Level <= 6).ToList();
                var list = dep.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                return View();
            }
        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(MachineModuleShowManageModel parm)
        {
            #region 初始化参数
            MachineModuleShowManage model = new MachineModuleShowManage();

            model.AddTime = DateTime.Now;
            model.AddUserId = AdminUser.ID;
            model.State = RowState.有效;
            model.Guid = Guid.NewGuid().ToString();


            model.DeptId = parm.DeptId;
            model.IconUrl = parm.IconUrl;
            model.LinkUrl = parm.LinkUrl;
            model.ModuleKey = parm.ModuleKey;
            model.Name = parm.Name;
            model.Sort = parm.Sort;
            model.Type = parm.Type;
            model.Icon = parm.Icon;
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

                var ad = db.T_MachineModuleShowManage;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;
                var t_s = db.T_SourceFile;

                //部门组织
                var list = db.T_Department.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                var model = (from a in ad
                             join d in t_d on a.DeptId equals d.Id
                             where a.ID == id
                             select new MachineModuleShowManageModel
                             {
                                 LinkUrl = a.LinkUrl,
                                 ModuleKey = a.ModuleKey,
                                 Name = a.Name,
                                 IconUrl = a.IconUrl,
                                 Sort = a.Sort,
                                 Guid = a.Guid,
                                 Type = a.Type,
                                 AddTime = a.AddTime,
                                 ID = a.ID,
                                 DeptId = a.DeptId,
                                 Department = d,
                                 State = a.State,
                                 Icon=a.Icon,
                                 SourceFileList = t_s.Where(x => x.Guid == a.Guid).ToList()
                             }).FirstOrDefault();

                return View(model);
            }

        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(MachineModuleShowManageModel parm)
        {



            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_m = db.T_Machine;
                var t_a = db.T_LoveBank_Ad;
                var t_s = db.T_SourceFile;
                var am = db.T_MachineModuleShowManage;

                #region 初始化参数
                MachineModuleShowManage model = am.Find(parm.ID);


                model.AddTime = DateTime.Now;
                model.AddUserId = AdminUser.ID;
                model.State = RowState.有效;
                //model.Guid = Guid.NewGuid().ToString();


                model.DeptId = parm.DeptId;
                model.IconUrl = parm.IconUrl;
                model.LinkUrl = parm.LinkUrl;
                model.ModuleKey = parm.ModuleKey;
                model.Name = parm.Name;
                model.Sort = parm.Sort;
                model.Type = parm.Type;
                model.Icon = parm.Icon;
               
                foreach (var item in parm.SourceFileList)
                {
                    item.Guid = model.Guid;
                    item.AddTime = DateTime.Now;

                }



                ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                var delSourceFile = from s in t_s where s.Guid == model.Guid select s;
                db.T_SourceFile.RemoveRange(delSourceFile);
                db.SaveChanges();
                #endregion

                db.Update<MachineModuleShowManage>(model);
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
            var ad = DbProvider.D<MachineModuleShowManage>().FirstOrDefault(x => x.ID == id);
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
            SourceFile res = UploadFileInstance.SaveFile(file, "MachineModuleShowManageImg", AdminUser.ID);
            return Json(res);
        }






    }
}