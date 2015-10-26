using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.MVC.Security;
using LoveBank.Services.SmMailModule;

namespace LoveBank.Web.Admin.Controllers
{
    [SecurityModule(Name = "消息模版管理")]
    public class MsgTemplateManagmentController : BaseController
    {
        private readonly IMsgService _msgService;

        public MsgTemplateManagmentController()
        {
            
            _msgService = IoC.Resolve<IMsgService>();
            Check.Argument.IsNotNull(_msgService,"_msgservice");
        }

        #region 消息模板管理
        /// <summary>
        /// 消息模版管理入口页（首页）
        /// </summary>
        /// <returns></returns>
        [SecurityNode(Name = "消息模版页")]
        public ActionResult MsgTemplate()
        {
            var tmpList = _msgService.GetAllMsgTemplate();
            ViewData["tmpList"] = tmpList;
            return View("_msgTemplate");
        }

        /// <summary>
        /// 取得指定模板数据
        /// </summary>
        /// <param name="identityName">模板标识名</param>
        /// <returns>指定模板数据的Json格式</returns>
        public JsonResult GetMsgTemplate(string identityName)
        {
            var model = _msgService.QueryMsgTemplateByIdentityName(identityName);
            return Json(new JsonMessage(true, "成功", model), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存编辑后的模板
        /// </summary>
        /// <param name="id">模板Id</param>
        /// <param name="content">模板内容</param>
        /// <param name="isHtml">是否支持Html</param>
        /// <returns>操作结果的Json格式</returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostEditMsgTemplate(int id, string content, bool isHtml)
        {
            try
            {
                _msgService.UpdateMsgTemplate(id, content, isHtml);
                return Json(new JsonMessage(true, "编辑成功!"));
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage(false, ex.Message));
            }
        }

        #endregion
    }
}
