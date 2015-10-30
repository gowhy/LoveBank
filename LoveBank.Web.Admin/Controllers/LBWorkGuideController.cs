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
    [SecurityModule(Name = "工作指南")]
    public class LBWorkGuideController : BaseController
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

                var t_p = db.T_Product;

                var list = from p in t_p select p;

                //list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId,)>=0);


                return View(list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size));
            }
        }


        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
            return View();
            //using (LoveBankDBContext db = new LoveBankDBContext())
            //{
            //    var dep = db.T_Department;

            //    //部门组织
            //    //var list = DbProvider.D<Department>().Where(x => x.Level <= 6).ToList();
            //    var list = dep.Where(x => x.Level <= 6).ToList();
            //    ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list);

            //    return View();
            //}
        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(ProductModel parm)
        {
            #region 初始化参数
            Product model = new Product();

            model.AddTime = DateTime.Now;
            model.AddUserId = AdminUser.ID;
            model.State = RowState.有效;
            //model.Guid = Guid.NewGuid().ToString();
            IdWorker workId = new IdWorker();
            model.Guid = workId.nextId().ToString();
            model.DeptId = AdminUser.DeptId;

            model.Desc = parm.Desc;
            model.Name = parm.Name;
            model.CostScore = parm.CostScore;
            model.Count = parm.Count;
            model.EndTime = parm.EndTime;
            model.StartTime = parm.StartTime;
            model.Price = parm.Price;
            model.BarCode = parm.BarCode;
            model.Type = parm.Type;

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

                var t_p = db.T_Product;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;
                var t_s = db.T_SourceFile;



                var model = (from p in t_p
                             where p.ID == id
                             select new ProductModel
                             {
                                 AddTime = p.AddTime,
                                 Price = p.Price,
                                 CostScore = p.CostScore,
                                 Count = p.Count,
                                 Name = p.Name,
                                 EndTime = p.EndTime,
                                 StartTime = p.StartTime,
                                 State = p.State,
                                 Id = p.ID,
                                 Desc=p.Desc,
                                 BarCode = p.BarCode,
                                 Type = p.Type,
                                 SourceFileList = t_s.Where(x => x.Guid == p.Guid).ToList()
                             }).SingleOrDefault();

                return View(model);
            }

        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(ProductModel parm)
        {



            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_m = db.T_Product;
                var t_a = db.T_LoveBank_Ad;
                var t_s = db.T_SourceFile;


                #region 初始化参数
                Product model = t_m.Find(parm.Id);


                model.Name = parm.Name;
                model.Price = parm.Price;
                model.CostScore = parm.CostScore;
                model.Count = parm.Count;
                model.EndTime = parm.EndTime;
                model.StartTime = parm.StartTime;
                model.BarCode = parm.BarCode;
                model.Desc = parm.Desc;
                model.Type = parm.Type;

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

                db.Update<Product>(model);
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
            var ad = DbProvider.D<Product>().FirstOrDefault(x => x.ID == id);
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
            
                SourceFile res = UploadFileInstance.SaveFile(file, "ProductImg", AdminUser.ID);
                return Json(res);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine">ProductId 是必须传</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [SecurityNode(Name = "绑定机器页面")]
        public ActionResult BindMachine(MachineProductModel machine, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
              
               
                var t_mp = db.T_MachineProduct;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from m in t_m
                           join d in t_d on m.DeptId equals d.Id
                           join m in t_mp on m.ID equals m.MachineId into pm
                           let mlist = pm.Where(x => x.ProductId == machine.ProductId)
                           from m1 in mlist.DefaultIfEmpty()
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
                               ProductId = m1.ProductId,
                               Department = d

                           };


                list = list.Where(x => x.State != RowState.删除);
          

                ViewBag.ProductId = machine.ProductId;
                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        [SecurityNode(Name = "绑定机器执行")]
        public ActionResult PostBindMachine(int productId, List<int> machineIdList)
        {

            MachineProduct mp;
            List<MachineProduct> machineProductList = new List<MachineProduct>();
            foreach (var item in machineIdList)
            {
                mp = new MachineProduct();
                mp.AddTime = DateTime.Now;
                mp.AddUserId = AdminUser.ID;
                mp.ProductId = productId;
                mp.MachineId = item;

                machineProductList.Add(mp);
            }

      
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_mp = db.T_MachineProduct;

                using (TransactionScope transaction = new TransactionScope())
                {
                    ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                    var delSourceFile = from s in t_mp where s.ProductId == productId select s;
                    db.T_MachineProduct.RemoveRange(delSourceFile);
                    db.SaveChanges();

                    if (machineProductList == null)//null 标示解除了全部绑定
                    {
                        retJson.Status = true;
                        retJson.Info = "绑定成功,解除了改机器所以产品的绑定";

                        return Json(retJson);
                    }

                    db.T_MachineProduct.AddRange(machineProductList);
                    db.SaveChanges();

                    transaction.Complete();//提交事务
                    retJson.Status = true;
                    retJson.Info = "绑定成功";
                }
                retJson.Status = true;
                retJson.Info = "绑定成功";
                return Json(retJson);
            }
        }
    }
}