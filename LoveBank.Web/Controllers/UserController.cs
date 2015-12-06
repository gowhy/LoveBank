using System;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins.Email;
using LoveBank.P2B.Domain.Config;
using LoveBank.Services;
using LoveBank.Web.Code;
using LoveBank.Web.Code.Attributes;
using LoveBank.Web.Models;
using LoveBank.Services.Members;
using LoveBank.Core.Members;
using LoveBank.MVC;
using System.Linq;
using LoveBank.P2B.Domain.Messages;
using System.Web.Hosting;
using LoveBank.Core.Domain;
using LoveBank.Core.MSData;
using LoveBank.MVC.Security;

namespace LoveBank.Web.Controllers
{
    [DefaultAuthorize]
    public abstract class UserBaseController : BaseController
    {
        protected new VolModel User { get { return LoveBankContext.Current.User; } }
    }

    [HttpsRequire]
    public partial class UserController : UserBaseController
    {
        private readonly IUserService _userService;

        public UserController()
        {

        }
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

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="passWord"></param>
        /// <param name="oldPassWord"></param>
        /// <returns></returns>
        public ActionResult UpdatePassWord(string passWord,string oldPassWord)
        {
            JsonMessage jMessage = new JsonMessage();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tv = db.T_Vol;

                Vol vol = tv.Find(User.ID);

                if (oldPassWord==vol.PassWord)
                {
                    vol.PassWord =passWord;
                    db.Update<Vol>(vol);
                    db.SaveChanges();

                    jMessage.Status = true;
                }
                else
                {
                    jMessage.Status = false;
                    jMessage.Info = "新旧密码不对";
                }
            }
            return Json (jMessage);
        }


    }
}
