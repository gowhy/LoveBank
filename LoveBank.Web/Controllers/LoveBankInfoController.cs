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
    public class LoveBankInfoController : BaseController
    {
        public ActionResult Index(LoveBankInfoType type, int page = 1, int pageSize = 8)
        {
            LoveBankInfoModel model = new LoveBankInfoModel();
            //获取最近的列表
            var list = Task.Factory.StartNew<List<LoveBankInfoModel>>(() =>
            {
                return GetListLoveBankInfo(type, page, pageSize);
            });

            //获取最新的一篇文章
            var NewLoveBankInfo = Task.Factory.StartNew<LoveBankInfoModel>(() =>
            {
                return GetNewLoveBankInfo(type);
            });

            Task.WaitAll(list, NewLoveBankInfo);
            model = NewLoveBankInfo.Result;
            model.LoveBankInfoPageList = list.Result;
            return View(model);
        }

        //获取详情
        public ActionResult Detail(int Id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_LoveBankInfo;
                var detailModel = (from w in tws
                                   where w.ID == Id
                                   select new LoveBankInfoModel
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
        private List<LoveBankInfoModel> GetListLoveBankInfo(LoveBankInfoType type, int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_LoveBankInfo;
                var list2 = from w in tws
                            select new LoveBankInfoModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                Type = w.Type
                            };
                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId && x.Type == type).OrderByDescending(x => x.Sort).ToPagedList(page, pageSize).ToList();

            }
        }

        //获取最新的爱心基金
        private LoveBankInfoModel GetNewLoveBankInfo(LoveBankInfoType type)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_LoveBankInfo;
                var list2 = from w in tws
                            select new LoveBankInfoModel
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