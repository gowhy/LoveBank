using System.Linq;
using System;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Services.AdminModule;
using LoveBank.Common.Data;
using LoveBank.MVC.Security;
using LoveBank.Core.MSData;
using LoveBank.Core.Domain.Enums;
using System.IO;
using System.Web;
using LoveBank.Services;
using LoveBank.Core.SerializerHelp;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LoveBank.Web.Controllers
{
    public class UnitInfoAboutController : BaseController
    {
        //
        // GET: /UnitInfoAbout/
        public ActionResult Index()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_UnitInfoAbout;
                UnitInfoAbout model = (from w in t_wsn where w.DeptId ==BaseWebSiteConifg.DeptId  select w).FirstOrDefault();
                if (model == null)
                {
                    model = new UnitInfoAbout();
                }
                return View(model);
            }
        }
	}
}