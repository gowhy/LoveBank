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
using MySql.Data.MySqlClient;
using LoveBank.Cache;
using System.Collections;
using LoveBank.Services;
using System.Web;
using System.IO;
using System.Data;
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
                               Vol = v,
                               Department = d
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
        public ActionResult PostAdd(Vol model, string volHeadImgBase64)
        {
            if (!string.IsNullOrEmpty(volHeadImgBase64))
            {
                byte[] headerimgByte = Convert.FromBase64String(volHeadImgBase64.Substring(23));
                model.VolHeadImg = headerimgByte;
            }

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tv = db.T_Vol;

                if (tv.Count(x => x.Phone == model.Phone) > 0)
                {
                    return Success("该手机号已经存在");
                }

                model.RealNameState = 1;
                model.State = 1;
                model.Type = "志愿者";
                model.LoveBankScore = 0;
                model.Score = 0;
                model.Source = 1;//来源未1标示是爱心银行
                model.AddUserId = AdminUser.ID;


                db.Add<Vol>(model);
                db.SaveChanges();

                return Success("操作成功");
            }



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

            Vol dbEntity = DbProvider.D<Vol>().FirstOrDefault(x => x.Phone == model.Phone && x.ID != model.ID);
            if (dbEntity != null && dbEntity.Phone != null)
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
                var list = from tp in t_tp
                           where tp.ID > 0
                           select new TeamProjectModel
                           {
                               ID = tp.ID,
                               Name = tp.Name,
                               Score = tp.Score,
                               DeptId = tp.DeptId

                           };
                //list = list.Where(x => x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                volAddScoreModel.TeamProjectList = list.Where(x => x.DeptId.IndexOf(AdminUser.DeptId) > -1).ToList();
            }
            return PartialView(volAddScoreModel);
        }

        [SecurityNode(Name = "增加积分执行")]
        public ActionResult PostVolAddScore(VolAddScoreRecorde model)
        {
            Vol vol = DbProvider.D<Vol>().FirstOrDefault(x => x.ID == model.VolID);
            if (vol == null)
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
        public ActionResult AuditingAddScoreIndex(VolAddSocreRecordeSelectModel model, int? page, int? pageSize)
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

                if ((int)model.AuditingState > -1)
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
                var t_tp = db.T_TeamProject;

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
                    return Error("已审核通过,不能在审核");
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
            if (string.IsNullOrEmpty(model.NFC) || model.NFC.Length != 10)
            {
                return Error("卡号不对,请重新操作");
            }

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;

                Vol vol = t_v.FirstOrDefault(x => x.ID == model.ID);
                if (vol == null)
                {
                    return Error("用户不存在,请核实后从新操作");
                }
                if (t_v.Count(x => x.NFC == model.NFC) > 0)
                {
                    return Error("该NFC卡已经被绑定");

                }
                vol.NFC = model.NFC;
                db.Update(vol);
                db.SaveChanges();
            }
            return Success("绑定成功");
        }

        [SecurityNode(Name = "辖区自愿者分析")]
        public ActionResult VolTypeEchartsPage()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var dep = db.T_Department;

                //部门组织
                //var list = DbProvider.D<Department>().Where(x => x.Level <= 6).ToList();
                var list = dep.Where(x => x.Level <= 6 && (x.Id.IndexOf(AdminUser.DeptId)>-1)).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                return View();
            }
            //return View();

        }
        //[OutputCache(Duration=20)]
         [SecurityNode(Name = "辖区自愿者分析数据源")]
        public ActionResult VolTypeEchartsData(string deptId)
        {
            if (string.IsNullOrEmpty(deptId))
            {
                deptId = AdminUser.DeptId;
            }
            
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                string sql = string.Format(@"SELECT v.VolType  ,count(v.VolType ) value from volunteer v
                                WHERE v.DepId LIKE'{0}%'
                                GROUP BY  v.VolType ", deptId);


                string cacheKey = "VolTypeEchartsData" + deptId;
                object listCache = BaseCacheManage.RetrieveObject(cacheKey);
                if (listCache!=null)
                {
                    return Json((VolTypeEchartsDataModel)listCache, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MySqlParameter[] parm = new MySqlParameter[] { };
                    List<VolTypeEchartsData> list = db.Database.SqlQuery<VolTypeEchartsData>(sql, parm).ToList();

                    decimal total = list.Sum(x => long.Parse(x.value));
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Oname = list[i].VolType.ToLocalizable();
                        list[i].name = list[i].VolType.ToLocalizable() + "(" + (decimal.Parse(list[i].value) / total).ToString("p") + ")";
                    }

                    VolTypeEchartsDataModel model = new VolTypeEchartsDataModel();
                    BaseCacheManage.AddObject(cacheKey, model, 10 * 60);

                    model.DepName = (from d in db.T_Department where d.Id == deptId select d.Name).FirstOrDefault();
                    model.VolTypeEchartsDataList = list;
                    model.Total = total.ToString();

                   
                    return Json(model);
                }

            }

        }

        [SecurityNode(Name = "辖区终端模块点击分析")]
        public ActionResult MachineModuleStatisticsPage()
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var dep = db.T_Department;

                //部门组织
                //var list = DbProvider.D<Department>().Where(x => x.Level <= 6).ToList();
                var list = dep.Where(x => x.Level <= 6 && (x.Id.IndexOf(AdminUser.DeptId) > -1)).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                return View();
            }

        }

       [SecurityNode(Name = "辖区终端模块点击分析数据源")]
        public ActionResult MachineModuleStatisticsData(string deptId)
        {
            if (string.IsNullOrEmpty(deptId))
            {
                deptId = AdminUser.DeptId;
            }
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                string sql = string.Format(@"SELECT  mm.`name` ,COUNT( mm.`name`) value  FROM machinestatistics  ms
                                                LEFT JOIN  machine  m  ON  ms.`MachineCode`=m.`MachineCode`
                                                LEFT JOIN  machinemoduleshowmanage mm ON mm.`ID`=ms.`ModuleId`
                                                WHERE mm.`Name` IS NOT NULL  and ms.`DeptId` LIKE '{0}%' 
                                                GROUP BY  mm.`name`  ", deptId);


                string cacheKey = "MachineModuleStatisticsData" + deptId;
                object listCache = BaseCacheManage.RetrieveObject(cacheKey);
                if (listCache != null)
                {
                    return Json((VolTypeEchartsDataModel)listCache, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MySqlParameter[] parm = new MySqlParameter[] { };
                    List<VolTypeEchartsData> list = db.Database.SqlQuery<VolTypeEchartsData>(sql, parm).ToList();

                    decimal total = list.Sum(x => long.Parse(x.value));
                    for (int i = 0; i < list.Count; i++)
                    {

                        list[i].name = list[i].name + "(" + (decimal.Parse(list[i].value) / total).ToString("p") + ")";
                    }

                    VolTypeEchartsDataModel model = new VolTypeEchartsDataModel();
                    BaseCacheManage.AddObject(cacheKey, model, 10 * 60);

                    model.DepName = (from d in db.T_Department where d.Id == deptId select d.Name).FirstOrDefault();
                    model.VolTypeEchartsDataList = list;
                    model.Total = total.ToString();
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

            }

        }


        [SecurityNode(Name = "机器运行活跃大数据页面")]
        public ActionResult MachineModuleLargeEffectsPage()
        {

            return View();

        }
        [SecurityNode(Name = "机器运行活跃大数据数据源")]
        public ActionResult MachineModuleLargeEffectsData(string deptId)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

              
//                string sql = string.Format(@"SELECT  m.`Lat`,m.`Lon`,d.name FROM machine m
//                                                                            INNER JOIN department d ON d.`Id`=m.`DeptId`
//                                               WHERE m.`DeptId` LIKE '{0}%'
//                                               ", AdminUser.DeptId);

                string sql = string.Format(@" 
 
                                        SELECT  m.`Lat`,m.`Lon`, m.`MachineCode`,d.name,m.`Address`,m.`DeptId`,m.`ID`
                                                         ,(SELECT MAX(id) FROM machineheartbeat  mhb 
                                                        WHERE   m.MachineCode=mhb.MachineCode AND    mhb.`AddTime` > DATE_ADD(NOW(), INTERVAL -200
                                                         MINUTE)) AS State
 
                                                    FROM machine m
                                                    LEFT JOIN department d ON  m.`DeptId`=d.`Id`
                                                    WHERE m.DeptId LIKE'{0}%'  "
                                , AdminUser.DeptId);

                string cacheKey = "MachineModuleLargeEffectsData_11" + AdminUser.DeptId;
                object listCache = BaseCacheManage.RetrieveObject(cacheKey);
                if (listCache!=null)
                {
                    return Json((LargeEffectsDataModel)listCache, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MySqlParameter[] parm = new MySqlParameter[] { };
                    List<LargeEffectsDataModel> list = db.Database.SqlQuery<LargeEffectsDataModel>(sql, parm).ToList();


                    LargeEffectsDataModelPage tmpPage = new LargeEffectsDataModelPage();

                    foreach (var item in list)
                    {
                        if (item.State.HasValue)
                        {
                            tmpPage.RunList.Add(item);
                        }
                        else
                        {
                            tmpPage.NoRunList.Add(item);
                        }
                    }

                    #region 以前的方式废弃
                    //List<LargeEffectsDataModelPage> listPage = new List<LargeEffectsDataModelPage>();

                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    //sb.Append("[");

                    //foreach (var item in list)
                    //{

                    //    //tmpPage = new LargeEffectsDataModelPage();
                    //    //tmpPage.name = item.name;
                    //    sb.Append("{name:'" + item.name.Trim() + "',geoCoord:[" + item.Lon.Trim() + "," + item.Lat.Trim() + "]},");

                    //}
                    //sb.Append("{name:'11',geoCoord:[01, 10]}");
                    //sb.Append("]");

                    //return Json(sb.ToString(), "application/Json", System.Text.Encoding.UTF8);
                    #endregion

                    return Json(tmpPage);
                }

            }


        }
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Error("请选择文件");
            }

            SourceFile res = UploadFileInstance.SaveFile(file, "LoveBankVolImg", AdminUser.ID);
            return Json(res);

        }

        #region 志愿者批量导入
        public ActionResult AddVolExcel()
        {

            return PartialView();

        }

        public ActionResult PostAddVolExcel()
        {
            HttpPostedFileBase Volfile = Request.Files["volInfoExcel"];

            FileInfo file2 = new FileInfo(Volfile.FileName);
            string FileName = Server.MapPath("..//Tmp") + "//" + AdminUser.ID + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file2.Extension;

            //string FileName = @"C:\工作项目资料\爱心银行项目\志愿者导入模板.xls";
            FileHelper.Upload(Volfile, FileName);

            ExcelHelper eh = new ExcelHelper(FileName, "");
            DataTable dt = eh.InputFromExcel();

            List<Vol> volList = new List<Vol>();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Vol tmp = new Vol();
                    tmp.Phone = dt.Rows[i]["手机号"].ToString();


                    if (db.T_Vol.Count(x => x.Phone == tmp.Phone) > 0)
                    {
                        return Error("批量导入失败，手机号是：" + tmp.Phone + "、姓名是" + tmp.RealName + "的自愿者已经存在");
                    }
                    tmp.RealName = dt.Rows[i]["姓名"].ToString();
                    tmp.VolType = (VolType)Enum.Parse(typeof(VolType), dt.Rows[i]["志愿者类型"].ToString());
                    tmp.Sex = dt.Rows[i]["性别"].ToString();
                    tmp.State = 1;
                    tmp.Score = 50;
                    tmp.ThsScore = 20;
                    tmp.LoveBankScore = 0;
                    tmp.Type = "志愿者";
                    tmp.PassWord = "111111";
                    tmp.DepId = AdminUser.DeptId;
                    tmp.VID = tmp.Phone;
                    tmp.Source = 1;
                    

                    if (volList.Count(x => x.Phone == tmp.Phone) > 0)
                    {
                        return Error("批量导入失败，Excel表格中手机号重复：" + tmp.Phone + "、姓名是" + tmp.RealName + "");

                    }
                    volList.Add(tmp);
                }

                db.T_Vol.AddRange(volList);
                db.SaveChanges();
            }

            System.IO.File.Delete(FileName);
            return Success("批量导入成功");
        }
        #endregion
    }
}