using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Core.Members;
using LoveBank.MVC.Security;
using LoveBank.Services.Members;
using LoveBank.Services.Payments;
using LoveBank.Web.Code;
using LoveBank.Web.Code.Attributes;
using LoveBank.Web.Code.Enums;
using LoveBank.Web.Models;

namespace LoveBank.Web.Controllers
{
    [DefaultAuthorize]
    public abstract class UserBaseController : BaseController
    {
        protected new User User { get { return LoveBankContext.Current.User; } }
    }

    [HttpsRequire]
    public partial class UserController : UserBaseController
    {
        private readonly IUserService _userService;

        //payment支付服务
        private static IPaymentService PaymentService { get { return IoC.Resolve<IPaymentService>(); } }


        public UserController(IUserService userService)
        {

          

            Check.Argument.IsNotNull(userService, "userService");

            _userService = userService;
        }

        [OutputCache(Duration = 60)]
        public ActionResult UserMenu(MenuRoute route)
        {
            return PartialView("_UserMenu", route);
        }

        public ActionResult Index()
        {
         
            return View();
        }





    }
}
