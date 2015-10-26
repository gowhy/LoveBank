using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins.Sms;
using LoveBank.Core.Members;
using LoveBank.P2B.Domain.Config;
using LoveBank.P2B.Domain.Messages;
using LoveBank.Services;
using LoveBank.Web.Code.Enums;
using LoveBank.Common.Plugins;

namespace LoveBank.Web.Controllers
{
    public partial class UserController
    {
        public ActionResult SafeCenter()
        {
            return View(User);
        }

        public ActionResult PrivacyProtect()
        {
            return View(User);
        }

        public ActionResult ValidatePassWord(ChangeRoute route, bool error=false)
        {
            if (!User.SafePasswordPassed)
            {
                return RedirectToAction("SafePassword");
            }
            ViewBag.Route = route;
            ViewBag.IsError = error;
            return View(User);
        }

        public ActionResult PostValidatePassWord(int id, string safePassword, ChangeRoute route)
        {
            
            if (User.SafePassword == safePassword.Hash())
            {
                var valipass = new HttpCookie("valipass") { Value = safePassword.Hash().Hash() };
                Response.AppendCookie(valipass);
                if (route == ChangeRoute.ChangeMobile)
                {
                    return RedirectToAction("BindPhone");
                }
                if (route == ChangeRoute.ChangeEmail)
                {
                    return RedirectToAction("BindEmail");
                }
            }
            return Error("安全密码错误");
        }

        public ActionResult ForgetSafePwd() {
            if (!User.MobilePassed) return Error("未绑定手机，无法找回安全密码，请先绑定手机或联系管理员");
            return View(User);
        }

        [HttpPost]
        public ActionResult ForgetSafePwd(string validCode, string safePassword)
        {
            var phone = Request.Cookies["mobile"];
            var valicode = Request.Cookies["valicode"];

            if (valicode == null || phone == null)
            {
                return Error("验证码不正确！");
            }

            var valicodeValue = valicode.Value;
            var phoneValue = phone.Value;

            if (validCode.Hash().Hash() != valicodeValue)
            {
                ModelState.AddModelError("", "验证码错误!");
                return Error("验证码不正确！");
            }

            if (User.Mobile.Hash().Hash() != phoneValue)
            {
                ModelState.AddModelError("", "手机号码不正确！");
                return Error("手机号码不正确！");
            }

            var user = _userService.GetUserByID(User.ID);

            user.ChangeSafePassword(safePassword);
            
            _userService.UpdateUser(user);

            return Success("安全密码重设成功");

        }

        public ActionResult SendSafeByPhone()
        {

            if (!User.MobilePassed || string.IsNullOrWhiteSpace(User.Mobile))
            {
                return Error("手机号与绑定手机号不匹配！");
            }

            var vCode = new ValidateImage();
            var code = vCode.CreateValidateCode(4);
            var valicode = new HttpCookie("valicode") { Value = code.Hash().Hash() };
            var mobile = new HttpCookie("mobile") { Value = User.Mobile.Hash().Hash() };

            //线下和路人不发短信

            var msg = new MsgQueueFactory().CreateValidatorMsg(User.Mobile, code);
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
