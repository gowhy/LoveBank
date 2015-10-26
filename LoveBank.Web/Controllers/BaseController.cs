using System;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Core;
using LoveBank.Web.Models;

namespace LoveBank.Web.Controllers
{
    public abstract class BaseController : Controller
    {
      
    

        public IDbProvider DbProvider
        {
            get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; }
        }

        protected ActionResult Message(MessageModel model)
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
            return Message(new MessageModel
            {
                Title = title,
                Message = msg,
                JumpUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"),
                WaitSecond = 3,
                Type = false
            });
        }

        protected ActionResult Error(string title, string msg, string jumpUrl)
        {
            return Message(new MessageModel
            {
                Title = title,
                Message = msg,
                JumpUrl = string.IsNullOrWhiteSpace(jumpUrl) ? Url.Action("Index", "Home") : jumpUrl,
                WaitSecond = 3,
                Type = false
            });
        }

        protected ActionResult Error()
        {
            var sumErr = ModelState.ContainsKey("");
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
            return Message(new MessageModel
            {
                Title = title,
                Message = msg,
                JumpUrl = jumpUrl,
                WaitSecond = 3,
                Type = true
            });
        }

        protected  ActionResult NotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return View("_NotFound");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error(filterContext.Exception.Message, filterContext.Exception);

            if (filterContext.IsChildAction) return;

            filterContext.ExceptionHandled = true;

            filterContext.Result = Error(filterContext.Exception.Message);

        }
    }
}
