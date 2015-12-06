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
    public class WebSitNoticeController : BaseController
    {


        public ActionResult Index(int page = 1, int pageSize = 8)
        {
            WebSitNoticeModel model = new WebSitNoticeModel();
            //获取最近的列表
            var list = Task.Factory.StartNew<List<WebSitNoticeModel>>(() =>
            {
                return GetListWebSitNotice(page, pageSize);
            });

            //获取最新的一篇文章
            var NewWebSitNotice = Task.Factory.StartNew<WebSitNoticeModel>(() =>
            {
                return GetNewWebSitNotice();
            });

            Task.WaitAll(list, NewWebSitNotice);
            model = NewWebSitNotice.Result;
            model.WebSitNoticeModelPageList = list.Result;

            return View(model);
        }

        //获取详情
        public ActionResult Detail(int Id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebSitNotice;
                var detailModel = (from w in tws
                                   where w.ID == Id
                                   select new WebSitNoticeModel
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
        private List<WebSitNoticeModel> GetListWebSitNotice(int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebSitNotice;
                var list2 = from w in tws
                            select new WebSitNoticeModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort

                            };
                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).ToPagedList(page, pageSize).ToList();

            }
        }

        //获取最新的爱心基金
        private WebSitNoticeModel GetNewWebSitNotice()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_WebSitNotice;
                var list2 = from w in tws
                            select new WebSitNoticeModel
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