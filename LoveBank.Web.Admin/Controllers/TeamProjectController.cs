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
using System.Threading.Tasks;


namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "公益活动项目")]
    public class TeamProjectController : BaseController
    {

        // GET: Product
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

                var t_p = db.T_TeamProject;

                var list = from p in t_p select p;

                list = list.Where(x => x.State != RowState.删除);

                //list = list.Where(x => x.Department.Id.IndexOf(AdminUser.DeptId) > -1);

                return View(list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size));
            }
        }

        [SecurityNode(Name = "详情页面")]
        public ActionResult View(int? id)
        {
            return View();

        }

        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
            return View();

        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(TeamProjectModel parm)
        {
            #region 初始化参数
            TeamProject model = new TeamProject();
            model.AddTime = DateTime.Now;
            model.AddUserId = AdminUser.ID;
            model.State = RowState.有效;

          

            model.Name = parm.Name;
            model.Desc = parm.Desc;
            model.Type = parm.Type;
            model.RecruitStartDate = parm.RecruitStartDate;
            model.RecruitEndDate = parm.RecruitEndDate;
            model.ProjectStartDate = parm.ProjectStartDate;
            model.ProjectEndDate = parm.ProjectEndDate;
            model.ServiceDate = parm.ServiceDate;
            model.Address = parm.Address;
            model.ServiceObject = parm.ServiceObject;
            model.HtmlUrl = parm.HtmlUrl;
            model.LinkMan = parm.LinkMan;
            model.LinkPhone = parm.LinkPhone;
            model.Score = parm.Score;
            model.CommentSocre = parm.CommentSocre;
            model.GoodScore = parm.GoodScore;
            model.ShareScore = parm.ShareScore;



            #endregion

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                db.Add(model);
                db.SaveChanges();
                if (parm.SourceFileList != null)
                {
                    foreach (var item in parm.SourceFileList)
                    {
                        item.Guid = model.Guid;
                        item.AddTime = DateTime.Now;

                    }
                    db.T_SourceFile.AddRange(parm.SourceFileList);
                    db.SaveChanges();
                }

                return Success("添加成功");
            }
        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_TeamProject;
              
                var t_s = db.T_SourceFile;



                var model = (from p in t_p
                             where p.ID == id
                             select new TeamProjectModel
                             {
                                 AddTime = p.AddTime,

                                 Name = p.Name,
                                 State = p.State,
                                 ID = p.ID,
                                 Desc = p.Desc,
                                 HtmlUrl = p.HtmlUrl,
                                 LinkMan = p.LinkMan,
                                 LinkPhone = p.LinkPhone,
                                 ProjectEndDate = p.ProjectEndDate,
                                 ProjectStartDate = p.ProjectStartDate,
                                 RecruitEndDate = p.RecruitEndDate,
                                 RecruitStartDate = p.RecruitStartDate,
                                 Score = p.Score,
                                 ServiceDate = p.ServiceDate,
                                 ServiceObject = p.ServiceObject,
                                 Type = p.Type,
                                 Address=p.Address,
                                   
                                 SourceFileList = t_s.Where(x => x.Guid == p.Guid).ToList()
                             }).SingleOrDefault();

                return View(model);
            }
        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(TeamProjectModel parm)
        {


            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var am = db.T_TeamProject;
                TeamProject model = am.Find(parm.ID);

                model.Name = parm.Name;
                model.Desc = parm.Desc;
                model.Type = parm.Type;
                model.RecruitStartDate = parm.RecruitStartDate;
                model.RecruitEndDate = parm.RecruitEndDate;
                model.ProjectStartDate = parm.ProjectStartDate;
                model.ProjectEndDate = parm.ProjectEndDate;
                model.ServiceDate = parm.ServiceDate;
                model.Address = parm.Address;
                model.ServiceObject = parm.ServiceObject;
                model.HtmlUrl = parm.HtmlUrl;
                model.LinkMan = parm.LinkMan;
                model.LinkPhone = parm.LinkPhone;
                model.Score = parm.Score;
                model.CommentSocre = parm.CommentSocre;
                model.GoodScore = parm.GoodScore;
                model.ShareScore = parm.ShareScore;


                db.Update<TeamProject>(model);
                db.SaveChanges();
                return Success("修改成功");
            }
        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {
            var ad = DbProvider.D<TeamProject>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.RowState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }


        [SecurityNode(Name = "添加岗位页面")]
        public ActionResult AddTeamPojectStation(int teamPojectId,int page=1,int pageSize=100)
        {
            ViewBag.TeamPojectId = teamPojectId;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_TeamPojectStation;

                var list = from p in t_p  select p;

                list = list.Where(x => x.State != RowState.删除&&x.TeamPojectId==teamPojectId);


                return View(list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize));
            }

        }

        [HttpPost]
        [SecurityNode(Name = "添加岗位执行")]
        public ActionResult PostAddTeamPojectStation(TeamPojectStation parm)
        {

            parm.AddTime = DateTime.Now;
            parm.AddUserId = AdminUser.ID;
            parm.State = RowState.有效;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
              

                db.Add<TeamPojectStation>(parm);
                db.SaveChanges();
            }
            return  Success("新增成功");
        }


        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Error("请选择文件");
            }

            SourceFile res = UploadFileInstance.SaveFile(file, "TeamProjectImg", AdminUser.ID);
            return Json(res);

        }
    }
}