using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Core;

namespace LoveBank.Web.Controllers
{
    public class HelpController : BaseController
    {

        public ActionResult Index(int id=0)
        {
           
            var project = DbProvider.D<Crawl_Data_Item_Selector>().ToList();
        

            return View(id);
        }

    }

    public class TestController : BaseController
    {

        public ActionResult Index(int id = 0)
        {

            var project = DbProvider.D<Crawl_Data_Item_Selector>().ToList();

            return View(id);
        }

    }
}
