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
    /// <summary>
    /// 领导信箱
    /// </summary>
    public class SuggestionController : BaseController
    {
        [ValidateInput(false)]
        public ActionResult PostAddSuggestion(Suggestion entity)
        {

            JsonMessage retJson = new JsonMessage();

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                entity.DepId = BaseWebSiteConifg.DeptId;
                if (User != null)
                {
                    entity.AddUserName = User.RealName;
                    entity.AddUserPhone = User.Phone;
                    entity.AddUser = User.ID;
                }

                entity.AddTime = DateTime.Now;
                db.Add<Suggestion>(entity);
                db.SaveChanges();

                retJson.Status = true;
                retJson.Info = "提交成功";
            }
            return Json(retJson);

        }
    }
}