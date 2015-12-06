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
using LoveBank.Core.MSData;
using LoveBank.Core.Domain;
using MySql.Data.MySqlClient;

namespace LoveBank.Web.Admin.Controllers
{
    public class TestController : BaseController
    {

        public ActionResult UpdateDep(string pid = "8")
        {
            List<Department> list4 = new List<Department>();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var dep = db.T_Department;

                List<Department> list = (from d in dep select d).ToList();


                List<Department> dpname2 = list.FindAll(x => x.PId == pid);//父节点

                List<Department> list3 = UpdateDepartment(dpname2);
                list4.AddRange(list3);

                while (list3 != null)
                {
                    list3 = UpdateDepartment(list3);
                    if (list3 == null || list3.Count < 1)
                    {
                        break;
                    }
                    list4.AddRange(list3);
                }

                foreach (Department item in list4)
                {
                    string sql = string.Format(@"update department d set PIds='{0}', Ids='{1}',`Names`='{2}' where d.Id='{3}'",

                      item.PIds, item.Ids, item.Names, item.Id);
                    MySqlParameter[] parm = new MySqlParameter[] { };
                    db.Database.ExecuteSqlCommand(sql, parm);
                    db.SaveChangesAsync();
                }
            }



            var s = list4;
            return Content("操作成功");


        }

        public List<Department> UpdateDepartment(List<Department> dpname2)
        {
            List<Department> list3 = new List<Department>();
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var dep = db.T_Department;

                List<Department> list = (from d in dep select d).ToList();


                //List<Department> dpname2 = list.FindAll(x => x.PId == pid);//父节点
                if (dpname2 == null)
                {
                    return null;
                }
                foreach (var dpname in dpname2)
                {
                    List<Department> list2 = list.FindAll(x => x.PId == dpname.Id);

                    foreach (Department item in list2)
                    {
                        if (item.Level < 4)
                        {
                            item.Ids = dpname.Id + "." + item.Id;
                            item.Names = dpname.Names.Trim() + "." + item.Name.Trim();
                            item.PIds = dpname.Ids;
                        }
                        else
                        {

                            item.Ids = dpname.Ids + "." + item.Id.Substring(item.Id.IndexOf(item.PId) + item.PId.Length);
                            item.Names = dpname.Names.Trim() + "." + item.Name.Trim();
                            item.PIds = dpname.Ids;
                        }

                    }
                    list3.AddRange(list2);
                }
                return list3;



            }

        }

        public ActionResult DNameTrim()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var dep = db.T_Department;

                List<Department> list = (from d in dep select d).ToList();
                foreach (Department item in list)
                {
                    string sql = string.Format(@"update department d set `Name`='{0}' where d.Id='{1}'",item.Name.Trim(), item.Id);

                    MySqlParameter[] parm = new MySqlParameter[] { };
                    db.Database.ExecuteSqlCommand(sql, parm);
                    db.SaveChangesAsync();
                }
                List<Department> list2 = (from d in dep select d).ToList();
                return Content("成功");
            }
        }
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
