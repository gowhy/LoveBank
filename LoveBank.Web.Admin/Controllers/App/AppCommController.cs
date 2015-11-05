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
using System.IO;
using LoveBank.Cache;
using System.Web.Security;

namespace LoveBank.Web.Admin.Controllers.App
{
    public class AppCommController : AppBaseController
    {
        /// <summary>
        /// 返回社区基础数据接口
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_DepartmentList(int page = 1, int pageSize = 20000)
        {
            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var dep = db.T_Department;
                var list = dep.Where(x => x.Level <= 6).ToList();

                var list2 = from d in dep
                            where d.Level <= 6
                            select new Department
                            {
                                Name = d.Name,
                                Level = d.Level,
                                Id = d.Id,
                                PId = d.PId,
                                Lat = d.Lat,
                                Lng = d.Lng
                            };

                retJson.Status = true;
                retJson.Data = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize).ToList();
                return Json(retJson);
            }

        }

        /// <summary>
        /// 机器注册接口
        /// </summary>
        /// <param name="model"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ActionResult App_MachineReg(Machine model, string phone)
        {
            model.AddTime = DateTime.Now;
            model.State = RowState.有效;
            model.AddUserId = -1;

            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_a = db.T_AdminUser;
                var t_m = db.T_Machine;

                if (t_m.Count(x => x.MachineCode == model.MachineCode) > 0)
                {
                    retJson.Status = false;
                    retJson.Info = "该设备已经注册";
                    return Json(retJson);
                }

                if (t_a.Count(x => x.Phone == phone) == 0)
                {
                    retJson.Status = false;
                    retJson.Info = "手机号不存在";
                    return Json(retJson);
                }

                db.Add<Machine>(model);
                db.SaveChanges();
                retJson.Status = true;
                retJson.Info = "机器注册成功";
                return Json(retJson);
            }

        }

        /// <summary>
        /// App登录，返回用户信息和认证Cookie
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public ActionResult App_Login(string phone, string passWord, string machineCode)
        {

            JsonMessage returnJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_a = db.T_Vol;
                //string passWordHash = passWord.Hash();
                Vol appUser = t_a.Where(x => x.Phone == phone && x.PassWord == passWord).FirstOrDefault();
                if (appUser != null)
                {
                    returnJson.Status = true;
                    appUser.PassWord = string.Empty;
                    returnJson.Data =
                        new
                        {
                            Ticket = AutheTicketManager.CreateAppLoginUserTicket(appUser.ID.ToString()),
                            User = appUser
                        };
                    returnJson.Info = "登录成功";

                    //登陆log
                    LoginStatistics loginStatistics = new LoginStatistics();
                    loginStatistics.Type = LoginType.手机;
                    loginStatistics.LoginState = 1;
                    loginStatistics.Phone = phone;
                    loginStatistics.MachineCode = machineCode;
                    loginStatistics.UserId = appUser.ID;

                    Action<LoginStatistics> loginStatisticsTarget = s => LoginLog(s);
                    loginStatisticsTarget(loginStatistics);


                    return Json(returnJson);
                }
                else
                {
                    returnJson.Status = false;
                    returnJson.Info = "登录失败,手机号或者密码错误";
                    returnJson.Data = HttpContext.Error;
                    return Json(returnJson);

                }

            }
            returnJson.Status = false;
            returnJson.Info = "登录失败,系统异常";
            returnJson.Data = HttpContext.Error;
            return Json(returnJson);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineId">
        /// 必须参数：MachineCode（机器的Id）
        /// ProductName:产品名称，可以模糊搜索
        /// BarCode:条形码
        /// </param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_GetProduct(MachineProductModel machine, int page = 1, int pageSize = 20)
        {
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_Product;
                var t_mp = db.T_MachineProduct;
                var t_m = db.T_Machine;
                var t_s = db.T_SourceFile;

                var list = from p in t_p
                           join m1 in t_mp on p.ID equals m1.ProductId
                           join m2 in t_m on m1.MachineId equals m2.ID
                           where m2.MachineCode == machine.MachineCode
                           && p.Count > 0
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
                               Desc=p.Desc,
                               //SourceFileList = t_s.Where(x => x.Guid == p.Guid).ToList()
                               AppSourceFileList = (from s in t_s where s.Guid == p.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()
                           };


                list = list.Where(x => x.State != RowState.删除);
                if (!string.IsNullOrEmpty(machine.BarCode)) list = list.Where(x => x.BarCode == machine.BarCode);
                if (!string.IsNullOrEmpty(machine.ProductName)) list = list.Where(x => x.Name.Contains(machine.ProductName));


                retJson.Status = true;
                retJson.Data = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize).ToList();
                return Json(retJson);
            }
        }

        /// <summary>
        /// App获取基础模块,根据机器所在社区Id
        /// 参数：deptId：社区Id
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_GetMachineModule(string deptId, int page = 1, int pageSize = 50)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var sf = db.T_SourceFile;
                var ad = db.T_MachineModuleShowManage;
                var t_m = db.T_Machine;
                var t_d = db.T_Department;

                var list = from a in ad
                           join d in t_d on a.DeptId equals d.Id
                           join s in sf on a.Guid equals s.Guid
                           where d.Id == deptId
                           select new MachineModuleShowManageModel
                           {
                               LinkUrl = a.LinkUrl,
                               ModuleKey = a.ModuleKey,
                               Name = a.Name,
                               //IconUrl = (from s in sf where s.Guid == a.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).FirstOrDefault().ImgHttpUrl,
                               IconUrl = s.Domain + s.Path,
                               Sort = a.Sort,
                               //Guid = a.Guid,
                               Type = a.Type,
                               AddTime = a.AddTime,
                               ID = a.ID,
                               DeptId = a.DeptId,
                               Department = d,
                               State = a.State,
                               Icon = a.Icon 
                           };

                list = list.Where(x => x.State != RowState.删除);

                JsonMessage retJson = new JsonMessage();
                retJson.Data = list.OrderBy(x => x.Sort).ToPagedList(page - 1, pageSize).ToList();
                retJson.Status = true;
                return Json(retJson);
            }
        }

        /// <summary>
        ///  获取App的首页顶端广告图
        ///  machineCode：机器的唯一码
        /// </summary>
        /// <returns></returns>

        [OutputCache(Duration = 10)]
        public ActionResult StartAdList(string machineCode)
        {
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var ad = db.T_LoveBank_Ad;
                var sf = db.T_SourceFile;

                var list = from a in ad
                           where a.State != RowState.删除
                           where a.MachineCode == machineCode.Trim()
                           select new LoveBank_AdModel
                           {
                               LinkUrl = a.LinkUrl,
                               Title = a.Title,
                               AppSourceFileList = (from s in sf where s.Guid == a.Guid select new { ImgHttpUrl = s.Domain + s.Path }).ToList()
                           };
                retJson.Data = list.ToList();
                retJson.Status = true;
                return Json(retJson);
            }

        }

        public ActionResult App_GetMachineConfig(string machineCode)
        {


            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_a = db.T_AdminUser;
                var t_m = db.T_Machine;

                Machine model = t_m.SingleOrDefault(x => x.MachineCode == machineCode.Trim());
                if (model == null)
                {
                    retJson.Status = false;
                    retJson.Info = "机器未注册";
                    return Json(retJson);
                }
                retJson.Status = true;
                retJson.Info = "机器注册成功";
                retJson.Data = model;
                return Json(retJson);
            }

        }

        /// <summary>
        /// 获取项目基本信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_TeamProjectList(int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_TeamProject;
                var list = from p in t_p select p;

                list = list.Where(x => x.State != RowState.删除);

                retJson.Data = list.OrderByDescending(x => x.ID).ToPagedList(page - 1, pageSize).ToList();
                retJson.Status = true;
                return Json(retJson);
            }
        }
        #region 该段代码暂时未使用
        ///// <summary>
        ///// MachineCode（必填）:机器为一码
        ///// Type（必填）:   登陆 = 0,  兑换 = 1
        ///// Data：客户端需要使用的其他数据，可以穿json数据，不用作服务端运算,在跨设备时之间进行中间数据传输
        ///// GrantLon：经度
        ///// GrantLat：纬度
        ///// GrantAddress：授权地址
        ///// 
        ///// </summary>
        ///// <param name="qrCodeModel"></param>
        ///// <returns></returns>
        //public ActionResult App_QRCode(QRCodeModel qrCodeModel)//该方法和App_DeCoderQRCode是一对配合使用
        //{
        //    JsonMessage retJson = new JsonMessage();
        //    string encoderStringKey = qrCodeModel.MachineCode + DateTime.Now.Ticks;//生成唯一key


        //    MemoryStream ms = new MemoryStream();
        //    QRCodeHelper.GetQRCode(encoderStringKey, ms);
        //    byte[] _ImageBytes = ms.ToArray();

        //    //QRCodeModel qrCodeModel = new QRCodeModel();

        //    qrCodeModel.AddTime = DateTime.Now;
        //    qrCodeModel.MachineCode = qrCodeModel.MachineCode;
        //    BaseCacheManage.AddObject(encoderStringKey, qrCodeModel, 60 * 3);//加入缓存


        //    retJson.Status = true;
        //    retJson.Data = new
        //    {
        //        MachineCode = qrCodeModel.MachineCode,
        //        QRCodeStringKey = encoderStringKey,
        //        QRCodeImg = "data:image/jpg;base64," + Convert.ToBase64String(_ImageBytes)
        //    };
        //    //ViewBag.img = Convert.ToBase64String(_ImageBytes);
        //    //return View();
        //    return Json(retJson,JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult App_DeCoderQRCode(string QRCodeStringKey, int appUserId, string obj)//该方法和App_QRCode是一对配合使用
        //{
        //    QRCodeModel qrCodeModel = (QRCodeModel)BaseCacheManage.RetrieveObject(QRCodeStringKey);//获取缓存对象

        //    JsonMessage retJson = new JsonMessage();
        //    if (qrCodeModel == null)
        //    {
        //        retJson.Status = false;
        //        retJson.Info = "已失效";

        //        return Json(retJson);
        //    }
        //    else
        //    {
        //        if (qrCodeModel.State != QRCodeState.待授权)
        //        {
        //            retJson.Status = false;
        //            retJson.Info = "已失效";
        //            return Json(retJson);
        //        }

        //        switch (qrCodeModel.Type)
        //        {
        //            case QRCodeType.登陆:
        //                retJson.Status = QRCodeLogin(appUserId, obj);///登录时obj参数是App存储在本科的票据Ticket
        //                retJson.Data = obj;
        //                if (retJson.Status == true)
        //                {
        //                    retJson.Info = "已经授权成功";
        //                    qrCodeModel.State = QRCodeState.已授权;
        //                    qrCodeModel.UserId = appUserId;
        //                    qrCodeModel.GrantTime = DateTime.Now;
        //                    BaseCacheManage.AddObject(QRCodeStringKey, qrCodeModel, 60 * 1);//加入缓存
        //                    //BaseCacheManage.RemoveObject(QRCodeStringKey);//授权成功清除缓存对象
        //                }
        //                return Json(retJson);
        //            case QRCodeType.兑换:
        //                return Json(retJson);

        //            default:
        //                retJson.Status = false;
        //                retJson.Info = "未知类型(Type)";
        //                return Json(retJson);

        //        }
        //    }

        //}
        #endregion

        public ActionResult App_LoginQRCode(QRCodeModel qrCodeModel)//该方法和App_QRCode是一对配合使用
        {

            JsonMessage retJson = new JsonMessage();
         
            switch (qrCodeModel.Type)
            {
                case QRCodeType.登陆:
                    retJson.Status = QRCodeLogin(qrCodeModel.UserId, qrCodeModel.Ticket);///登录时obj参数是App存储在本科的票据Ticket
                    retJson.Data = qrCodeModel.Data;
                    if (retJson.Status == true)
                    {
                        //登陆log
                        LoginStatistics loginStatistics = new LoginStatistics();
                        loginStatistics.Type = LoginType.二维码登陆;
                        loginStatistics.LoginState = 1;
                        //LoginLog(loginStatistics);
                        Action<LoginStatistics> loginStatisticsTarget = s => LoginLog(s);
                        loginStatisticsTarget(loginStatistics);

                        retJson.Info = "已经授权成功";
                        qrCodeModel.State = QRCodeState.已授权;
                        qrCodeModel.UserId = qrCodeModel.UserId;
                        qrCodeModel.GrantTime = DateTime.Now;
                        BaseCacheManage.AddObject(qrCodeModel.QRCodeStringKey, qrCodeModel, 60 * 1);//加入缓存

                    }
                    return Json(retJson);
                case QRCodeType.兑换:
                    return Json(retJson);

                default:
                    retJson.Status = false;
                    retJson.Info = "未知类型(Type)";
                    return Json(retJson);

            }


        }

    
        private   bool LoginLog(LoginStatistics model)
        {
            model.AddTime = DateTime.Now;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                Machine mModel = db.T_Machine.Find(model.MachineCode);

                model.Lon = mModel.Lon;
                model.Lat = mModel.Lat;
                model.Address = mModel.Address;

                db.Add<LoginStatistics>(model);
                db.SaveChangesAsync();
            }

            return true;
        }
        /// <summary>
        /// 用社区1+1扫码登陆
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private bool QRCodeLogin(int uId, string ticket)
        {
            //dynamic userInfo;
            //bool isLogin = AutheTicketManager.ValidateUserTicket(ticket, out userInfo);
            try
            {
                if (uId > 0)
                {
                    return true;
                }
                var userTicket = FormsAuthentication.Decrypt(ticket);
                return true;

            }
            catch (Exception ex)
            {
                return false;

            }

        }

        public ActionResult App_CheckStateQRCode(string QRCodeStringKey)//该方法和App_DeCoderQRCode是一对配合使用
        {

            JsonMessage retJson = new JsonMessage();

            QRCodeModel qrCodeModel = (QRCodeModel)BaseCacheManage.RetrieveObject(QRCodeStringKey);//获取缓存对象
            if (qrCodeModel == null)
            {
                retJson.Status = false;
                retJson.Info = "QRCodeStringKey,值不存在";

                return Json(retJson);
            }

            if (qrCodeModel.State == QRCodeState.已授权)
            {
                retJson.Status = true;
                retJson.Info = "已授权";
                retJson.Data = new { User = QRCodeLogin(qrCodeModel.UserId), Data = qrCodeModel.Data };
                return Json(retJson);

            }
            retJson.Status = false;
            retJson.Info = "未授权";
            return Json(retJson);
        }

        private Vol QRCodeLogin(int uId)
        {
            Vol appUser = null;
            try
            {

                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var t_a = db.T_Vol;

                    appUser = t_a.Where(x => x.ID == uId).SingleOrDefault();
                    if (appUser != null)
                    {
                        appUser.PassWord = string.Empty;
                        return appUser;

                    }
                    return appUser;
                }

            }
            catch (Exception ex)
            {
                return appUser;

            }

        }
        public ActionResult App_GetVolOrderByScore(string deptid, int page = 1, int pageSize = 20)//该方法和App_DeCoderQRCode是一对配合使用
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_v = db.T_Vol;
                var list = from v in t_v
                           where v.DepId.IndexOf(deptid) > -1
                           select new VolShowModel
                           {
                               DepId = v.DepId,
                               ID = v.ID,
                               Phone = v.Phone,
                               RealName = v.RealName,
                               LoveBankScore = v.LoveBankScore

                           };
                retJson.Data = list.OrderByDescending(x => x.LoveBankScore).ToPagedList(page - 1, pageSize);
                retJson.Status = true;
            }

            return Json(retJson);
        }

        /// <summary>
        /// 监督投诉接口
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AppPostTSAdd(Supervise entity)
        {
            JsonMessage retJson = new JsonMessage();
            try
            {

                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    entity.AddTime = DateTime.Now;
                    db.Add(entity);
                    db.SaveChanges();

                    retJson.Status = true;
                    retJson.Info = "新增成功";
                }
                return Json(retJson);
            }
            catch (Exception ex)
            {
                retJson.Status = false;
                retJson.Data = ex.Message + ex.TargetSite;

                return Json(retJson);
            }
        }

        /// <summary>
        /// App提交意见
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AppPostAddSuggestion(Suggestion entity)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var addUserDetpId = (from v in db.T_Vol where v.ID == entity.AddUser select v.DepId).FirstOrDefault();

                entity.DepId = addUserDetpId;
                entity.AddTime = DateTime.Now;
                db.Add<Suggestion>(entity);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "新增成功";
            }
            return Json(retJson);


        }

        public ActionResult App_GridMemberByuUserId(int userId, int page = 1, int pageSize = 20)
        {
            var pageNumber = page;
            var size = pageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var vol = db.T_Vol;
                var grid = db.T_GridMember;
                var dep = db.T_Department;
                VolShowModel addUserDeptId = (from v in db.T_Vol
                                              where v.ID == userId
                                              select new VolShowModel
                                              {
                                                  DepId = v.DepId,
                                                  Address = v.Address

                                              }).FirstOrDefault();


                var list = from g in grid select g;

                list = list.Where(x => x.DeptId == addUserDeptId.DepId);

                List<GridMember> listObj = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size).ToList();

                var depNameList = from d in dep where d.PId == addUserDeptId.DepId select d;
                foreach (var item in listObj)
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

                List<GridMember> listObj1 = listObj.FindAll(x => x.DeptId == addUserDeptId.DepId && x.VDeptName.Contains(addUserDeptId.Address));
                List<GridMember> listObj2 = listObj.FindAll(x => !(x.DeptId == addUserDeptId.DepId && x.VDeptName.Contains(addUserDeptId.Address)));

                List<GridMember> listObj3 = new List<GridMember>();


                if (listObj1 != null)
                {
                    listObj3 = listObj3.Union(listObj1).ToList();
                }
                if (listObj2 != null)
                {
                    listObj3 = listObj3.Union(listObj2).ToList();
                }

                return Json(listObj3);
            }
        }


        /// <summary>
        /// 兑换产品：
        /// 参数：MacineCode机器Id
        /// LoveBankProductId:被兑换产品Id
        /// MacineCode：被兑换机器唯一码
        /// Type: 账号登陆兑换 = 0,  二维码登陆兑换 = 1,   NFC登陆兑换 = 2
        /// ExChangeCount:兑换数量，默认值是：1
        /// AddUserId：兑换人的用户ID
        /// Address（可选,兑换的地址）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public ActionResult App_ExchangeProduct(LoveBankProductExchangeLog entity)
        {
            entity.AddTime = DateTime.Now;
            entity.ExChangeCount = entity.ExChangeCount <= 0 ? 1 : entity.ExChangeCount;
            entity.Source = "爱心银行";
            entity.State = 0;

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_p = db.T_Product;
                var t_v = db.T_Vol;

                Product productModel = t_p.Find(entity.LoveBankProductId);


                if (productModel == null)
                {
                    retJson.Status = false;
                    retJson.Info = "产品不存在";
                    return Json(retJson);
                }

                if (productModel.Count < entity.ExChangeCount)
                {
                    retJson.Status = false;
                    retJson.Info = "产品数量不足";
                    return Json(retJson);
                }

                Vol vol = t_v.Find(entity.AddUserId);
                if (vol == null)
                {
                    retJson.Status = false;
                    retJson.Info = "用户不存在";
                    return Json(retJson);
                }
                if (vol.LoveBankScore < entity.CostScore)
                {
                    retJson.Status = false;
                    retJson.Info = "积分不足,可用积分:" + vol.LoveBankScore + ".产品所需积分:" + entity.CostScore * entity.ExChangeCount;

                    return Json(retJson);
                }

                productModel.Count = productModel.Count - entity.ExChangeCount;//减少兑换数量
                db.Update<Product>(productModel);
                db.SaveChanges();



                entity.CostScore = productModel.CostScore * entity.ExChangeCount;
                db.Add<LoveBankProductExchangeLog>(entity);//保存兑换记录
                db.SaveChanges();


                vol.LoveBankScore = vol.LoveBankScore - entity.CostScore * entity.ExChangeCount;//减少积分
                db.Update<Vol>(vol);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "兑换成功";
            }
            return Json(retJson);
        }


        /// <summary>
        /// 统计首页各个模块点击情况
        /// ModuleCode：模块自定义编码
        /// ModuleId：模块Id
        /// MachineCode:机器唯一编码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult App_MachineStatistics(MachineStatistics entity)
        {
            entity.AddTime = DateTime.Now;
            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var tm = db.T_Machine;
                Machine mModel = tm.FirstOrDefault(x => x.MachineCode == entity.MachineCode);
                if (mModel != null)
                {
                    entity.DeptId = mModel.DeptId;
                    //entity.ModuleId = mModel.ID;
                }

                db.AddAsync<MachineStatistics>(entity);
                db.SaveChangesAsync();
                retJson.Status = true;
                return Json(retJson);
            }

        }

        /// <summary>
        /// 心跳检查一体机运行状况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult App_MachineHeartbeat(MachineHeartbeat entity)
        {
            entity.AddTime = DateTime.Now;
            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
 
                db.AddAsync<MachineHeartbeat>(entity);
                db.SaveChangesAsync();
                retJson.Status = true;
                return Json(retJson);
            }

        }

        public ActionResult App_LBStartOrBottomAd(string machineCode)
        {
            //AppSourceFileList = (from s in t_s where s.Guid == p.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()
            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tlbs = db.T_LBStartOrBottomAd;
                var tm = db.T_Machine;
                var t_s = db.T_SourceFile;
                var list = from bs in tlbs
                           join m in tm on bs.DeptId equals m.DeptId
                           where m.MachineCode == machineCode
                           select new LBStartOrBottomAdModel
                           {
                               LinkUrl = bs.LinkUrl,
                               Postion=bs.Postion,
                               HttpImgUrl = (from s in t_s where s.Guid == bs.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).FirstOrDefault().ImgHttpUrl

                           };
         
                retJson.Status = true;
                retJson.Data = list.ToList();
                return Json(retJson,JsonRequestBehavior.AllowGet);
            }

        }
    }
}