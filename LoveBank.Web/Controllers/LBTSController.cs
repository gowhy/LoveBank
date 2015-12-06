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
    public class LBTSController : BaseController
    {
        [ValidateInput(false)]
        public ActionResult PostAddSupervise(Supervise entity)
        {

            JsonMessage retJson = new JsonMessage();

            entity.DepId = BaseWebSiteConifg.DeptId;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                if (User != null)
                {
                    entity.AddUserName = User.RealName;
                    entity.AddUserPhone = User.Phone;
                    entity.AddUser = User.ID;
                }

                entity.AddTime = DateTime.Now;
                db.Add<Supervise>(entity);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "提交成功";
            }
            return Json(retJson);

        }
    }
}