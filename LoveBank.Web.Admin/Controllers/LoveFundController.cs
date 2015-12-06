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
     [SecurityModule(Name = "网站-爱心基金管理")]
    public class LoveFundController : BaseController
    {
        //
        // GET: /LoveFund/
        const int PageSize = 20;
        [SecurityNode(Name = "首页")]
        public ActionResult Index(LoveFundModel model, int? page, int? pageSize)
        {

            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_LoveFund;

                var list = from w in t_wsn select w;

                list = list.Where(x => x.State != RowState.删除 && x.DeptId.IndexOf(AdminUser.DeptId) > -1);

                if (!string.IsNullOrEmpty(model.Title)) list = list.Where(x => x.Title.Contains(model.Title));
                model.LoveFundList = list.OrderByDescending(x => x.ID).ToPagedList(pageNumber - 1, size);
                return View(model);
            }
        }



        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {

            return View();

        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(LoveFund model)
        {
            #region 初始化参数

            model.AddUserId = AdminUser.ID;
            model.DeptId = AdminUser.DeptId;
            model.AddTime = DateTime.Now;
            model.State = RowState.有效;


            #endregion

            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                db.Add(model);
                db.SaveChanges();

                return Success("添加成功");
            }
        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_LoveFund;
                LoveFund model = t_wsn.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        [SecurityNode(Name = "编辑执行")]
        public ActionResult PostEdit(LoveFund parm)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var t_wsn = db.T_LoveFund;
                LoveFund model = t_wsn.Find(parm.ID);
                model.Sort = parm.Sort;
                model.Type = parm.Type;
                model.Title = parm.Title;
                model.Content = parm.Content;

                db.Update(model);
                db.SaveChanges();

                return Success("修改成功");
            }
        }

        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {
            var ad = DbProvider.D<LoveFund>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.RowState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }
	}
}