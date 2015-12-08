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
    public class CServiceCenterInfoController : BaseController
    {
        public ActionResult Index(CServiceCenterInfoType type, int page = 1, int pageSize = 8)
        {
            CServiceCenterInfoModel model = new CServiceCenterInfoModel();
            model.Type = type;
       
            model.CServiceCenterInfoPageList = GetListCServiceCenterInfo(type, page, pageSize);

            return View(model);

        }

        //获取详情
        public ActionResult Detail(int Id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_CServiceCenterInfo;
                var detailModel = (from w in tws
                                   where w.ID == Id
                                   select new CServiceCenterInfoModel
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
        private IPagedList<CServiceCenterInfoModel> GetListCServiceCenterInfo(CServiceCenterInfoType type, int page = 1, int pageSize = 8)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_CServiceCenterInfo;
                var list2 = from w in tws
                            select new CServiceCenterInfoModel
                            {
                                AddTime = w.AddTime,
                                Title = w.Title,
                                DeptId = w.DeptId,
                                Sort = w.Sort,
                                Type = w.Type
                            };

                if (type != CServiceCenterInfoType.服务中心)
                {
                    list2 = list2.Where(x => x.Type == type);
                }

                return list2.Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).ToPagedList(page-1, pageSize);

            }
        }

        //获取最新的爱心基金
        private CServiceCenterInfoModel GetNewCServiceCenterInfo(CServiceCenterInfoType type)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tws = db.T_CServiceCenterInfo;
                var list2 = from w in tws
                            select new CServiceCenterInfoModel
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