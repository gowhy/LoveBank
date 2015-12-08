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
    public class ConvenienceServicesController : BaseController
    {

        public ActionResult Index(ConvenienceServicesType type, int page = 1, int pageSize = 8)
        {
            ConvenienceServicesModel model = new ConvenienceServicesModel();
            model.Type = type;
            //获取最近的列表
            //var list = Task.Factory.StartNew<List<ConvenienceServicesModel>>(() =>
            //{
            //    return GetListConvenienceServices(type, page, pageSize);
            //});

            ////获取最新的一篇文章
            //var NewConvenienceServices = Task.Factory.StartNew<ConvenienceServicesModel>(() =>
            //{
            //    return GetNewConvenienceServices(type);
            //});

            //Task.WaitAll(list, NewConvenienceServices);

            //model = NewConvenienceServices.Result;
            model.ConvenienceServicesModelList = GetListConvenienceServices(type, page, pageSize);

            return View(model);
        }

        //获取详情
        public ActionResult Detail(int Id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_ConvenienceServices;
                var detailModel = (from w in tws
                                   where w.ID == Id
                                   select new ConvenienceServicesModel
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
        private IPagedList<ConvenienceServicesModel> GetListConvenienceServices(ConvenienceServicesType type, int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_ConvenienceServices;
                var list2 = from w in tws
                            select new ConvenienceServicesModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                Type = w.Type
                            };
                if (type != ConvenienceServicesType.所有便民服务)
                {
                    list2 = list2.Where(x => x.Type == type);
                }

                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).ToPagedList(page-1, pageSize);

            }
        }

        //获取最新的爱心基金
        private ConvenienceServicesModel GetNewConvenienceServices(ConvenienceServicesType type)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_ConvenienceServices;
                var list2 = from w in tws
                            select new ConvenienceServicesModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                Type = w.Type
                            };
                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId && x.Type == type).OrderByDescending(x => x.Sort).FirstOrDefault();
            }
        }
	}
}