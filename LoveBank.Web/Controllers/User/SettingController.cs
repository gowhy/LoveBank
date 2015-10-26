using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;
using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;
using LoveBank.P2B.Domain.Messages;
using LoveBank.Web.Code.Enums;
using LoveBank.Web.Models;

namespace LoveBank.Web.Controllers
{
    public partial class UserController
    {
        public ActionResult Account()
        {
            return View(User);
        }

        [HttpPost]
        public ActionResult AccountSetting(AccountSettingModel model)
        {
            if (!ModelState.IsValid) return Error();
            if (User.ID != model.Id) return Error("信息错误");

            User user = _userService.GetUserByID(model.Id);
            bool isChange = false;
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.PasswordOld.Hash().Equals(user.Password))
                {
                    user.ChangePassword(model.Password);
                    isChange = true;
                }
            }
            if (isChange)
            {
                _userService.UpdateUser(user);
                return Success("账户修改成功");
            }
            return Error("原始密码不正确");
        }

        public ActionResult SafePassword()
        {
            return View(User);
        }

        [HttpPost]
        public ActionResult SafePasswordSetting(SafePasswordModel model)
        {
            if (!ModelState.IsValid) return Error();
            if (User.ID != model.Id) return Error("信息错误");

            User user = _userService.GetUserByID(model.Id);
            if (user.SafePasswordPassed)
            {
                if (!model.SafePasswordOld.Hash().Equals(user.SafePassword))
                {
                    return Error("修改失败！安全密码不正确！");
                }
                user.ChangeSafePassword(model.SafePasswordOld, model.SafePassword);
            }
            else
            {
                user.BindSafePassword(model.SafePassword);
            }
            _userService.UpdateUser(user);
            return Success("账户修改成功");
        }

        public ActionResult BindPhone()
        {
            HttpCookie valipass = Request.Cookies["valipass"];

            if (User.MobilePassed)
            {
                if (valipass == null || string.IsNullOrWhiteSpace(valipass.Value))
                {
                    return RedirectToAction("ValidatePassWord", new {route = ChangeRoute.ChangeMobile});
                }
                if (User.SafePassword.Hash() != valipass.Value)
                {
                    return RedirectToAction("ValidatePassWord", new {route = ChangeRoute.ChangeMobile, error = true});
                }
                valipass.Value = null;
                Response.Cookies.Remove("valipass");
            }

            return View(User);
        }

        
        [HttpPost]
        public ActionResult PostBindPhone(int id, string mobile, string validateCode)
        {
            if (User.ID != id) return Error("信息错误");

            HttpCookie phone = Request.Cookies["mobile"];
            HttpCookie valicode = Request.Cookies["valicode"];

            if (valicode == null || phone == null)
            {
                return Error("验证码不正确！");
            }

            string valicodeValue = valicode.Value;
            string phoneValue = phone.Value;

            if (mobile.Hash().Hash() != phoneValue)
            {
                ModelState.AddModelError("", "手机号码不正确！");
                return Error("手机号码不正确！");
            }

            if (validateCode.Hash().Hash() != valicodeValue)
            {
                ModelState.AddModelError("", "验证码错误!");
                return Error("验证码不正确！");
            }

            User user = _userService.GetUserByID(id);
            user.Mobile = mobile;
            user.BindMobile(mobile);
            _userService.UpdateUser(user);

            Response.Cookies.Remove("valipass");

            return Success("手机绑定提交成功！");
        }

        public ActionResult BindIdCard()
        {
            return View(User);
        }

        [HttpPost]
        public ActionResult PostBindIdCard(IdcardSettingModel model)
        {
            if (!ModelState.IsValid) return Error();
            if (User.ID != model.Id) return Error("信息错误");

            User user = _userService.GetUserByID(model.Id);
            if (user.IDCardPassed)
            {
                return Error("身份证已验证，不能修改！");
            }
            if (user.IdValidatorTime >= 3)
            {
                return Error("身份证验证次数超过规定次数，请联系客服进行身份证验证！");
            }
            bool idHasPass = DbProvider.D<User>().Any(x => x.IDCardPassed && x.IDCard.Equals(model.IdCard) && x.GroupID==3);
            if (idHasPass)
            {
                return Error("此身份证已验证，不能重复验证！");
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var validator = new QueryValidatorService();

            string userinfo = "{0},{1}".FormatWith(model.RealName, model.IdCard.ToUpper());
            string result = validator.querySingle("qiandaitong".ToDesEncrypt(), "qiandaitong_B97v_b6^".ToDesEncrypt(),
                                                  "1A020201".ToDesEncrypt(),
                                                  userinfo.ToDesEncrypt());
            if (!string.IsNullOrWhiteSpace(result))
            {
                string re = result.ToDesDecrypt();
                var xd = new XmlDocument();
                xd.LoadXml(re);
                XmlNode xmlRoot = xd.SelectSingleNode("data");
                if (xmlRoot != null)
                {
                    XmlNode messageNode = xmlRoot.ChildNodes[0];
                    string messageStatus = messageNode.ChildNodes[0].InnerText;
                    XmlNode checkInfosNode = xmlRoot.ChildNodes[1];
                    XmlNode checkInfoNode = checkInfosNode.ChildNodes[0];
                    string compStatus = checkInfoNode.ChildNodes[3].InnerText;

                    if (messageStatus == "0")
                    {
                        user.IdValidatorTime++;
                        if (compStatus == "3")
                        {
                            user.BindIDCard(model.IdCard, model.RealName);
                            _userService.UpdateUser(user);
                            return Success("", "身份认证成功！", Url.Action("Index"));
                        }
                        _userService.UpdateUser(user);
                        return Error("身份认证失败，请正确填写您的真实姓名和身份证号！");
                    }
                }
            }
            return Error("身份认证失败！");
        }

     
     

    
        public ActionResult BindEmail()
        {
            HttpCookie valipass = Request.Cookies["valipass"];

            if (User.EmailPassed)
            {
                if (valipass == null || string.IsNullOrWhiteSpace(valipass.Value))
                {
                    return RedirectToAction("ValidatePassWord", new { route = ChangeRoute.ChangeEmail });
                }
                if (User.SafePassword.Hash() != valipass.Value)
                {
                    return RedirectToAction("ValidatePassWord", new { route = ChangeRoute.ChangeEmail, error = true });
                }
                valipass.Value = null;
                Response.AppendCookie(valipass);
            }

            return View(User);
        }

        public ActionResult SendEmail(string email)
        {
            var isHas = DbProvider.D<User>().Any(x => x.Email == email && x.EmailPassed);
            if (isHas) return Error("邮箱已经存在，请重新输入！");

            var Ptime = DateTime.Now.Ticks;
            var Psign = email + Des.LoveBank_Key + Ptime.ToString();
            var activateUrl = MakeActiveUrl(Url.Action("PostBindEmail", "Account", new { id = User.ID, email = email, time = Ptime, sign = Psign.Hash().Hash() }));
            var content = PrepareMailBodyWith("BindEmail", "Email", email, "ActiveUrl", activateUrl);
            var msg = new MsgQueueFactory().CreateValidatorMsg(email, "邮箱验证", content);
            msg.Level = 2;

            DbProvider.Add(msg);
            DbProvider.SaveChanges();
            return Success("","邮件发送成功，请登陆邮箱继续后续操作！", Url.Action("Index"));
        }

        private string PrepareMailBodyWith(string templateName, params string[] pairs)
        {
            string body = GetMailBodyOfTemplate(templateName);

            for (var i = 0; i < pairs.Length; i += 2)
            {
                body = body.Replace("<%={0}%>".FormatWith(pairs[i]), pairs[i + 1]);
            }

            body = body.Replace("<%=siteTitle%>", "").Replace("<%=rootUrl%>", "");

            return body;
        }

        private string GetMailBodyOfTemplate(string templateName)
        {
            string body = string.Empty;
            if (string.IsNullOrEmpty(body))
            {
                body = ReadFileFrom(templateName);
            }
            return body;
        }

        private string ReadFileFrom(string templateName)
        {
            string templateDirectory = HostingEnvironment.IsHosted
            ? HostingEnvironment.MapPath("~/Content/EmailTemp") ?? ""
            : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "EmailTemp");

            string filePath = string.Concat(System.IO.Path.Combine(templateDirectory, templateName), ".txt");

            string body = System.IO.File.ReadAllText(filePath);

            return body;
        }

        private string MakeActiveUrl(string action)
        {
            Uri requestUrl = Url.RequestContext.HttpContext.Request.Url;
            return "{0}://{1}{2}".FormatWith(requestUrl.Scheme, requestUrl.Authority, action);
        }

    }
}