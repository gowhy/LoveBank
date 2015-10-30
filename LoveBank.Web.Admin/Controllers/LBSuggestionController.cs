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
    [SecurityModule(Name = "书记信箱")]
    public class LBSuggestionController : BaseController
    {
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int page = 1, int pageSize = 20)
        {

            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                var sugg = db.T_Suggestion;
                var dep = db.T_Department;
                var vol = db.T_Vol;

                var list = from s in sugg
                           join d in dep on s.DepId equals d.Id
                           join v in vol on s.AddUser equals v.ID
                           select new SuggestionModel
                           {
                               AddUser = s.AddUser,
                               Content = s.Content,
                               AddTime = s.AddTime,
                               DepId = s.DepId,
                               DepName = d.Name,
                               Id = s.Id,
                               Vol = v
                           };
                list = list.Where(x => x.DepId.IndexOf(AdminUser.DeptId) > -1);
                return View(list.OrderByDescending(x => x.Id).ToPagedList(page - 1, pageSize));
            }
        }


        public ActionResult View(int id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
            

                var sup = db.T_Supervise;
                var dep = db.T_Department;
                var vol = db.T_Vol;
                var grid = db.T_GridMember;
                var list = from s in sup
                           join d in dep on s.DepId equals d.Id
                           join v in vol on s.AddUser equals v.ID
                           select new SuperviseModel
                           {
                               AddUser = s.AddUser,
                               Content = s.Content,
                               AddTime = s.AddTime,
                               DeptId = s.DepId,
                               DepName = d.Name,
                               Id = s.Id,
                               ImgUrl = s.ImgUrl,
                               volEntity = v,
                               Msg = s.Msg,
                               State = s.State,
                               Address = s.Address,
                               Lat = s.Lat,
                               Lng = s.Lng
                           };
                var model = list.SingleOrDefault(x => x.Id == id);

                GridMember gridMember = grid.Where(x => x.VDeptId.StartsWith(model.DeptId)).FirstOrDefault();
                model.GridMember = gridMember;
                return View(model);
            }
        }
        [HttpPost]
        [SecurityNode(Name = "删除执行")]
        public ActionResult Delete(int id)
        {
            var ad = DbProvider.D<Product>().FirstOrDefault(x => x.ID == id);
            ad.State = LoveBank.Core.Domain.Enums.RowState.删除;
            DbProvider.SaveChanges();
            return Success("删除成功");
        }

        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            if (file == null)
            {
                Error("请选择文件");
            }
            
                SourceFile res = UploadFileInstance.SaveFile(file, "LBTSImg", AdminUser.ID);
                return Json(res);
            
        }

        public ActionResult AdminApply(int id, int state, string msg)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {

                Supervise model = db.T_Supervise.Find(id);


                var vol = db.T_Vol;
                Vol vEntity = vol.Find(model.AddUser);
                if (model.State != SuperviseRowState.已回复)
                {
                    vEntity.Score = vEntity.Score + 5;
                    db.Update<Vol>(vEntity);
                    db.SaveChanges();
                }

                model.Msg = msg.Trim();
                model.State = (SuperviseRowState)state;

                db.Update<Supervise>(model);
                db.SaveChanges();

                #region 发送通知信息
                //短信通知
                //string failMsg = "【社区1＋1】您于" + model.AddTime.ToString("yyyy-MM-dd HH:mm") + "投诉的:" + model.Content + ".管理员处理,处理意见是:" + msg;
                SMSComm.Send(vEntity.Phone, "【社区1＋1】您于" + model.AddTime.ToString("yyyy-MM-dd HH:mm") + "反映的问题我们将尽快核实处理，感谢您的参与.");

                //App 推送通知待完善
          
                #endregion
            }


            return Success("操作成功");
        }

        public PartialViewResult ShowMap(int? id)
        {
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                Supervise sup = db.T_Supervise.Find(id);

                return PartialView(sup);
            }

        }
    }
}