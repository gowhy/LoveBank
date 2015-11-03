using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using LoveBank.Common;
using LoveBank.Core;
using LoveBank.Core.Members;
using LoveBank.Common.Data;
using LoveBank.Services.Members;
using LoveBank.Web.Admin.Models;
using LoveBank.MVC.Security;
using LoveBank.Core.Domain;
using LoveBank.Core.SerializerHelp;
using LoveBank.Core.MSData;
using LoveBank.Core.Domain.Enums;
namespace LoveBank.Web.Admin.Controllers
{
     [SecurityModule(Name = "社区自愿者管理")]
    public class LoveBankVolController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int PageSize = 20;
        [SecurityNode(Name = "首页")]
        public ActionResult Index(VolModelSelect volModel, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_v = db.T_Vol;
                var t_d = db.T_Department;

                var list = from v in t_v
                           join d in t_d on v.DepId equals d.Id
                           select new VolModel 
                           { 
                              Vol=v,
                              Department=d
                           };

                //list = list.Where(x => x.Department.Id.IndexOf(AdminUser.DeptId)>-1);
                list = list.Where(x => x.Department.Id.StartsWith(AdminUser.DeptId));
                if (!string.IsNullOrEmpty(volModel.Phone)) list = list.Where(x => x.Vol.Phone == volModel.Phone);
                if (!string.IsNullOrEmpty(volModel.NFC)) list = list.Where(x => x.Vol.NFC == volModel.NFC);
                if (!string.IsNullOrEmpty(volModel.RealName)) list = list.Where(x => x.Vol.RealName.Contains(volModel.RealName));
                volModel.VolList = list.OrderByDescending(x => x.Vol.ID).ToPagedList(pageNumber - 1, size);
                return View(volModel);
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

        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(Vol model)
        {
            model.RealNameState = 1;
            model.State = 1;
            model.Type = "志愿者";
            model.LoveBankScore = 0;
            model.Score = 0;
            DbProvider.Add(model);
            DbProvider.SaveChanges();
            model.Score = 1;//来源未1标示是爱心银行
            return Success("操作成功");

        }


        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {

            //Vol machine = DbProvider.D<Vol>().FirstOrDefault(x => x.ID == id);
            //if (machine == null) return Error("机器不存在");
            //machine.State = RowState.删除;
            //DbProvider.SaveChanges();
            return Success("操作成功");

        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {

  

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_v = db.T_Vol;
                var t_d = db.T_Department;
               
                //部门组织
                var listDep = t_d.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(listDep);

                var list = from v in t_v
                           join d in t_d on v.DepId equals d.Id
                           select new VolModel
                           {
                               Vol = v,
                               Department = d
                           };

                VolModel model = list.Single(x => x.Vol.ID == id);
                return View(model);
            }

        }



        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(Vol model)
        {
          
            Vol dbEntity=DbProvider.D<Vol>().FirstOrDefault(x => x.Phone == model.Phone&&x.ID!=model.ID);
            if (dbEntity !=null&& dbEntity.Phone != null)
            {
                Error("该手机号已经存在");
            }

            Vol vol = DbProvider.D<Vol>().FirstOrDefault(x => x.ID == model.ID);
            vol.Phone = model.Phone;
            vol.RealName = model.RealName;
            vol.Sex = model.Sex;
            vol.Age = model.Age;
            vol.Address = model.Address;
            vol.DepId = model.DepId;
            vol.VolType = model.VolType;

            DbProvider.SaveChanges();

            return Success("编辑保存成功");

        }

         /// <summary>
         /// 给自愿者新增积分
         /// </summary>
         /// <returns></returns>
        [SecurityNode(Name = "增加积分")]
        public PartialViewResult VolAddScore(int volId)
        {
            VolAddScoreModel volAddScoreModel = new VolAddScoreModel();

         
            volAddScoreModel.TeamProjectList = null;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;
                var t_tp = db.T_TeamProject;


                volAddScoreModel.Vol = t_v.FirstOrDefault(x => x.ID == volId);
                volAddScoreModel.TeamProjectList = (from tp in t_tp
                                                    where tp.ID > 0
                                                    select new TeamProjectModel
                                                    {
                                                        ID = tp.ID,
                                                        Name = tp.Name,
                                                        Score = tp.Score

                                                    }

                                                    ).ToList();


            }
            return PartialView(volAddScoreModel);
        }

        [SecurityNode(Name = "增加积分执行")]
        public ActionResult PostVolAddScore(VolAddScoreRecorde model)
        {
            Vol vol = DbProvider.D<Vol>().FirstOrDefault(x => x.ID == model.VolID);
            if (vol==null)
            {
                Error("用户不存在,请核实后从新操作");
            }
            model.AddUserId = base.AdminUser.ID;
            model.AuditingState = AuditingState.待审核;
            model.AddTime = DateTime.Now;
            model.DeptId = base.AdminUser.DeptId;
            DbProvider.Add(model);
            DbProvider.SaveChanges();

            return Success("操作成功,等待审核");
        }

        [SecurityNode(Name = "待审核积分列表")]
        public ActionResult AuditingAddScoreIndex(VolAddSocreRecordeSelectModel model,int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;
                var t_sr = db.T_VolAddScoreRecorde;

                var list = from s in t_sr
                           join v in t_v on s.VolID equals v.ID
                           select new VolAddSocreRecordeModel
                           {
                               AddScore = s.AddScore,
                               AddTime = s.AddTime,
                               State = s.State,
                               AuditingState = s.AuditingState,
                               AuditingMsg = s.AuditingMsg,
                               Msg = s.Msg,
                               AuditingTime = s.AuditingTime,
                               ID = s.ID,
                               DeptId = s.DeptId,
                               Vol = v

                           };

                list = list.Where(x => x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                if (!string.IsNullOrEmpty(model.Phone)) list = list.Where(x => x.Vol.Phone == model.Phone);
                if (!string.IsNullOrEmpty(model.RealName)) list = list.Where(x => x.Vol.RealName == model.RealName);
                if (!string.IsNullOrEmpty(model.NFC)) list = list.Where(x => x.Vol.NFC == model.NFC);

                if ((int)model.AuditingState>-1)
                {
                    list = list.Where(x => x.AuditingState == model.AuditingState);
                }
        

                model.VolAddSocreRecordeList = list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size);
                return View(model);
            }
        }

        [SecurityNode(Name = "待审积分页")]
        public ActionResult AuditingVolAddScore(int id)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;
                var t_sr = db.T_VolAddScoreRecorde;
                var t_tp= db.T_TeamProject;

                var list = from s in t_sr
                           join v in t_v on s.VolID equals v.ID
                           join p in t_tp on s.TeamProjectId equals p.ID
                           select new VolAddSocreRecordeModel
                           {
                               AddScore = s.AddScore,
                               AddTime = s.AddTime,
                               State = s.State,
                               AuditingState = s.AuditingState,
                               AuditingMsg = s.AuditingMsg,
                               Msg = s.Msg,
                               AuditingTime = s.AuditingTime,
                               ID = s.ID,
                               DeptId = s.DeptId,
                               Vol = v,
                               TeamProjectName = p.Name

                           };
                VolAddSocreRecordeModel volMode = list.SingleOrDefault(x => x.ID == id);
                if (volMode.AuditingState == AuditingState.审核通过)
                {
                    MsgModel mo = new MsgModel();
                    mo.Message = "已审核通过,不能在审核";
                    mo.WaitSecond = -1;
                    mo.Title = "错误";
                    return View("_Message", mo);
                    //return Error("已审核通过,不能在审核");
                }

                return PartialView(volMode);
            }
        }

        [SecurityNode(Name = "待审执行")]
        public ActionResult PostAuditingVolAddScore(int id, int auditingState, string auditingMsg)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;
                var t_sr = db.T_VolAddScoreRecorde;

                //修改审核状态
                VolAddScoreRecorde vsr = t_sr.Find(id);
                if (vsr.AuditingState == AuditingState.审核通过)
                {
                  return  Error("已审核通过,不能在审核");
                }

                vsr.AuditingState = (AuditingState)auditingState;
                vsr.AuditingTime = DateTime.Now;
                vsr.Auditor = AdminUser.ID;
                vsr.AuditingMsg = auditingMsg;
                db.SaveChanges();

                if (vsr.AuditingState == AuditingState.审核通过)
                {
                    //审核通过,增加积分
                    Vol v = t_v.Find(vsr.VolID);
                    v.LoveBankScore = v.LoveBankScore + vsr.AddScore;
                    db.SaveChanges();
                }
                return Success("操作成功");
            }
        }



        /// <summary>
        /// 给自愿者新增积分
        /// </summary>
        /// <returns></returns>
        [SecurityNode(Name = "绑定NFC卡页面")]
        public PartialViewResult VolBindNFC(int volId)
        {
            Vol vol = null;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;


                vol = t_v.FirstOrDefault(x => x.ID == volId);
            
            }
            return PartialView(vol);
        }

        [SecurityNode(Name = "绑定NFC卡执行")]
        public ActionResult PostVolBindNFC(Vol model)
        {
            Vol vol = DbProvider.D<Vol>().FirstOrDefault(x => x.ID == model.ID);
            if (vol == null)
            {
                Error("用户不存在,请核实后从新操作");
            }
            vol.NFC = model.NFC;
            DbProvider.Update(vol);
            DbProvider.SaveChanges();

            return Success("绑定成功");
        }
    }
}