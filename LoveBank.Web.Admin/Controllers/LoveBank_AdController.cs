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

namespace LoveBank.Web.Admin.Controllers
{
   
    [SecurityModule(Name="一体机广告管理")]
    public class LoveBank_AdController : BaseController
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

                var ad = db.T_LoveBank_Ad;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from a in ad
                           join d in t_d on a.DeptId equals d.Id
                           join m in t_m on a.MachineCode equals m.MachineCode
                           select new LoveBank_AdModel
                           {

                               AddTime = a.AddTime,
                               MachineCode = a.MachineCode,
                               Title = a.Title,
                               ID = a.ID,
                               DeptId = a.DeptId,
                               Desc = a.Desc,
                               State = a.State,
                               Machine = m,
                               Department = d

                           };

                list = list.Where(x => x.State != RowState.删除);
                list = list.Where(x => x.Department.Id.IndexOf(AdminUser.DeptId) > -1);

                return View(list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size));
            }
        }


        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
          
            return View();
        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(LoveBank_AdModel parm)
        {
            #region 初始化参数
            LoveBank_Ad model = new LoveBank_Ad();

            model.AddTime = DateTime.Now;
            model.AddUserId = AdminUser.ID;
            model.State = RowState.有效;
            model.Guid = Guid.NewGuid().ToString();

          
            model.MachineCode = parm.MachineCode;
            model.Title = parm.Title;
            model.Desc = parm.Desc;
            foreach (var item in parm.SourceFileList)
            {
                item.Guid = model.Guid;
                item.AddTime = DateTime.Now;

            }
            #endregion

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_m = db.T_Machine;

                var mDeptId = from m in t_m where model.MachineCode == m.MachineCode select new { m.DeptId, m.ID };
                var tmpObjMachine = mDeptId.SingleOrDefault();
                if (tmpObjMachine == null)
                {
                    return Error("机器唯未授权,请先授权");
                }

                model.DeptId = tmpObjMachine.DeptId.ToString();
                model.MachineId = tmpObjMachine.ID;
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
                var t_a = db.T_LoveBank_Ad;
                var t_s = db.T_SourceFile;

                var model = (from a in t_a
                             where a.ID == id
                             select new LoveBank_AdModel
                             {
                                 AddTime = a.AddTime,
                                 MachineCode = a.MachineCode,
                                 Title = a.Title,
                                 ID = a.ID,
                                 DeptId = a.DeptId,
                                 Desc = a.Desc,
                                 State = a.State,
                                 SourceFileList = t_s.Where(x => x.Guid == a.Guid).ToList()
                             }).SingleOrDefault();

                return View(model);
            }
          
        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(LoveBank_AdModel parm)
        {

          

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_m = db.T_Machine;
                var t_a = db.T_LoveBank_Ad;
                var t_s = db.T_SourceFile;

                #region 初始化参数
                LoveBank_Ad model = t_a.Find(parm.ID);

               
                model.MachineCode = parm.MachineCode;
                model.Title = parm.Title;
                model.Desc = parm.Desc;
               

             
                ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                var delSourceFile = from s in t_s where s.Guid == model.Guid select s;
                db.T_SourceFile.RemoveRange(delSourceFile);
                db.SaveChanges();
                #endregion


                var mDeptId = from m in t_m where model.MachineCode == m.MachineCode select new { m.DeptId, m.ID };
                var tmpObjMachine = mDeptId.SingleOrDefault();
                if (tmpObjMachine == null)
                {
                    return Error("机器唯未授权,请先授权");
                }

                model.DeptId = tmpObjMachine.DeptId.ToString();
                model.MachineId = tmpObjMachine.ID;
                db.Update<LoveBank_Ad>(model);
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
            var ad = DbProvider.D<LoveBank_Ad>().FirstOrDefault(x => x.ID == id);
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

            SourceFile res = UploadFileInstance.SaveFile(file, "LoveBank_AdImg", AdminUser.ID);
            //res.ClientFileId = id;
            return Json(res);


        }
    }
}