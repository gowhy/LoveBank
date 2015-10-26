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
        public ActionResult App_Login(string phone, string passWord)
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

                var list = from p in t_p
                           join m1 in t_mp on p.ID equals m1.ProductId
                           join m2 in t_m on m1.MachineId equals m2.ID
                           where m2.MachineCode == machine.MachineCode

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
                               State = p.State

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
                           where d.Id == deptId
                           select new MachineModuleShowManageModel
                           {
                               LinkUrl = a.LinkUrl,
                               ModuleKey = a.ModuleKey,
                               Name = a.Name,
                               IconUrl = (from s in sf where s.Guid == a.Guid select new { ImgHttpUrl = s.Domain + s.Path }).FirstOrDefault().ImgHttpUrl,
                               Sort = a.Sort,
                               Guid = a.Guid,
                               Type = a.Type,
                               AddTime = a.AddTime,
                               ID = a.ID,
                               DeptId = a.DeptId,
                               Department = d,
                               State = a.State
                           };

                list = list.Where(x => x.State != RowState.删除);


                return Json(list.OrderByDescending(x => x.ID).ToPagedList(page - 1, pageSize).ToList());
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

                return Json(list.ToList());
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
        /// <summary>
        /// MachineCode（必填）:机器为一码
        /// Type（必填）:   登陆 = 0,  兑换 = 1
        /// Data：客户端需要使用的其他数据，可以穿json数据，不用作服务端运算,在跨设备时之间进行中间数据传输
        /// GrantLon：经度
        /// GrantLat：纬度
        /// GrantAddress：授权地址
        /// 
        /// </summary>
        /// <param name="qrCodeModel"></param>
        /// <returns></returns>
        public ActionResult App_QRCode(QRCodeModel qrCodeModel)//该方法和App_DeCoderQRCode是一对配合使用
        {
            JsonMessage retJson = new JsonMessage();
            string encoderStringKey = qrCodeModel.MachineCode + DateTime.Now.Ticks;//生成唯一key


            MemoryStream ms = new MemoryStream();
            QRCodeHelper.GetQRCode(encoderStringKey, ms);
            byte[] _ImageBytes = ms.ToArray();

            //QRCodeModel qrCodeModel = new QRCodeModel();

            qrCodeModel.AddTime = DateTime.Now;
            qrCodeModel.MachineCode = qrCodeModel.MachineCode;
            BaseCacheManage.AddObject(encoderStringKey, qrCodeModel, 60 * 3);//加入缓存


            retJson.Status = true;
            retJson.Data = new
            {
                MachineCode = qrCodeModel.MachineCode,
                QRCodeStringKey = encoderStringKey,
                QRCodeImg = "data:image/jpg;base64," + Convert.ToBase64String(_ImageBytes)
            };
            //ViewBag.img = Convert.ToBase64String(_ImageBytes);
            //return View();
            return Json(retJson,JsonRequestBehavior.AllowGet);
        }

        public ActionResult App_DeCoderQRCode(string QRCodeStringKey, int appUserId, string obj)//该方法和App_QRCode是一对配合使用
        {
            QRCodeModel qrCodeModel = (QRCodeModel)BaseCacheManage.RetrieveObject(QRCodeStringKey);//获取缓存对象

            JsonMessage retJson = new JsonMessage();
            if (qrCodeModel == null)
            {
                retJson.Status = false;
                retJson.Info = "已失效";

                return Json(retJson);
            }
            else
            {
                if (qrCodeModel.State != QRCodeState.待授权)
                {
                    retJson.Status = false;
                    retJson.Info = "已失效";
                    return Json(retJson);
                }

                switch (qrCodeModel.Type)
                {
                    case QRCodeType.登陆:
                        retJson.Status = QRCodeLogin(appUserId, obj);///登录时obj参数是App存储在本科的票据Ticket
                        retJson.Data = obj;
                        if (retJson.Status == true)
                        {
                            retJson.Info = "已经授权成功";
                            qrCodeModel.State = QRCodeState.已授权;
                            qrCodeModel.UserId = appUserId;
                            qrCodeModel.GrantTime = DateTime.Now;
                            BaseCacheManage.AddObject(QRCodeStringKey, qrCodeModel, 60 * 1);//加入缓存
                            //BaseCacheManage.RemoveObject(QRCodeStringKey);//授权成功清除缓存对象
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
                retJson.Data = new { User = QRCodeLogin(qrCodeModel.UserId), Data = qrCodeModel.Data};
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
                        appUser.PassWord =string.Empty;
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
    }
}