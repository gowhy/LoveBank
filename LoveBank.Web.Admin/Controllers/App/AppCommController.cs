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
        public ActionResult App_DepartmentList(string id, int page = 1, int pageSize = 20000)
        {
            JsonMessage retJson = new JsonMessage();

            //using (LoveBankDBContext db = new LoveBankDBContext())
            //{
            //    var dep = db.T_Department;
            //    var list = dep.Where(x => x.Level <= 6).ToList();

            //    var list2 = from d in dep
            //                where d.Level <= 6
            //                select new Department
            //                {
            //                    Name = d.Name,
            //                    Level = d.Level,
            //                    Id = d.Id,
            //                    PId = d.PId,
            //                    Lat = d.Lat,
            //                    Lng = d.Lng
            //                };



            //    retJson.Status = true;
            //    retJson.Data = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize).ToList();
            //    return Json(retJson);
            //}


            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                if (id != null)
                {
                    var list = db.T_Department.AsQueryable<Department>().Where(x => x.PId == id).ToList();
                    retJson.Status = true;
                    retJson.Data = list;
                    return Json(retJson);
                }
                else
                {
                    var list = db.T_Department.AsQueryable<Department>().Where(x => x.Level <= 6).ToList();
                      retJson.Status = true;
                     retJson.Data = list;
                    return Json(retJson);
                }
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

                if (db.T_MachineModuleShowManage.Count(x => x.DeptId == model.DeptId) == 0)
                {
                    ///初始化模块 积分商城：JFSC
                    MachineModuleShowManage initMachineModule1 = new MachineModuleShowManage()
                    {
                        Name = "积分商城",
                        ModuleKey = "JFSC",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "e96fdf19-465c-4190-b47b-27a9af8ecfd4",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/jfsc.png"
                    };

                    ///初始化模块 社区公益：SQGY
                    MachineModuleShowManage initMachineModule2 = new MachineModuleShowManage()
                    {
                        Name = "社区公益",
                        ModuleKey = "SQGY",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "bb2ee2e2-3487-4853-846c-75f71558e389",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/sqgy.png"
                    };


                    ///初始化模块 网格员：WGY
                    MachineModuleShowManage initMachineModule3 = new MachineModuleShowManage()
                    {
                        Name = "网格员",
                        ModuleKey = "WGY",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "e5eafdf6-076f-4ac7-9fd0-38d3c935dd0c",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/wgy.png"
                    };

                    #region 初始化数据
                    ///初始化模块 办事指南：BSZN
                    MachineModuleShowManage initMachineModule4 = new MachineModuleShowManage()
                    {
                        Name = "办事指南",
                        ModuleKey = "BSZN",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "d2ffce6d-9a2a-4f12-869d-5962b18b571e",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/bszn.png"
                    };

                    ///初始化模块 社区动态：SQDT
                    MachineModuleShowManage initMachineModule5 = new MachineModuleShowManage()
                    {
                        Name = "社区动态",
                        ModuleKey = "SQDT",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "de359c69-c7e1-410d-8291-f8717198bc82",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/sqdt.png"
                    };

                    ///初始化模块 生活圈：SHQ
                    MachineModuleShowManage initMachineModule6 = new MachineModuleShowManage()
                    {
                        Name = "生活圈",
                        ModuleKey = "SHQ",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "4b6eadc8-48d8-48d9-bd2b-623717d45406",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/shq.png"
                    };

                    ///初始化模块 志愿者风采：ZYZFC
                    MachineModuleShowManage initMachineModule7 = new MachineModuleShowManage()
                    {
                        Name = "志愿者风采",
                        ModuleKey = "ZYZFC",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "a1581632-87aa-43e0-9b1a-78a3353c1d54",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/zyzfc.png"
                    };

                    ///初始化模块 书记主任信箱：SJZRXX
                    MachineModuleShowManage initMachineModule8 = new MachineModuleShowManage()
                    {
                        Name = "书记主任信箱",
                        ModuleKey = "SJZRXX",
                        DeptId = model.DeptId,
                        AddTime = DateTime.Now,
                        AddUserId = -1,
                        Guid = "682f93c7-b9fa-427e-8c8f-cff0632e87be",
                        Icon = "http://www.24hmart.cn:8082/iplustv/shouye/sjzrxx.png"
                    };
                    #endregion

                    db.Add(initMachineModule1);
                    db.Add(initMachineModule2);
                    db.Add(initMachineModule3);
                    db.Add(initMachineModule4);
                    db.Add(initMachineModule5);
                    db.Add(initMachineModule6);
                    db.Add(initMachineModule7);
                    db.Add(initMachineModule8);
                    db.SaveChanges();
                }
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
                    appUser.VolHeadImg = null;
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

        public ActionResult App_NFCLogin(string nfcUid, string passWord, string machineCode)
        {

            JsonMessage returnJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_a = db.T_Vol;
                //string passWordHash = passWord.Hash();
                Vol appUser =null;
                if (!string.IsNullOrEmpty(passWord))
                {
                    appUser = t_a.Where(x => x.NFC == nfcUid && x.PassWord == passWord).FirstOrDefault();

                }
                else
                {
                    appUser = t_a.Where(x => x.NFC == nfcUid).FirstOrDefault();
                }

                if (appUser != null)
                {
                    returnJson.Status = true;
                    appUser.PassWord = string.Empty;
                    appUser.VolHeadImg = null;
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
                    loginStatistics.Phone = nfcUid;
                    loginStatistics.MachineCode = machineCode;
                    loginStatistics.UserId = appUser.ID;
                    loginStatistics.Type = LoginType.NFC登陆;

                    Action<LoginStatistics> loginStatisticsTarget = s => LoginLog(s);
                    loginStatisticsTarget(loginStatistics);


                    return Json(returnJson);
                }
                else
                {
                    returnJson.Status = false;
                    returnJson.Info = "登录失败,手机号(NFC)或者密码错误";
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
        public ActionResult App_GetProduct(MachineProductModel machine, int page = 1, int pageSize = 80)
        {
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_Product;
                var t_mp = db.T_MachineProduct;
                var t_m = db.T_Machine;
                var t_s = db.T_SourceFile;

                string nulllogo = "http://www.24hmart.cn:8082/TeamProjectImg/20151103/661488175855828992.jpg";
                var list = from p in t_p
                           join m1 in t_mp on p.ID equals m1.ProductId
                           join m2 in t_m on m1.MachineId equals m2.ID
                           where m2.MachineCode == machine.MachineCode&&p.DeptId==m2.DeptId
                           //&&p.StartTime>=DateTime.Now
                           //&&DateTime.Now<=p.EndTime
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
                                Sponsors=p.Sponsors,
                               //SourceFileList = t_s.Where(x => x.Guid == p.Guid).ToList()
                               AppSourceFileList = (from s in t_s where s.Guid == p.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList(),
                               AppSourceFileListLogo = (from s in t_s where s.Guid == p.LogoGuid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList(),
                               //AppSourceFileListAd = (from s in t_s where s.Guid == p.AdGuid select new AppImgUrlModel { ImgHttpUrl = string.IsNullOrEmpty((s.Domain + s.Path)) ? nulllogo : (s.Domain + s.Path) }).ToList()
                               AppSourceFileListAd = (from s in t_s where s.Guid == p.AdGuid select new AppImgUrlModel { ImgHttpUrl = nulllogo }).ToList()
                           };


                list = list.Where(x => x.State != RowState.删除);
                if (!string.IsNullOrEmpty(machine.BarCode)) list = list.Where(x => x.BarCode == machine.BarCode);
                if (!string.IsNullOrEmpty(machine.ProductName)) list = list.Where(x => x.Name.Contains(machine.ProductName));

                List<ProductModel> resList = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize).ToList();
          
                retJson.Status = true;
                retJson.Data = resList;
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
                           && a.MachineCode == machineCode.Trim()&&a.Type==LoveBank_AdType.一体机首页滚动广告
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
                var t_d = db.T_Department;

                var model = (from m in t_m
                             join d in t_d on m.DeptId equals d.Id

                             where m.MachineCode == machineCode
                             select new MachineModel
                             {
                                 DeptIdName = d.Name,
                                 DeptId = m.DeptId,
                                 MachineCode = m.MachineCode,
                                 Lat = m.Lat,
                                 Lon = m.Lon

                             }).FirstOrDefault();

                //Machine model = t_m.SingleOrDefault(x => x.MachineCode == machineCode.Trim());

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
        public ActionResult App_TeamProjectList(string deptId, string type, int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_p = db.T_TeamProject;
                var sf = db.T_SourceFile;


                var list = from p in t_p

                           select new TeamProjectModel
                           {
                               Address = p.Address,
                               AddTime = p.AddTime,
                               Name = p.Name,
                               LinkMan = p.LinkMan,
                               LinkPhone = p.LinkPhone,
                               ProjectEndDate = p.ProjectEndDate,
                               ProjectStartDate = p.ProjectStartDate,
                               Type = p.Type,
                               State = p.State,
                               DeptId = p.DeptId,
                               Desc = p.Desc,
                               HtmlUrl = p.HtmlUrl,
                               Score = p.Score,
                               ServiceDate = p.ServiceDate,
                               ServiceObject = p.ServiceObject,

                               ID = p.ID,
                               AppSourceFileList = (from s in sf where s.Guid == p.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()
                           };

                list = list.Where(x => x.State != RowState.删除 && deptId.IndexOf(x.DeptId) > -1);
                if (!string.IsNullOrEmpty(type)) list = list.Where(x => x.Type == type);

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
                        ////登陆log
                        LoginStatistics loginStatistics = new LoginStatistics();
                        loginStatistics.Type = LoginType.二维码登陆;
                        loginStatistics.LoginState = 1;
                        loginStatistics.MachineCode = qrCodeModel.MachineCode;
                        loginStatistics.UserId = qrCodeModel.UserId;
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
                Machine mModel = db.T_Machine.Where(x=>x.MachineCode==model.MachineCode).FirstOrDefault();
                if (mModel!=null)
                {
                    model.Lon = mModel.Lon;
                    model.Lat = mModel.Lat;
                    model.Address = mModel.Address;
                }
      

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


        /// <summary>
        /// 获取该社区下的所有志愿者，并且根据积分排名
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
                               LoveBankScore = v.LoveBankScore,
                               VolType = v.VolType
                
                           };

              

                retJson.Data = list.OrderByDescending(x => x.LoveBankScore).ToPagedList(page - 1, pageSize);
                retJson.Status = true;
            }

            return Json(retJson);
        }


        //根据志愿者类型获取该社区下的某类志愿者排名
        public ActionResult App_GetVolOrderByTypeScore(string deptid, VolType type, int page = 1, int pageSize = 20)//该方法和App_DeCoderQRCode是一对配合使用
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
                               LoveBankScore = v.LoveBankScore,
                               VolType = v.VolType

                           };

                list = list.Where(x => x.VolType == type);

                retJson.Data = list.OrderByDescending(x => x.LoveBankScore).ToPagedList(page - 1, pageSize);
                retJson.Status = true;
            }

            return Json(retJson);
        }

        //自愿者头像
        public ActionResult App_VolHeadImg(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                byte[] image = (from c in db.T_Vol where c.ID == id select c.VolHeadImg).FirstOrDefault<byte[]>();
                return new FileContentResult(image, "image/jpeg");
            }
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
               
                if (entity.AddUser>0)
                {
                      var addUserModel = (from v in db.T_Vol where v.ID == entity.AddUser select new { v.DepId, v.Phone, v.RealName }).FirstOrDefault();

                      entity.DepId = addUserModel.DepId;
                      entity.AddUserName = addUserModel.RealName;
                      entity.AddUserPhone = addUserModel.Phone;
                }
       
                entity.AddTime = DateTime.Now;
                db.Add<Suggestion>(entity);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "新增成功";
            }
            return Json(retJson);


        }
        /// <summary>
        /// 获取书记信箱
        /// </summary>
        /// <param name="machineCode"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_SuggestionIndex(string machineCode,int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var t_s = db.T_Suggestion;
                var t_m = db.T_Machine;

                var list = from s in t_s
                           join m in t_m on s.DepId equals m.DeptId
                           where m.MachineCode == machineCode
                           select new SuggestionModel
                           {
                               Content = s.Content,
                               AddTime = s.AddTime,
                               DepId = s.DepId,
                               Id = s.Id,
                               AddUser = s.AddUser,
                               AddUserName = s.AddUserName,
                               AddUserPhone = s.AddUserPhone
                               
                           };
               
                retJson.Status = true;
                retJson.Info = "获取成功";
                retJson.Data = list.OrderByDescending(x => x.Id).ToPagedList(page - 1, pageSize);

                return Json(retJson);
            }
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
                var t_d = db.T_Department;
                var t_m = db.T_Machine;


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


                MachineModel mm = (from m in t_m
                                   where m.MachineCode == entity.MacineCode
                                   select new MachineModel
                                   {
                                       Address = m.Address,
                                       Lat = m.Lat,
                                       Lon = m.Lon,
                                       DeptId = m.DeptId

                                   }).FirstOrDefault();

                if (vol.DepId.IndexOf(mm.DeptId) < 0)//判断该机器和用户是否是同一个社区
                {
                    var depName = (from d in t_d where d.Id == mm.DeptId select d.Name).FirstOrDefault();
                    retJson.Status = false;
                    retJson.Info = string.Format("抱歉,该终端只服务于{0}用户", depName);
                    retJson.Data = "-2";
                    return Json(retJson);
                }

                if (vol.DepId.IndexOf(productModel.DeptId) < 0)//判断是否属于该社区
                {
                    var depName = (from d in t_d where d.Id == productModel.DeptId select d.Name).FirstOrDefault();
                    retJson.Status = false;
                    retJson.Info = string.Format("抱歉,该产品属于{0}", depName);
                    retJson.Data = "-2";
                    return Json(retJson);
                }


                int totalCostScore = entity.CostScore * entity.ExChangeCount;
                if (!vol.LoveBankScore.HasValue || vol.LoveBankScore.Value < totalCostScore)
                {
                    retJson.Status = false;
                    retJson.Info = "积分不足,可用积分:" + vol.LoveBankScore + ".对应产品所需总积分:" + totalCostScore + ".兑换数量:" + entity.ExChangeCount;

                    return Json(retJson);
                }

                productModel.Count = productModel.Count - entity.ExChangeCount;//减少兑换数量
                db.Update<Product>(productModel);
                db.SaveChanges();


                entity.Address = mm.Address;
                entity.Lat = mm.Lat;
                entity.Lon = mm.Lon;
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
        /// 获取兑换记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_ExchangeProductLog(int userId,int page =1,int pageSize=20)
        {
         

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var epl = db.T_LoveBankProductExchangeLog;
                var t_p = db.T_Product;

                var list = from ep in epl
                           join p in t_p on ep.LoveBankProductId equals p.ID
                           where ep.AddUserId == userId
                           select new LoveBankProductExchangeLogModel
                           {
                               Address = ep.Address,
                               AddTime = ep.AddTime,
                               CostScore = ep.CostScore,
                               ExChangeCount = ep.ExChangeCount,
                               Id = ep.Id,
                               ProductName = p.Name
                           };
                retJson.Status = true;
                retJson.Info = "请求成功";
                retJson.Data = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize);
                return Json(retJson);
            }
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


        public ActionResult App_UserTeamProject(int userId,int page=1 ,int pageSize=20)
        {
           

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {


                var t_vr = db.T_VolAddScoreRecorde;

                var t_s = db.T_SourceFile;
                var t_t = db.T_TeamProject;
                var t_tps = db.T_TeamProjectSummary;

                var list = from vr in t_vr
                           join p in t_t on vr.TeamProjectId equals p.ID
                           where vr.VolID == userId &&vr.AddScore>0
                           select new TeamProjectModel
                           {
                               Name = p.Name,
                               State = p.State,
                               ID = p.ID,
                               Desc = p.Desc,
                               HtmlUrl = p.HtmlUrl,
                               LinkMan = p.LinkMan,
                               LinkPhone = p.LinkPhone,
                               ProjectEndDate = p.ProjectEndDate,
                               ProjectStartDate = p.ProjectStartDate,
                               //RecruitEndDate = p.RecruitEndDate,
                               //RecruitStartDate = p.RecruitStartDate,
                               Score = p.Score,
                               ServiceDate = p.ServiceDate,
                               ServiceObject = p.ServiceObject,
                               Type = p.Type,
                               Address = p.Address,
                               //SourceFileList = t_s.Where(x => x.Guid == p.Guid).ToList(),
                               AppSourceFileList = (from s in t_s where s.Guid == p.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()

                           };

                retJson.Status = true;
                retJson.Data = list.OrderBy(x => x.ID).ToPagedList(page-1,pageSize).ToList();
                return Json(retJson);
            }
        }


        /// <summary>
        /// 获取公益服务总结，只返回最新一篇
        /// </summary>
        /// <param name="teamProjectId"></param>
        /// <returns></returns>
        public ActionResult App_TeamProjectSummary(int teamProjectId)
        {


            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
           
                var t_t = db.T_TeamProject;
                var t_tps = db.T_TeamProjectSummary;
                var t_s = db.T_SourceFile;

                var list = from tps in t_tps
                           join t in t_t on tps.TeamProjectId equals t.ID

                           select new TeamProjectSummaryModel
                           {
                               SubTitle = tps.SubTitle,
                               Desc = tps.Desc,
                               AddTime = tps.AddTime,
                               Id = tps.Id,
                               TeamProjectName = t.Name,
                               AppSourceFileList = (from s in t_s where s.Guid == tps.Guid select new AppImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()

                           };
                retJson.Status = true;
                retJson.Data = list.OrderByDescending(x=>x.Id).FirstOrDefault();
                return Json(retJson);
            }
        }

        /// <summary>
        /// 获取社区网格员
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult App_GridMember(string deptId, int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tgm = db.T_GridMember;
                var t_d = db.T_Department;

                var dTable = from d in t_d
                             where d.PId == deptId
                             select d;

                List<Department> dlist = dTable.ToList();

                var list = from gm in tgm
                           where gm.DeptId.IndexOf(deptId) > -1
                           select gm;

                List<GridMember> gList = list.OrderBy(x => x.Id).ToPagedList(page - 1, pageSize).ToList();


                List<GridMemberModel> listModel = new List<GridMemberModel>();
           
                GridMemberModel tmp = null;
                foreach (var item in gList)
                {
                    tmp = new GridMemberModel();

                    tmp.VDeptName = dlist.Where(x => item.VDeptId.IndexOf(x.Id) > -1).FirstOrDefault().Name;
                    tmp.GridName = item.GridName;
                    tmp.GridNo = item.GridNo;
                    tmp.GridPhone = item.GridPhone;
                    tmp.GridHeaderImg = item.GridHeaderImg;
                    tmp.Desc = item.Desc;

                    listModel.Add(tmp);
                }
                retJson.Data = listModel;
                retJson.Status = true;
                return Json(retJson);
            }
        }

        public ActionResult App_GetLastApk(AppVerType type)
        {
            var pageNumber = 1;
            var size = 1;
            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var adimg = db.T_AppVer;

                var list = from a in adimg select a;

                list = list.Where(x => x.State == 0 && x.Type == type);
                retJson.Status = true;
                retJson.Data=list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);
                return Json(retJson);
            }
        }

        public ActionResult App_UserApplyProject(int userId, int teamProjectId)
        {
            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {


                //var t_vr = db.T_VolAddScoreRecorde;
                var t_tps = db.T_TeamPojectStationApply;
                var t_v = db.T_Vol;

                if (t_tps.Count(x=>x.TeamPojectId==teamProjectId&&x.ApplyUserId==userId)>0)
                {
                    retJson.Status = false;
                    retJson.Info = "申请失败,你已经申请了";

                    return Json(retJson);
                }

                TeamPojectStationApply model = new TeamPojectStationApply();
                model.TeamPojectId = teamProjectId;
                model.ApplyUserId = userId;

                VolShowModel vmodel =( from v in t_v
                                  where v.ID == userId
                                  select new VolShowModel
                {
                    Phone = v.Phone,
                    RealName=v.RealName
                 
                }).FirstOrDefault();
                model.Phone = vmodel.Phone;
                model.Name = vmodel.RealName;
                db.Add(model);
                db.SaveChanges();
                
                retJson.Status = true;
                retJson.Info ="申请成功";
                return Json(retJson);
            }
        }


        #region 获取爱心基金
        /// <summary>
        /// 获取爱心基金
        /// </summary>
        /// <param name="teamProjectId"></param>
        /// <returns></returns>
        public ActionResult App_GetLoveFoundIndex(string deptId, LoveFundType type, int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_LoveFund;
                var list2 = from w in tws
                            select new LoveFundModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                Type = w.Type,
                                ID=w.ID
                            };

                if ((int)type>0)
                {
                   list2= list2.Where(x=>x.Type == type);
                }
                retJson.Status = true;
                retJson.Data = list2.Where(x => x.DeptId == deptId).OrderByDescending(x => x.Sort).ToPagedList(page-1, pageSize).ToList();
                return Json(retJson);
            }
        }
        //获取爱心基金详情
        public ActionResult App_GetLoveFound(int id)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_LoveFund;
                var detailModel = (from w in tws
                                   where w.ID == id
                                   select new LoveFundModel
                                   {
                                       AddTime = w.AddTime,
                                       Title = w.Title,
                                       DeptId = w.DeptId,
                                       Content = w.Content,
                                       Type=w.Type
                                   }).FirstOrDefault();
                retJson.Data = detailModel;
                retJson.Status = true;
                return Json(retJson);

            }
        }
        #endregion 


        #region 获取爱心风采展
        /// <summary>
        /// 获取爱心风采展列表
        /// </summary>
        /// <param name="teamProjectId"></param>
        /// <returns></returns>
        public ActionResult App_WebLoveShowIndex(string deptId,  int page = 1, int pageSize = 20)
        {

            JsonMessage retJson = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebLoveShow;
                var list2 = from w in tws
                            select new WebLoveShowModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                ID = w.ID
                            };

             
                retJson.Status = true;
                retJson.Data = list2.Where(x => x.DeptId == deptId ).OrderByDescending(x => x.Sort).ToPagedList(page-1, pageSize).ToList();
                return Json(retJson);
            }
        }
        //获取爱心风采展详情
        public ActionResult App_GetWebLoveShow(int id)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebLoveShow;
                var detailModel = (from w in tws
                                   where w.ID == id
                                   select new WebLoveShowModel
                                   {
                                       AddTime = w.AddTime,
                                       Title = w.Title,
                                       DeptId = w.DeptId,
                                       Content = w.Content
                                   }).FirstOrDefault();
                retJson.Data = detailModel;
                retJson.Status = true;
                return Json(retJson);
            }
        }
        #endregion 
    }
}