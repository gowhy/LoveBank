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
    public class WebLoveShowController : BaseController
    {

        public ActionResult Index(int page = 1, int pageSize = 8)
        {
            WebLoveShowModel model = new WebLoveShowModel();
            //获取最近的列表
            var list = Task.Factory.StartNew<List<WebLoveShowModel>>(() =>
            {
                return GetListWebLoveShow(page, pageSize);
            });

            //获取最新的一篇文章
            var NewWebLoveShow = Task.Factory.StartNew<WebLoveShowModel>(() =>
            {
                return GetNewWebLoveShow();
            });

            Task.WaitAll(list, NewWebLoveShow);
            model = NewWebLoveShow.Result;
            model.WebLoveShowModelPageList = list.Result;

            return View(model);
        }

        //获取详情
        public ActionResult Detail(int Id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebLoveShow;
                var detailModel = (from w in tws
                                   where w.ID == Id
                                   select new WebLoveShowModel
                                   {
                                       AddTime = w.AddTime,
                                       Title = w.Title,
                                       DeptId = w.DeptId,
                                       Content = w.Content
                                   }).FirstOrDefault();
                return View(detailModel);

            }
        }

        //获取最近的列表
        private List<WebLoveShowModel> GetListWebLoveShow(int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebLoveShow;
                var list2 = from w in tws
                            select new WebLoveShowModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort
                            };
                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).ToPagedList(page, pageSize).ToList();

            }
        }

        //获取最新的爱心风采展
        private WebLoveShowModel GetNewWebLoveShow()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebLoveShow;
                var list2 = from w in tws
                            select new WebLoveShowModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort
                            };
                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).FirstOrDefault();
            }
        }
    }
}