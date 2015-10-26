using System;
using System.Web.Mvc;
using LoveBank.Web.Admin.Models;
using LoveBank.Services.AdminModule;
using LoveBank.Core.Domain;
using LoveBank.MVC;
using LoveBank.Services.LogMoudle;
using LoveBank.Common;


namespace LoveBank.Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        protected IAdminService AdminService;
        protected IFormsAuthenticationService FormsService;

        public AccountController(IFormsAuthenticationService formsAuthentication,IAdminService adminService)
        {
            FormsService = formsAuthentication;
            AdminService = adminService;
        }

        public ActionResult LogOn()
        {
            var model = new LogOnModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            if (ModelState.IsValid)
            {
                AdminUser admin;
                if (AdminService.Validate(model.UserName, model.Password, out admin))
                {
                    FormsService.SignIn(model.UserName, false);

                    admin.LoginIP = Utility.GetIP();
                    admin.LoginTime = DateTime.Now;
                    AdminService.UpdateEntity(admin);

                    IoC.Resolve<IAdminLogService>().AddAdminLog(admin, admin.UserName + "登陆成功", "/Account/LogOn", admin.LoginIP);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "您输入的用户名和密码无效！");
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            var msg = new MsgModel
                          {
                              WaitSecond = 3,
                              JumpUrl = Url.Action("LogOn"),
                              Title = "登出成功",
                              Message = "成功登出"
                          };
            return View(msg);
        }

        public ActionResult Unauth()
        {
            return View();
        }

        public ActionResult NoPermission()
        {
            var model =new MsgModel
            {
                Title = "操作失败",
                Message = "没有权限",
                JumpUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"),
                WaitSecond = 30
            };
            return View("_Message", model);
        }


    }
}
