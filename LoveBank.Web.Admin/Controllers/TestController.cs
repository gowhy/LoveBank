using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveBank.Services.Members;
using LoveBank.Services.AdminModule;
using LoveBank.Core;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using LoveBank.Common;
using LoveBank.Common.Data;
namespace LoveBank.Web.Admin.Controllers
{
    public class TestController : BaseController
    {
     


        [HttpGet]
        public ActionResult Index( GridSortOptions sort)
        {
            //BaseController
            var a = AdminUser;
            var b = User;
           


            return View(AdminUser);
        }


        //
        // GET: /Test/
        [HttpPost]
        public ActionResult Index(LoveBank.Web.Admin.Models.Test model, GridSortOptions sort)
        {

            //var source = DbProvider.D<User>().Where(x => x.IsDelete).OrderBy(sort.Column,
            //                                                             sort.Direction == SortDirection.Ascending);

            //return View(source.ToPagedList(pageNumber - 1, pageSize));

            //LoveBank.Web.Admin.Models.Test model = new Models.Test();



            model.list = DbProvider.D<TestProduct>().Where(x => x.ID > 0).OrderBy(sort.Column, sort.Direction == SortDirection.Descending).ToPagedList<TestProduct>(Int32.Parse( model.UserName), 5);
            return View(model);


        }

    }
}
