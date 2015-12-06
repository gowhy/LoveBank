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
using System.Transactions;
using MySql.Data.MySqlClient;

namespace LoveBank.Web.Admin.Controllers
{
     [SecurityModule(Name = "一体机管理")]
    public class MachineController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
         const int PageSize = 20;


         [SecurityNode(Name = "首页")]
         public ActionResult Index(MachineModel machine, int? page, int? pageSize)
         {
             var pageNumber = page ?? 1;
             var size = pageSize ?? PageSize;

             using (LoveBankDBContext db = new LoveBankDBContext())
             {

                 var t_m = db.T_Machine;
                 var t_d = db.T_Department;

                 //var list = from m in mac select m;


                 var list = from m in t_m
                            join d in t_d on m.DeptId equals d.Id

                            select new MachineModel
                            {
                                Address = m.Address,
                                AddTime = m.AddTime,
                                AddUserDeptId = m.AddUserDeptId,
                                DeptId = m.DeptId,
                                Desc = m.Desc,
                                Id = m.ID,
                                MachineCode = m.MachineCode,
                                Name = m.Name,
                                State = m.State,
                                Title = m.Title,
                                Lat = m.Lat,
                                Lon = m.Lon,
                                Department = d

                            };


                 list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                 if (!string.IsNullOrEmpty(machine.Name)) list = list.Where(x => x.Name.Contains(machine.Name));
                 if (!string.IsNullOrEmpty(machine.MachineCode)) list = list.Where(x => x.MachineCode == machine.MachineCode);
                 if (!string.IsNullOrEmpty(machine.DeptId)) list = list.Where(x => x.DeptId.IndexOf(machine.DeptId) > -1);

                 machine.MachineModelList = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);

                 var list2 = t_d.Where(x => x.Level <= 6 && x.Id.IndexOf(AdminUser.DeptId) > -1).ToList();
                 //if (!string.IsNullOrEmpty(machine.DeptId))
                 //{
                 //    list2.FirstOrDefault(x => x.Id == machine.DeptId).IsCheck = true;
                    
                 //}


                 ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list2);
                 return View(machine);
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
        public ActionResult PostAdd(Machine machine)
        {
            Machine tem = DbProvider.D<Machine>().FirstOrDefault(x => x.MachineCode == machine.MachineCode&&x.State!=RowState.删除);
            if (machine == null) return Error("机器编码已经存在,不能再新增.");

            machine.AddUserId = AdminUser.ID;
            machine.AddUserDeptId = AdminUser.DeptId;
            machine.AddTime = DateTime.Now;

          
            DbProvider.Add(machine);
            DbProvider.SaveChanges();
            return Success("操作成功");

        }

 
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {

            Machine machine = DbProvider.D<Machine>().FirstOrDefault(x => x.ID == id);
            if (machine == null) return Error("机器不存在");
            machine.State = RowState.删除;
            DbProvider.SaveChanges();
            return Success("操作成功");

        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            var user = DbProvider.D<Machine>().FirstOrDefault(x => x.ID == id);
            IList<Role> role = DbProvider.D<Role>().ToList();

            ViewData["UserRole"] = role;
            using (LoveBankDBContext db= new LoveBankDBContext())
            {
                //部门组织
                var list = db.T_Department.Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

                user.Department = list.FirstOrDefault(x => x.Id == user.DeptId);
                return View(user);
            }
          
        }



        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(Machine model)
        {

            Machine machine = DbProvider.D<Machine>().FirstOrDefault(x => x.ID == model.ID);

            if (machine == null) return Error("机器不存在");

            machine.DeptId = model.DeptId;
            machine.MachineCode = model.MachineCode;
            machine.Name = model.Name;
            machine.Title = model.Title;
            machine.Desc = model.Desc;
            machine.Address = model.Address;

            DbProvider.SaveChanges();

            return Success("操作成功");

        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="machine"> MachineId 是其中的必传参数</param>
         /// <param name="page"></param>
         /// <param name="pageSize"></param>
         /// <returns></returns>
        [SecurityNode(Name = "绑定机器页面")]
        public ActionResult BindProduct(MachineProductModel machine, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_d = db.T_Department;
                var t_p = db.T_Product; 
                var t_m = db.T_MachineProduct;


                var list = from p in t_p
                           join d in t_d on p.DeptId equals d.Id
                           join m in t_m on p.ID equals m.ProductId into pm
                           let mlist = pm.Where(x => x.MachineId == machine.MachineId)
                           from m1 in mlist.DefaultIfEmpty()
                           select new ProductModel
                           {
                               BarCode = p.BarCode,
                               CostScore = p.CostScore,
                               Count = p.Count,
                               DeptId = p.DeptId,
                               Id = p.ID,
                               EndTime = p.EndTime,
                               MachineId = m1.MachineId,
                               Name = p.Name,
                               Price = p.Price,
                               StartTime = p.StartTime,
                               State = p.State,
                               Type = p.Type,
                               AddUserId=p.AddUserId,
                               DeptIdName=d.Name
                           
                           };


                list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                if (!string.IsNullOrEmpty(machine.BarCode)) list = list.Where(x => x.BarCode == machine.BarCode);
                if (!string.IsNullOrEmpty(machine.ProductName)) list = list.Where(x => x.Name.Contains(machine.ProductName));
                //list = list.Where(x => x.AddUserId==AdminUser.ID);//限制只能绑定自己新增的产品

                ViewBag.MachineId = machine.MachineId;

                //return PartialView(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        [SecurityNode(Name = "绑定机器执行")]

         public ActionResult PostBindMachine(int machineId, List<int> productIdList, List<int> productIdInitList)
        {


            MachineProduct mp;
            List<MachineProduct> machineProductList = new List<MachineProduct>();


            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_mp = db.T_MachineProduct;
                var t_d = db.T_Department;
                var t_p = db.T_Product;
                var t_m = db.T_MachineProduct;

                ///和初始的机器列表对比，找出被删除的机器Id
                List<int> delli = null;
                if (productIdInitList != null)
                {
                    delli = productIdInitList.FindAll(x => productIdList == null || !productIdList.Contains(x));
                }
                //查找出新增的
                List<int> addli = null;
                if (productIdList != null)
                {
                    //查找出新增的
                    addli = productIdList.FindAll(x => productIdInitList == null || !productIdInitList.Contains(x));
                }


                if (productIdList != null && addli != null)
                {
                    foreach (var item in addli)
                    {
                        mp = new MachineProduct();
                        mp.AddTime = DateTime.Now;
                        mp.AddUserId = AdminUser.ID;
                        mp.ProductId = item;
                        mp.MachineId = machineId;

                        machineProductList.Add(mp);
                    }
                }



                using (TransactionScope transaction = new TransactionScope())
                {
                    ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                    if (delli != null && delli.Count > 0)
                    {   //var delSourceFile = from s in t_mp where s.MachineId == machineId select s;
                        var delSourceFile = from s in t_mp where delli.Contains(s.ProductId) && s.MachineId == machineId select s;
                        db.T_MachineProduct.RemoveRange(delSourceFile);
                        db.SaveChanges();
                    }


                    if (machineProductList == null)//null 标示解除了全部绑定
                    {
                        retJson.Status = true;
                        retJson.Info = "绑定成功,解除了改机器所以产品的绑定";

                        return Json(retJson);
                    }

                    db.T_MachineProduct.AddRange(machineProductList);
                    db.SaveChanges();
                    transaction.Complete();
                    retJson.Status = true;
                    retJson.Info = "绑定成功";
                }
                return Json(retJson);
            }
        }


        [SecurityNode(Name = "一体机运行状况")]
        public ActionResult MachineRunState(string deptId,int page=1,int size=50)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                string sql = string.Format(@" 
 
                                        SELECT  m.`MachineCode`,m.`Name`,m.`Address`,m.`DeptId`,d.`Name` AS DeptIdName,m.`ID`
                                                         ,(SELECT MAX(id) FROM machineheartbeat  mhb 
                                                        WHERE   m.MachineCode=mhb.MachineCode AND    mhb.`AddTime` > DATE_ADD(NOW(), INTERVAL -200
                                                         MINUTE)) AS State
 
                                                    FROM machine m
                                                    LEFT JOIN department d ON  m.`DeptId`=d.`Id`
                                                    WHERE m.DeptId LIKE'{0}%'  "
                                   , AdminUser.DeptId);


                string cacheKey = "MachineRunState" + AdminUser.DeptId;
                object listCache = BaseCacheManage.RetrieveObject(cacheKey);
                if (listCache != null)
                {
                    return View((IPagedList<LoveBank.Web.Admin.Models.MachineRunStateModel>)listCache);
                }
                else
                {
                    MySqlParameter[] parm = new MySqlParameter[] { };
                    var list = db.Database.SqlQuery<MachineRunStateModel>(sql, parm).OrderByDescending(x => x.Id).ToPagedList(page - 1, size);

                    BaseCacheManage.AddObject(cacheKey, list);
                    return View(list);
                }

                return View();
            }

        }
    }
}