using System;
using System.Linq;
using System.Web.Mvc;


using LoveBank.Web.Admin.Code;
using LoveBank.Web.Admin.Models;
using LoveBank.Common.Data;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.Common;
using LoveBank.Services.LogMoudle;
using LoveBank.MVC;
using LoveBank.Cache;

namespace LoveBank.Web.Admin.Controllers
{
    [AdminAuthorize]
    public abstract class BaseController : Controller
    {
     public readonly static   ICacheStrategy BaseCacheManage = new DefaultCacheStrategy();
        /// <summary>
        /// 每页默认显示条数
        /// </summary>
        protected const int pageSize = 20;

        public IDbProvider DbProvider
        {
            get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; }
        }

        public AdminUser AdminUser
        {
            get
            {
                var admin = HttpContext.Items["LoveBank_Admin"] as AdminUser;
                if (admin == null)
                {
                    admin = DbProvider.D<AdminUser>().FirstOrDefault(x => x.UserName == User.Identity.Name);
                    HttpContext.Items["LoveBank_Admin"] = admin;
                }
                return admin;
            }
        }

        protected ActionResult Message(MsgModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new JsonMessage(model.Type, model.Message), JsonRequestBehavior.AllowGet);
            }
            return View("_Message", model);
        }

        protected ActionResult Error(string msg)
        {
            return Error("操作失败", msg);
        }

        protected ActionResult Error(string title, string msg)
        {
            return Message(new MsgModel
                {
                    Title = title,
                    Message = msg,
                    JumpUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"),
                    WaitSecond = 3,
                    Type = false
                });
        }

        protected ActionResult Error()
        {
            bool sumErr = ModelState.ContainsKey("");
            if (!sumErr)
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError modelError in modelState.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(modelError.ErrorMessage))
                        {
                            return Error(modelError.ErrorMessage);
                        }
                    }
                }
            }

            return Error(ModelState[""].Errors[0].ErrorMessage);
        }

        protected ActionResult Success(string msg)
        {
            return Success("操作成功", msg,
                           Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"));
        }

        protected ActionResult Success(string title, string msg)
        {
            return Success(title, msg,
                           Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"));
        }

        protected ActionResult Success(string title, string msg, string jumpUrl)
        {
            return Message(new MsgModel
                {
                    Title = title,
                    Message = msg,
                    JumpUrl = jumpUrl,
                    WaitSecond = 3,
                    Type = true
                });
        }

        public void Logs(string logInfo, params object[] param)
        {
            IoC.Resolve<IAdminLogService>()
               .AddAdminLog(AdminUser, logInfo.FormatWith(param), Request.Url == null ? null : Request.Url.AbsolutePath,
                            Utility.GetIP());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error(filterContext.Exception.Message, filterContext.Exception);

            if (filterContext.IsChildAction) return;

            filterContext.Result = Error(filterContext.Exception.Message + filterContext.Exception.Source + filterContext.Exception.StackTrace + filterContext.Exception.TargetSite+
              filterContext.Exception.InnerException);

         

            filterContext.ExceptionHandled = true;
        }
    }
}