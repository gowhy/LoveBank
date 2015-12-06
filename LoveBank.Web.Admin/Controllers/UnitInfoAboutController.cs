using System.Linq;
using System;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Services.AdminModule;
using LoveBank.Common.Data;
using LoveBank.Web.Admin.Models;
using LoveBank.MVC.Security;
using LoveBank.Core.MSData;
using LoveBank.Core.Domain.Enums;
using System.IO;
using System.Web;
using LoveBank.Services;
using LoveBank.Core.SerializerHelp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "网站-关于")]
    public class UnitInfoAboutController : BaseController
    {
      
        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_UnitInfoAbout;
                UnitInfoAbout model = (from w in t_wsn where w.DeptId == AdminUser.DeptId select w).FirstOrDefault();
                if (model==null)
                {
                    model = new UnitInfoAbout();
                }
                return View(model);
            }
        }

        [HttpPost]
        [SecurityNode(Name = "执行保存")]
        public ActionResult PostEdit(UnitInfoAbout parm)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_UnitInfoAbout;

                UnitInfoAbout model= (from w in t_wsn where w.DeptId == AdminUser.DeptId select w).FirstOrDefault();

                if (model==null)
                {
                    parm.AddUserId = AdminUser.ID;
                    parm.DeptId = AdminUser.DeptId;
                    parm.AddTime = DateTime.Now;
                    parm.State = RowState.有效;
                    db.Add(parm);
                    db.SaveChanges();
                    return Success("保存成功");
                }

                //UnitInfoAbout model = t_wsn.Find(parm.ID);

                model.Sort = parm.Sort;
                model.Address = parm.Address;
                model.Title = parm.Title;
                model.Content = parm.Content;
                model.EMail = parm.EMail;
                model.Fax = parm.Fax;
                model.Type = parm.Type;
                model.UnitName = parm.UnitName;


                db.Update(model);
                db.SaveChanges();

                return Success("保存成功");
            }
        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {
            var ad = DbProvider.D<UnitInfoAbout>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.RowState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }
    }
}