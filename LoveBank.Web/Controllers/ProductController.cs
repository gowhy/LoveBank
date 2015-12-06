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
using LoveBank.Web.Models;


namespace LoveBank.Web.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        public ActionResult Index(int page = 1, int pageSize = 8)
        {



            var list = Task.Factory.StartNew<List<ProductModel>>(() =>
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {

                    return GetProduct(page, pageSize);
                }
            });

            var listHot = Task.Factory.StartNew<List<ProductModel>>(() =>
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {

                    return GetHotExchangProduct(page, pageSize);
                }
            });

            Task.WaitAll(list,listHot);

            LoveProductModel model = new LoveProductModel();
            model.ProductList = list.Result;
            model.ProductListHot = listHot.Result;

            return View(model);
        }

        private List<ProductModel> GetProduct(int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tp = db.T_Product;
                var t_s = db.T_SourceFile;
                var list = from p in tp
                           where p.DeptId == BaseWebSiteConifg.DeptId && p.State != RowState.删除
                           select new ProductModel
                           {
                               CostScore = p.CostScore,
                               DeptId = p.DeptId,
                               Id = p.ID,
                               Name = p.Name,
                               Price = p.Price,
                               State = p.State
                               ,
                               ProductPic = (from s in t_s where s.Guid == p.Guid select new ImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()

                           };
                return list.OrderByDescending(x => x.Id).ToPagedList(page, pageSize).ToList();
            }
        }

        private List<ProductModel> GetHotExchangProduct(int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tp = db.T_Product;
                var tpe = db.T_LoveBankProductExchangeLog;
                var t_s = db.T_SourceFile;
                var list = from pe in tpe
                           join p in tp on  pe.LoveBankProductId equals p.ID
                           where p.DeptId == BaseWebSiteConifg.DeptId && p.State != RowState.删除
                           select new ProductModel
                           {
                               CostScore = p.CostScore,
                               DeptId = p.DeptId,
                               Id = p.ID,
                               Name = p.Name,
                               Price = p.Price,
                               State = p.State,
                               ProductPic = (from s in t_s where s.Guid == p.Guid select new ImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()

                           };
                return list.OrderByDescending(x => x.Id).ToPagedList(page, pageSize).ToList();

            }
        }
	}
}