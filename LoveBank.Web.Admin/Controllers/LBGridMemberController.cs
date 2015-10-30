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
    [SecurityModule(Name = "网格员")]
    public class LBGridMemberController : BaseController
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

                var work = db.T_GridMember;
                var dep = db.T_Department;
                var list = from w in work select w;

                list = list.Where(x => x.DeptId.IndexOf(AdminUser.DeptId) > -1);

                var depNameList = (from d in dep where d.PId.IndexOf(AdminUser.DeptId) > -1 select d).ToList();
                foreach (var item in list)
                {
                    if (item != null && !string.IsNullOrEmpty(item.VDeptId))
                    {
                        foreach (var item2 in item.VDeptId.Split(','))
                        {
                            string tmp = (depNameList.FirstOrDefault(x => x.Id == item2) ?? new Department()).Name;
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                item.VDeptName += tmp + ",";
                            }
                        }
                    }
                }

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }



        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Error("请选择文件");
            }

            SourceFile res = UploadFileInstance.SaveFile(file, "GridMemberImg", AdminUser.ID);
            return Json(res);

        }

        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                //获取当前登录用户下的小区信息
                var list = db.T_Department.AsQueryable<Department>().Where(x => x.Level == 7 && x.PId == AdminUser.DeptId).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(GridMemberModel entity)
        {
            if (string.IsNullOrEmpty(entity.GridPhone) || string.IsNullOrEmpty(entity.GridName) || string.IsNullOrEmpty(entity.GridNo))
            {
                return RedirectToAction("Add");
            }
            GridMember model = new GridMember();

            model.AddUserId = AdminUser.ID;
            model.AddTime = DateTime.Now;
            model.DeptId = AdminUser.DeptId;

            model.Desc = entity.Desc;
            model.GridName = entity.GridName;
            model.GridNo = entity.GridNo;
            model.GridPhone = entity.GridPhone;
            model.VDeptId = entity.VDeptId;
            model.GridHeaderImg = entity.SourceFile.Domain + entity.SourceFile.Path;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(model);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

      


        public ActionResult Delete(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                GridMember notice = db.T_GridMember.Find(id);
                db.Delete<GridMember>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult Edit(int id)
        {


            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var dep = db.T_Department;
                var grid = db.T_GridMember;
           
                var glist =( from g in grid
                            //join d in dep on g.VDeptId equals d.Id
                            where g.Id==id
                            select new GridMemberModel
                            {

                                Desc = g.Desc,
                                VDeptId = g.VDeptId,
                                GridNo = g.GridNo,
                                GridPhone = g.GridPhone,
                                GridName = g.GridName,
                                GridHeaderImg = g.GridHeaderImg,
                                DeptId = g.DeptId,
                                Id = g.Id
                            }).FirstOrDefault();
                glist.VDeptName = dep.Where(x => glist.VDeptId.IndexOf(x.Id) > -1).FirstOrDefault().Name;

                #region    //恢复图片数组
                SourceFile sf = new SourceFile();
                sf.Id = glist.Id+DateTime.Now.Millisecond;

                Uri uri = new Uri(glist.GridHeaderImg);
                sf.Domain =uri.Scheme+ "://"+uri.Authority;
                sf.Path = uri.LocalPath;
                sf.FileName = uri.Segments[3];

                glist.SourceFileList = new List<SourceFile>();
                glist.SourceFileList.Add(sf);
                #endregion


                //获取当前登录用户下的小区信息
                var list = db.T_Department.AsQueryable<Department>().Where(x => x.Level == 7 && x.PId == AdminUser.DeptId).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
                return View(glist);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(GridMemberModel entity)
        {
           

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                GridMember model = db.T_GridMember.Find(entity.Id);

                model.Desc = entity.Desc;
                model.GridName = entity.GridName;
                model.GridNo = entity.GridNo;
                model.GridPhone = entity.GridPhone;
                model.VDeptId = entity.VDeptId;
                model.GridHeaderImg = entity.SourceFile.Domain + entity.SourceFile.Path;

                //grid.Desc = entity.Desc;
                //grid.GridHeaderImg = entity.GridHeaderImg;
                //grid.GridName = entity.GridName;
                //grid.GridNo = entity.GridNo;
                //grid.GridPhone = entity.GridPhone;
                //grid.VDeptId = entity.VDeptId;

                db.Update(model);
                db.SaveChanges();

            }
            return Success("修改成功");
        }

    }
}