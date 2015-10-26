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

        public ActionResult Phone(string phone)
        {
            var pattern =new Regex( @"^1[3-9][0-9]\d{8}$");
            if (!phone.Match(pattern)) return Error("请输入正确的手机号码");

            var isHas = DbProvider.D<User>().Any(x => x.Mobile == phone && x.MobilePassed);
            if (isHas) return Error("手机号已经存在，请重新输入！");

            var vCode = new ValidateImage();
            var code = vCode.CreateValidateCode(4);
            var valicode = new HttpCookie("valicode") { Value = code.Hash().Hash(),Expires = DateTime.Now.AddMinutes(30)};
            var mobile = new HttpCookie("mobile") { Value = phone.Hash().Hash(),Expires = DateTime.Now.AddMinutes(30)};

            //线下和路人不发短信

            var msg = new MsgQueueFactory().CreateValidatorMsg(phone,code);
            DbProvider.Add(msg);
            DbProvider.SaveChanges();

            QuickSendSMS(msg);

            Response.AppendCookie(valicode);
            Response.AppendCookie(mobile);
            return Success("发送成功");
        }

        public ActionResult FindPasswordByPhone(string phone)
        {
            var pattern = new Regex(@"^1[3-9][0-9]\d{8}$");
            if (!phone.Match(pattern)) return Error("请输入正确的手机号码");

            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return Error("此页面已经过期");

            var Pid = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key));

            var user = DbProvider.GetByID<User>(Pid);
            if (!user.MobilePassed || phone != user.Mobile)
            {
                return Error("手机号与绑定手机号不匹配！");
            }

            var vCode = new ValidateImage();
            var code = vCode.CreateValidateCode(4);
            var valicode = new HttpCookie("valicode") { Value = code.Hash().Hash() };
            var mobile = new HttpCookie("mobile") { Value = phone.Hash().Hash() };

            //线下和路人不发短信

            var msg = new MsgQueueFactory().CreateValidatorMsg(phone, code);
            DbProvider.Add(msg);
            DbProvider.SaveChanges();

            QuickSendSMS(msg);

            Response.AppendCookie(valicode);
            Response.AppendCookie(mobile);

            return Success("发送成功");
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
