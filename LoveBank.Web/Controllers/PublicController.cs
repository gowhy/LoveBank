using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveBank.Web.Controllers
{
    public class PublicController : Controller
    {
        
//        [OutputCache(Duration = 60)]
        public PartialViewResult Footer() {
            return PartialView();
        }
         [OutputCache(Duration = 60)]
        public PartialViewResult TrustLogo()
        {
            return PartialView();
        }

    }
}
