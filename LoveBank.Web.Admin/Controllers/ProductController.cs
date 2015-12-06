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
    [SecurityModule(Name = "商品管理")]
    public class ProductController : BaseController
    {
        // GET: Product
        /// <summary>
        /// 每页条数
        /// </summary>
        const int PageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(ProductSelectModel model, int? page, int? pageSize)
        {

            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_Product;
                var t_d = db.T_Department;

                //var list2 = from p in t_p select p;
                var list = from p in t_p
                           join d in t_d on p.DeptId equals d.Id
                           select new ProductModel
                           {
                               Id = p.ID,
                               Name = p.Name,
                               DeptId = p.DeptId,
                               Count = p.Count,
                               Price = p.Price,
                               BarCode = p.BarCode,
                               CostScore = p.CostScore,
                               EndTime = p.EndTime,
                               StartTime = p.StartTime,
                               State = p.State,
                               DeptIdName = d.Name,
                               AddUserId = p.AddUserId,
                               IsOwn = p.AddUserId == AdminUser.ID ? true : false

                           };

                list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);

                if (!string.IsNullOrEmpty(model.Name)) list = list.Where(x => x.Name.Contains(model.Name));
                if (!string.IsNullOrEmpty(model.BarCode)) list = list.Where(x => x.BarCode == model.BarCode);

                model.ProductList = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);
                return View(model);
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
            model.Sponsors = parm.Sponsors;
            foreach (var item in parm.SourceFileList)
            {
                item.Guid = model.Guid;
                item.AddTime = DateTime.Now;

            }

            model.LogoGuid = Guid.NewGuid().ToString();
            foreach (var item in parm.SourceFileListLogo)
            {
                item.Guid = model.LogoGuid;
                item.AddTime = DateTime.Now;

            }
            model.AdGuid = Guid.NewGuid().ToString();
            foreach (var item in parm.SourceFileListAd)
            {
                item.Guid = model.AdGuid;
                item.AddTime = DateTime.Now;

            }
            #endregion

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                db.Add(model);
                db.SaveChanges();
                db.T_SourceFile.AddRange(parm.SourceFileList);
                db.T_SourceFile.AddRange(parm.SourceFileListLogo);
                db.T_SourceFile.AddRange(parm.SourceFileListAd);
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
                                 Desc = p.Desc,
                                 BarCode = p.BarCode,
                                 Type = p.Type,
                                 AddUserId=p.AddUserId,
                                 Sponsors = p.Sponsors,
                                 SourceFileList = t_s.Where(x => x.Guid == p.Guid && !string.IsNullOrEmpty(p.Guid)).ToList(),
                                 SourceFileListLogo = t_s.Where(x => x.Guid == p.LogoGuid && !string.IsNullOrEmpty(p.LogoGuid)).ToList(),
                                 SourceFileListAd = t_s.Where(x => x.Guid == p.AdGuid && !string.IsNullOrEmpty(p.AdGuid)).ToList()
                             }).SingleOrDefault();

                if (model.AddUserId!=AdminUser.ID)
                {
                    return Error("无权限操作只能编辑自己新增的数据");
                }
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
                model.Sponsors = parm.Sponsors;
                //foreach (var item in parm.SourceFileList)
                //{
                //    item.Guid = model.Guid;
                //    item.AddTime = DateTime.Now;

                //}
                ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                var delSourceFile = from s in t_s
                                    where (!string.IsNullOrEmpty(model.Guid) && s.Guid == model.Guid)
                                        || (!string.IsNullOrEmpty(model.LogoGuid) && s.Guid == model.LogoGuid) ||
                                        (!string.IsNullOrEmpty(model.AdGuid) && s.Guid == model.AdGuid)
                                    select s;
                db.T_SourceFile.RemoveRange(delSourceFile);
                db.SaveChanges();
                #endregion

                db.Update<Product>(model);
                db.SaveChanges();

                foreach (var item in parm.SourceFileList)
                {
                    if (string.IsNullOrEmpty(model.Guid))
                    {
                        model.Guid = Guid.NewGuid().ToString();

                    }
                    item.AddTime = DateTime.Now;
                    item.Guid = model.Guid;

                }


                foreach (var item in parm.SourceFileListLogo)
                {
                    if (string.IsNullOrEmpty(model.LogoGuid))
                    {
                        model.LogoGuid = Guid.NewGuid().ToString();

                    }
                    item.AddTime = DateTime.Now;
                    item.Guid = model.LogoGuid;


                }

                foreach (var item in parm.SourceFileListAd)
                {
                    if (string.IsNullOrEmpty(model.AdGuid))
                    {
                        model.AdGuid = Guid.NewGuid().ToString();

                    }
                    item.AddTime = DateTime.Now;
                    item.Guid = model.AdGuid;


                }
                db.T_SourceFile.AddRange(parm.SourceFileList);//重新绑定

                if (parm.SourceFileListLogo != null &&parm.SourceFileListLogo.Count > 0)
                {
                    db.T_SourceFile.AddRange(parm.SourceFileListLogo);
                }

                if (parm.SourceFileListAd!=null&&parm.SourceFileListAd.Count>0)
                {
                    db.T_SourceFile.AddRange(parm.SourceFileListAd);
                }
              
                //db.T_SourceFile.AddRange(parm.SourceFileList);


                db.SaveChanges();

                return Success("修改成功");

            }

        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {

            var ad = DbProvider.D<Product>().FirstOrDefault(x => x.ID == id);

            if (ad.AddUserId != AdminUser.ID)
            {
                MsgModel msgModel = new MsgModel();
                msgModel.Message = "无权限操作只能删除自己新增的数据";
                msgModel.WaitSecond = -1;
                msgModel.Title = "权限不足";
                return Message(msgModel);
            }

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
        public ActionResult BindMachine(MachineModel machine, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {


                var t_mp = db.T_MachineProduct;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;
                var t_p = db.T_Product;

                if (t_p.Count(x=>x.AddUserId==AdminUser.ID&&x.ID== machine.ProductId)==0)
                {
                    MsgModel msgModel = new MsgModel();
                    msgModel.Message = "无权限操作，只能绑定自己新增的产品";
                    msgModel.WaitSecond =- 1;
                    msgModel.Title = "绑定机器无权操作";
                    return Message(msgModel);
                }

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


                list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);
                if (!string.IsNullOrEmpty(machine.Name)) list = list.Where(x => x.Name.Contains(machine.Name));
                if (!string.IsNullOrEmpty(machine.MachineCode)) list = list.Where(x => x.MachineCode == machine.MachineCode);
                if (!string.IsNullOrEmpty(machine.DeptId)) list = list.Where(x => x.DeptId.IndexOf(machine.DeptId) > -1);


                ViewBag.ProductId = machine.ProductId;
                machine.MachineModelList = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);

                var list2 = t_d.Where(x => x.Level <= 6 && x.Id.IndexOf(AdminUser.DeptId) > -1).ToList();
                if (!string.IsNullOrEmpty(machine.DeptId))
                {
                    list2.FirstOrDefault(x => x.Id == machine.DeptId).IsCheck = true;
                    //list2[list2.FindIndex(x => x.Id == machine.DeptId)].IsCheck = true;
                }


                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<Department>>(list2);

                return View(machine);
            }
        }
        [SecurityNode(Name = "绑定机器执行")]
        public ActionResult PostBindMachine(int productId, List<int> machineIdList, List<int> machineIdInitList)
        {


            MachineProduct mp;
            List<MachineProduct> machineProductList = new List<MachineProduct>();


            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_mp = db.T_MachineProduct;
                var t_m = db.T_Machine;


          
                ///和初始的机器列表对比，找出被删除的机器Id
                List<int> delli = null;
                if (machineIdInitList!=null)
                {
                    delli = machineIdInitList.FindAll(x => !machineIdList.Contains(x));

                }
                ////查找出新增的
                //List<int> addli = machineIdList.FindAll(x => machineIdInitList == null || !machineIdInitList.Contains(x));

                //查找出新增的
                List<int> addli = null;
                if (machineIdList != null)
                {
                    //查找出新增的
                    addli = machineIdList.FindAll(x => machineIdInitList == null || !machineIdInitList.Contains(x));
                }

                if (machineIdList != null && addli != null)
                {
                    foreach (var item in addli)
                    {
                        mp = new MachineProduct();
                        mp.AddTime = DateTime.Now;
                        mp.AddUserId = AdminUser.ID;
                        mp.ProductId = productId;
                        mp.MachineId = item;

                        machineProductList.Add(mp);
                    }
                }



                using (TransactionScope transaction = new TransactionScope())
                {
                    ///删除原来的,彻底以新增方式进行（修改通过删除在新增实现）
                    if (delli!=null&&delli.Count>0)
                    {
                        var delSourceFile = from s in t_mp where s.MachineId.HasValue && delli.Contains(s.MachineId.Value) && s.ProductId == productId select s;
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