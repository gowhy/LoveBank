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
    [SecurityModule(Name = "自愿者团体")]
    public class TeamController : BaseController
    {
        /// <summary>
        /// 每页条数 
        /// </summary>
        const int PageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_t = db.T_Team;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from a in t_t
                           join d in t_d on a.DeptId equals d.Id
                           select new TeamModel
                           {
                               Address = a.Address,
                               Desc = a.Desc,
                               EstabDate = a.EstabDate,
                               Level = a.Level,
                               LinkMan = a.LinkMan,
                               LinkPhone = a.LinkPhone,
                               Name = a.Name,
                               WebSite = a.WebSite,
                               AddTime = a.AddTime,
                               Id = a.ID,
                               DeptId = a.DeptId,
                               Department = d,
                               State = a.State
                           };

                list = list.Where(x => x.State != TeamState.删除);


                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
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
        public ActionResult PostAdd(Team parm, IList<SourceFile> SourceFileList)
        {
            #region 初始化参数
            //Team model = new Team();

            parm.AddTime = DateTime.Now;
            parm.AddUserId = AdminUser.ID;
            parm.State = TeamState.审核通过;
            parm.Guid = Guid.NewGuid().ToString();



            foreach (var item in SourceFileList)
            {
                item.Guid = parm.Guid;
                item.AddTime = DateTime.Now;

            }
            #endregion

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(parm);
                db.SaveChanges();

                db.T_SourceFile.AddRange(SourceFileList);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "添加成功";
                return Json(retJson);

            }
        }



        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_t = db.T_Team;

                var t_d = db.T_Department;
                var t_s = db.T_SourceFile;

                var list = t_d.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                var model = (from a in t_t
                             join d in t_d on a.DeptId equals d.Id
                             where a.ID == id
                             select new TeamModel
                             {
                                 Address = a.Address,
                                 Desc = a.Desc,
                                 EstabDate = a.EstabDate,
                                 Level = a.Level,
                                 LinkMan = a.LinkMan,
                                 LinkPhone = a.LinkPhone,
                                 Name = a.Name,
                                 WebSite = a.WebSite,
                                 AddTime = a.AddTime,
                                 Id = a.ID,
                                 DeptId = a.DeptId,
                                 Department = d,
                                 State = a.State,
                                 SourceFileList = t_s.Where(x => x.Guid == a.Guid).ToList()
                             }).SingleOrDefault();

                return View(model);
            }

        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(TeamModel parm)
        {



            using (LoveBankDBContext db = new LoveBankDBContext())
            {


                var t_a = db.T_LoveBank_Ad;
                var t_s = db.T_SourceFile;
                var am = db.T_Team;

                #region 初始化参数
                Team model = am.Find(parm.Id);


                model.DeptId = parm.DeptId;
                model.Address = parm.Address;

                model.Desc = parm.Desc;
                model.EstabDate = parm.EstabDate;
                model.Level = parm.Level;
                model.LinkMan = parm.LinkMan;
                model.LinkPhone = parm.LinkPhone;
                model.Name = parm.Name;
                model.WebSite = parm.WebSite;


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

                db.Update<Team>(model);
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
            var ad = DbProvider.D<Team>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.TeamState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }

        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Error("请选择文件");
            }
            SourceFile res = UploadFileInstance.SaveFile(file, "TeamImg", AdminUser.ID);
            return Json(res);
        }



        [SecurityNode(Name = "添加项目页面")]
        public ActionResult AddTeamProject()
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
        [SecurityNode(Name = "添加项目执行")]
        public ActionResult PostAddTeamProject(TeamProjectModel parm)
        {
            #region 初始化参数
            //Team model = new Team();

            parm.AddTime = DateTime.Now;
            parm.AddUserId = AdminUser.ID;
            parm.State = RowState.有效;
            parm.Guid = Guid.NewGuid().ToString();

            #endregion
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(parm);
                db.SaveChanges();
                retJson.Status = true;
                retJson.Info = "添加成功";
                return Json(retJson);

            }
        }

        [SecurityNode(Name = "添加项目岗位页面")]
        public ActionResult AddTeamProjectStation()
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
        [SecurityNode(Name = "添加项目岗位执行")]
        public ActionResult PostAddTeamProject(TeamPojectStation parm)
        {
            #region 初始化参数
            //Team model = new Team();

            parm.AddTime = DateTime.Now;
            parm.AddUserId = AdminUser.ID;
            parm.State = 0;

            #endregion
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(parm);
                db.SaveChanges();
                retJson.Status = true;
                retJson.Info = "添加成功";
                return Json(retJson);

            }


            foreach (int myCode in Enum.GetValues(typeof(RowState)))
            {

                string strName = Enum.GetName(typeof(RowState), myCode);//获取名称

                string strVaule = myCode.ToString();//获取值



            }
        }
    }
}