using System;
using System.Linq;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Web.Code;
using LoveBank.Web.Models;

namespace LoveBank.Web.Controllers
{
    public class HomeController : BaseController
    {
    

        public ActionResult Index()
        {
        

            return View();
        }



        [OutputCache(Duration = 600)]
        public ActionResult Reg()
        {
            return View();
        }

        public PartialViewResult LoginInfo()
        {
            var isLogin = LoveBankContext.Current.IsAuthenticated;
            return PartialView("_LoginInfo", new IsLoginModel(isLogin, isLogin ? LoveBankContext.Current.User : null));
        }

      
        public ActionResult Guide() {
            return View();
        }

        public ActionResult Help() {
            return View();
        }

        public ActionResult Contact() {
            return View();
        }

        public ActionResult About(string p) {
            return View("About"+p);
        }

        public ActionResult Service() {
            return View();
        }

        public ActionResult Guarantee() {
            return View();
        }

        public ActionResult Loan() {
            return View();
        }


        public ActionResult Banks() {
            return View();
        }

        public ActionResult Data()
        {
            return View();
        }

    }
}
