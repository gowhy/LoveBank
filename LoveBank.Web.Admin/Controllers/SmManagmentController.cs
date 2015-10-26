using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Common.Plugins;
using LoveBank.Common.Plugins.Sms;
using LoveBank.MVC.Security;

using LoveBank.P2B.Domain.Config;
using LoveBank.P2B.Domain.Messages;
using LoveBank.Services;
using LoveBank.Services.DTO;
using LoveBank.Services.SmMailModule;
using LoveBank.Web.Admin.Models;

namespace LoveBank.Web.Admin.Controllers {
    /// <summary>
    /// 短信管理
    /// </summary>
    [SecurityModule(Name = "短信管理")]
    public class SmManagmentController : BaseController {
        private readonly IMsgService _msgService;

        public SmManagmentController() {
            _msgService = IoC.Resolve<IMsgService>();
        }

        #region 短信管理

        #region 短信接口列表

        [SecurityNode(Name = "短信接口列表")]
        public ActionResult MsgInterfaceList() {
            IEnumerable<Type> types = SMSPlugins.Instance().GetPlugins();

            List<SmsServerModel> models = DbProvider.D<SmsServer>().ToList().Select(o => new SmsServerModel(o)).ToList();

            foreach (Type type in types) {
                SmsServerModel model = models.FirstOrDefault(o => o.ClassName == type.FullName);
                if (model != null) {
                    model.IsDefault = model.ClassName == SettingManager.Get<MessageConfig>().SmsType;
                    continue;
                }
                SMSAttribute sender = SMSSender.CreateInstance(type.FullName).GetConfig();
                models.Add(new SmsServerModel {
                                                  ServerName = sender.Name,
                                                  ClassName = sender.TypeName,
                                                  Config = sender.Config.ToDictionary(x => x.Title),
                                                  Description = sender.Description,
                                                  IsInstall = false
                                              });
            }


            return View(models);
        }

        /// <summary>
        /// 卸载短信接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityNode(Name = "卸载短信接口")]
        public ActionResult UninstallInterface(int id) {
            DbProvider.Delete<SmsServer>(o => o.ID == id);
            DbProvider.SaveChanges();
            return Success("操作成功");
        }

        [SecurityNode(Name = "安装短信接口页面")]
        public ActionResult InstallInterface(string key) {
            Type type = SMSPlugins.Instance().GetPlugin(key);

            SMSAttribute sender = SMSSender.CreateInstance(type.FullName).GetConfig();

            return View("InstallSms", sender);
        }

        [SecurityNode(Name = "安装短信接口")]
        public ActionResult PostInstallInterface(SmsServerModel model) {

            SMSAttribute smsInfo = SMSSender.CreateInstance(model.ClassName).GetConfig();

            foreach (DynamicConfig config in smsInfo.Config) {
                string key = "Config[{0}]".FormatWith(config.Key);

                switch (config.Type) {
                    case DynamicConfig.AttributeType.Text:
                        config.Value = Request[key] ?? "";
                        break;
                    case DynamicConfig.AttributeType.Select:
                        string[] values = (Request[key] ?? "").SplitNull(',');
                        var groups = config.Value as CheckBoxGroup;

                        if (groups == null) break;

                        foreach (string value in values) {
                            groups.SetChecked(value, true);
                        }
                        break;
                }
            }

            var smsServer = new SmsServer {
                                              ServerName = smsInfo.Name,
                                              ClassName = smsInfo.TypeName,
                                              Config = smsInfo.Config.ToDictionary(x => x.Key),
                                              Description = model.Description,
                                              UserName = model.UserName,
                                              Password = model.Password
                                          };


            DbProvider.Add(smsServer);
            DbProvider.SaveChanges();

            return Success("安装成功");
        }

        /// <summary>
        /// 编辑短信接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [SecurityNode(Name = "编辑短信接口页面")]
        public ActionResult EditMsgInterface(int id) {
            var data = DbProvider.GetByID<SmsServer>(id);
            var model = new SmsServerModel(data);
            return View("EditSms", model);
        }

        [HttpPost]
        [SecurityNode(Name = "编辑短信接口")]
        public ActionResult PostEditMsgInterface(SmsServerModel model) {
            SMSAttribute smsInfo = SMSSender.CreateInstance(model.ClassName).GetConfig();

            foreach (DynamicConfig config in smsInfo.Config) {
                string key = "Config[{0}]".FormatWith(config.Key);

                switch (config.Type) {
                    case DynamicConfig.AttributeType.Text:
                        config.Value = Request[key] ?? "";
                        break;
                    case DynamicConfig.AttributeType.Select:
                        string[] values = (Request[key] ?? "").SplitNull(',');
                        var groups = config.Value as CheckBoxGroup;

                        if (groups == null) break;

                        foreach (string value in values) {
                            groups.SetChecked(value, true);
                        }
                        break;
                }
            }

            SmsServer entity = model.ToEntity();

            var data = DbProvider.GetByID<SmsServer>(entity.ID);

            data.Config = smsInfo.Config.ToDictionary(x => x.Key);
            data.UserName = entity.UserName;
            data.Password = entity.Password;
            data.Description = entity.Description;

            DbProvider.Update(data);
            DbProvider.SaveChanges();

            return Success("编辑成功");
        }

        /// <summary>
        /// 激活接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityNode(Name = "激活短信接口")]
        public ActionResult ActiviteMsgInterface(int id) {
            var config = SettingManager.Get<MessageConfig>();
            SmsServer firstOrDefault = DbProvider.D<SmsServer>().FirstOrDefault(x => x.ID == id);
            if (firstOrDefault == null) return Error("接口不存在或者已经被移除");

            config.SmsType = firstOrDefault.ClassName;
            SettingManager.Save(config);

            return Success("操作成功");
        }

        [SecurityNode(Name = "发送测试短信")]
        public JsonResult SendDemo(string phoneNo) {

            string type = SettingManager.Get<MessageConfig>().SmsType;

            var smsSever = DbProvider.D<SmsServer>().FirstOrDefault(x => x.ClassName == type);

            if (smsSever == null) return Json("还没有设置默认短信发送接口", JsonRequestBehavior.DenyGet);

            var sender = SMSSender.CreateInstance(new SMSAttribute { Config = smsSever.Config.Values.ToArray(), Name = smsSever.ServerName, SmsAccount = smsSever.UserName, SmsPassword = smsSever.Password, TypeName = smsSever.ClassName });

            if (sender == null) return Json("接口不存在或者未实现", JsonRequestBehavior.DenyGet);

            try
            {
                sender.Send(phoneNo, "您的验证码是5658");
                return Json("发送成功", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("发送失败(原因：" + ex.Message + ")", JsonRequestBehavior.AllowGet);
            }

        }

        //[SecurityNode(Name = "查看短信余额")]
        //public string CheckFee() {
        //    var sender = new SmsSender();
        //    return "当前剩余短信条数: " + sender.CheckFee();
        //}

        #endregion

        #region 短信列表

        /// <summary>
        /// 短信列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [SecurityNode(Name = "短信发件箱")]
        public ActionResult Index(int? page, GridSortOptions sort) {

            const int pageSize = 20;
            var pageNumber = page ?? 1;
            ViewData["sort"] = sort;

            int type = (int)MsgType.SMS;

            var source =
                DbProvider.D<MsgQueue>().Where(x => x.InnerType == type && x.IsSend).OrderBy(sort.Column, sort.Direction == SortDirection.Descending);

            return View(source.ToPagedList(pageNumber - 1, pageSize));
        }

        [SecurityNode(Name = "短信队列")]
        public ActionResult Queue(int? page, GridSortOptions sort)
        {

            const int pageSize = 20;
            var pageNumber = page ?? 1;
            ViewData["sort"] = sort;

            int type = (int)MsgType.SMS;

            var source =
                DbProvider.D<MsgQueue>().Where(x => x.InnerType == type && !x.IsSend).OrderBy(sort.Column, sort.Direction == SortDirection.Descending);

            return View(source.ToPagedList(pageNumber - 1, pageSize));
        }

        ///// <summary>
        ///// 新增短信
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[SecurityNode(Name = "添加短信页面")]
        //public ActionResult AddMsg() {
        //    return View();
        //}

        ///// <summary>
        ///// 保存短信
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //[SecurityNode(Name = "添加短信")]
        //public ActionResult PostAddMsg(SmModel model) {
        //    SmDTO dto = model.ToDto();
        //    //_msgService.AddPromoteSm(dto);
        //    return Success("添加成功");
        //}

        /////// <summary>
        /////// 编辑短信
        /////// </summary>
        /////// <param name="id"></param>
        /////// <returns></returns>
        ////[HttpGet]
        ////[SecurityNode(Name = "修改短信页面")]
        ////public ActionResult EditMsg(int id) {
        ////    var data = DbProvider.GetByID<PromoteMsg>(id);
        ////    var model = new SmModel(data);
        ////    return View(model);
        ////}

        ///// <summary>
        ///// 保存编辑
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[SecurityNode(Name = "修改短信")]
        //public ActionResult PostEditMsg(SmModel msg) {
        //    SmDTO dto = msg.ToDto();
        //    _msgService.UpdatePromoteSm(dto);
        //    return Success("编辑成功");
        //}

        ///// <summary>
        ///// 彻底删除短信
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[SecurityNode(Name = "彻底删除短信")]
        //public ActionResult DeleteMsgForever(int[] id) {
        //    _msgService.DeletePromoteSmForever(id);
        //    return Success("删除成功");
        //}

        #endregion

        public ActionResult Add() {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public ActionResult PostAdd(string mobile, string context) {
           var msg= new MsgQueue(mobile, 0)
            {
                Type = MsgType.SMS,
                Content = context,
                IsSend = false,
                IsSuccess = false,
                Result = string.Empty
            };
            DbProvider.Add(msg);
            DbProvider.SaveChanges();
            return Success("提交成功！");
        }
        #endregion
    }
}