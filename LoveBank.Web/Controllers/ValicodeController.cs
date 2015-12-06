using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using LoveBank.Common.Plugins.Sms;
using LoveBank.Core.Members;
using LoveBank.P2B.Domain.Config;
using LoveBank.P2B.Domain.Messages;
using System.Web.UI;
using System.Web.Hosting;
using System;
using LoveBank.Services;

namespace LoveBank.Web.Controllers
{
    public class ValicodeController : BaseController
    {

        public ActionResult Image()
        {
            var vCode = new ValidateImage();
            var code = vCode.CreateValidateCode(4);
            var cookie = new HttpCookie("valicode") { Value = code.Hash().Hash() };
            Response.AppendCookie(cookie);
            var bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg"); 
        }

      
      


        private void QuickSendSMS(MsgQueue msg)
        {
            msg.UpdateSend();

            string type = SettingManager.Get<MessageConfig>().SmsType;

            var smsSever = DbProvider.D<SmsServer>().FirstOrDefault(x => x.ClassName == type);

            if (smsSever == null)
            {
                var m = DbProvider.GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = "未设置默认短信通道";
                DbProvider.SaveChanges();
                return;
            }

            var sender = SMSSender.CreateInstance(new SMSAttribute
            {
                Config = smsSever.Config.Values.ToArray(),
                Name = smsSever.ServerName,
                SmsAccount = smsSever.UserName,
                SmsPassword = smsSever.Password,
                TypeName = smsSever.ClassName
            });


            if (sender == null)
            {
                var m = DbProvider.GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = "短信通道不存在";
                DbProvider.SaveChanges();
                return;
            }

            try
            {
                sender.Send(msg.Dest, msg.Content);
                var m = DbProvider.GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = true;
                DbProvider.SaveChanges();
            }
            catch (Exception ex)
            {
                var m = DbProvider.GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = ex.Message;
                DbProvider.SaveChanges();
            }
        }

       
    }
}
