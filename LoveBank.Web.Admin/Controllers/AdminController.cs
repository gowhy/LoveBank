using System.Linq;
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
using System.Collections.Generic;
using LoveBank.Core.SerializerHelp;
using System;
using LoveBank.MVC;

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "管理员")]
    public class AdminController : BaseController
    {
        protected IAdminService AdminService;

        public AdminController(IAdminService adminService)
        {
            Check.Argument.IsNotNull(adminService, "adminService");
        
            AdminService = adminService;
        }

        [SecurityNode(Name = "管理员列表")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 20;
            var pageNumber = page ?? 1;

            //if (sort.Column == null) sort.Column = "AdminUser.ID";

            //ViewData["sort"] = sort;
            //var source = DbProvider.D<AdminUser>().OrderBy(sort.Column, sort.Direction == SortDirection.Ascending);



            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_User = db.T_AdminUser;
                var t_Role = db.T_Role;
                var t_Dep = db.T_Department;

                var list = from u in t_User
                           join r in t_Role on u.RoleID equals r.ID
                           join d in t_Dep on u.DeptId equals d.Id

                           select new AdminUserModel
                           {
                               AdminUser = u,
                               Department = d,
                               Role = r
                           };
               // return View(list.OrderBy(sort.Column, sort.Direction == SortDirection.Ascending).ToPagedList(pageNumber - 1, pageSize));
                  return View(list.OrderByDescending(x => x.AdminUser.ID).ToPagedList(pageNumber - 1, pageSize));
            }

          
         
        }

        [SecurityNode(Name = "管理员回收站")]
        public ActionResult Trash(int? page, GridSortOptions sort)
        {
            const int pageSize = 20;
            var pageNumber = page ?? 1;

            if (sort.Column == null) sort.Column = "ID";

            ViewData["sort"] = sort;
            var source = DbProvider.D<AdminUser>().OrderBy(sort.Column, sort.Direction == SortDirection.Ascending);

            return View(source.ToPagedList(pageNumber - 1, pageSize));
        }

        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                IList<Role> role = db.T_Role.AsQueryable<Role>().ToList();
               
                ViewData["UserRole"] = role;

                //部门组织
                var list = db.T_Department.AsQueryable<Department>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
                db.Dispose();
                return PartialView();
            }
        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(AdminUser model)
        {
            model.LoginTime = DateTime.Now;
            model.LoginIP =Utility.GetIP();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                if (db.T_AdminUser.Count(u => u.UserName.Trim() == model.UserName.Trim()) > 0)
                {
                    db.Dispose();
                    return Error("用户已经存在");
                }
               
                db.T_AdminUser.Add(model);
                db.SaveChanges();
                return Success("操作成功");
            }
        

        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                AdminUser user = db.T_AdminUser.Find(id);

                List<Role> role = db.T_Role.Where(x=>x.ID>0).ToList();

                ViewData["UserRole"] = role;


                //部门组织
                var list = db.T_Department.Where(x => x.Level <= 6).ToList();
                if (list != null)
                {
                   ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);
                }
                db.Dispose();

                return PartialView(user);
            }
        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(AdminUser model)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                AdminUser user = db.T_AdminUser.First(x => x.ID == model.ID);
                if (user == null) return Error("用户不存在");

                user.DeptId = model.DeptId;
                user.RealName = model.RealName;
                user.RoleID = model.RoleID;
                db.Update<AdminUser>(user);
                db.SaveChanges();

                return Success("修改成功");
            }

        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int[] id)
        {
            AdminService.DelAdmin(id);
            return Success("删除成功");
        }

        [HttpPost]
        [SecurityNode(Name = "永久删除")]
        public ActionResult Delete(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                AdminUser user = db.T_AdminUser.Find(id);
                db.Delete<AdminUser>(user);
                db.SaveChanges();
                return Success("操作成功");
            }
        }

     
    }
}
