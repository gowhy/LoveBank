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
    public class HomeController : BaseController
    {


        public ActionResult Index2()
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                string s = null;
                s.ToString();

            }

            return View();
        }
        public ActionResult Index()
        {

            ///首页顶端滚动图
            var webFirstAdPicList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db2 = new LoveBankDBContext())
                {
                    var tf = db2.T_WebFirstAdPic;
                    var t_s = db2.T_SourceFile;
                    //ImgUrlModel 
                    var list = from t in tf
                               where t.State!=RowState.删除
                               select new WebFirstAdPicModel
                               {
                                   LinkUrl = t.LinkUrl,
                                   AddUserDeptId=t.AddUserDeptId,
                                   ImgHttpUrl = (from s in t_s where s.Guid == t.Guid select new ImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).FirstOrDefault().ImgHttpUrl
                               };
                    list = list.Where(x=>x.AddUserDeptId==BaseWebSiteConifg.DeptId);
                    return list.ToList();

                }
            });
       
            ///网站公告
            var webSitNoticeList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tw = db.T_WebSitNotice;
                    var list = (from w in tw
                                select new WebSitNoticeModel
                                   {
                                       AddTime = w.AddTime,
                                       Title = w.Title,
                                       Source = w.Source,
                                       Content = w.Content,
                                       DeptId = w.DeptId,
                                       Id = w.ID,
                                       Sort=w.Sort
                                   }).Where(x => x.DeptId == BaseWebSiteConifg.DeptId).OrderByDescending(x => x.Sort).ToPagedList(0, 8).ToList();

                    //List<WebSitNotice> rlist = list;
                    return list;
                }
            });

            //关于
            var unitInfoAbout = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tws = db.T_UnitInfoAbout;

                    return tws.Where(x => x.DeptId==BaseWebSiteConifg.DeptId).FirstOrDefault();
                }
            });
           
            //爱心风采展
            var webLoveShowList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tws = db.T_WebLoveShow;

                    var list = from w in tws select w;

                    list = list.Where(x => x.DeptId == BaseWebSiteConifg.DeptId && x.State != RowState.删除);
                    return list.OrderByDescending(x => x.Sort).ToPagedList(0, 6).ToList();

                }
            });

            //爱心基金
            var loveFundList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tws = db.T_LoveFund;

                    var list = from w in tws select w;
                    list = list.Where(x => x.DeptId == BaseWebSiteConifg.DeptId && x.State != RowState.删除);
                    return list.OrderByDescending(x => x.Sort).ToPagedList(0, 6).ToList();
                }
            });


            //志愿者排名
            var volList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tws = db.T_Vol;

                    var list = from w in tws select w;
                    list = list.Where(x => x.DepId == BaseWebSiteConifg.DeptId );
                    return list.OrderByDescending(x => x.LoveBankScore).ToPagedList(0, 6).ToList();
                }
            });

            //公益组织
            var teamList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tws = db.T_Team;

                    var list = from w in tws select w;
                    list = list.Where(x => x.DeptId == BaseWebSiteConifg.DeptId);
                    return list.OrderByDescending(x => x.ID).ToPagedList(0, 6).ToList();
                }
            });

            //爱心商品
            var productList = Task.Factory.StartNew(delegate
            {
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tp = db.T_Product;
                    var t_s = db.T_SourceFile;
                    var list = from p in tp
                               where p.DeptId == BaseWebSiteConifg.DeptId && p.State != RowState.删除
                               select new ProductModel {
                                   CostScore = p.CostScore,
                                   DeptId = p.DeptId,
                                   Id = p.ID,
                                   Name = p.Name,
                                   Price = p.Price,        
                                   State = p.State
                                   ,
                                   ProductPic = (from s in t_s where s.Guid == p.Guid select new ImgUrlModel { ImgHttpUrl = s.Domain + s.Path }).ToList()
                               
                               };
                    return list.OrderByDescending(x => x.Id).ToPagedList(0, 7).ToList();
                }
            });



            Task.WaitAll(webFirstAdPicList, webSitNoticeList, webLoveShowList, unitInfoAbout
                        ,loveFundList, volList, teamList, productList);


            HomeModel homeModel = new HomeModel();
            //homeModel.DeptIdName = BaseWebSiteConifg.DeptIdName;
            homeModel.LoveFundList = loveFundList.Result;
            homeModel.TeamList = teamList.Result;
            homeModel.UnitInfoAbout = unitInfoAbout.Result;
            homeModel.VolList = volList.Result;
            homeModel.ProductList = productList.Result;
            homeModel.WebSitNoticeList = webSitNoticeList.Result;
            homeModel.WebLoveShowList = webLoveShowList.Result;

            homeModel.WebFirstAdPicModelList = webFirstAdPicList.Result;

            ViewBag.DeptIdName = BaseWebSiteConifg.DeptIdName;

            return View(homeModel);
        }
    }
}
