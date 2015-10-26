using System;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins.Email;
using LoveBank.P2B.Domain.Config;
using LoveBank.Services;
using LoveBank.Web.Code;
using LoveBank.Web.Code.Attributes;
using LoveBank.Web.Models;
using LoveBank.Services.Members;
using LoveBank.Core.Members;
using LoveBank.MVC;
using System.Linq;
using LoveBank.P2B.Domain.Messages;
using System.Web.Hosting;

namespace LoveBank.Web.Controllers {
    public class AccountController : BaseController {

        private readonly IUserService _userService;
        private readonly IFormsAuthenticationService _authenticationService;

        public AccountController(IUserService userService,IFormsAuthenticationService authenticationService) {
            Check.Argument.IsNotNull(userService, "userService");
            Check.Argument.IsNotNull(authenticationService, "authenticationService");

            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpsRequire]
        public ActionResult Register() {
            return View();
        }

        public ActionResult RegisterPlus()
        {
            return View();
        }

        public ActionResult RegSuccess()
        {
            ////注册成功后，自动登录
            //_authenticationService.SignIn(user.UserName, false);
            return View();
        }

        public ActionResult EmailRegSuccess(int id, string Email)
        {
            var user = _userService.GetUserByID(id);
            user.Email = Email;
            return View(user);
        }

        [HttpPost]
        public ActionResult PostRegister(string UserName, string Password, string Validate)
        {
            if (!ModelState.IsValid) return Error();

            if (!UserName.MatchAndNotNull(RegularUtil.UserName)) return Error("只能是字母、数字、点、减号或下划线组成,并且首字母只能是字母或下划线");

            var cookie = Request.Cookies["valicode"];

            if (cookie == null)
            {
                return Error("验证码错误.");
            }

            var cookieValue = cookie.Value;

            if (Validate.Hash().Hash() != cookieValue)
            {
                ModelState.AddModelError("", "验证码错误!");
                return Error("验证码错误.");
            }

            try
            {
                var user = _userService.CreateUser(UserName, Password, true);

                var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddMinutes(300) };
                
                Response.AppendCookie(userCookie);
                
                //跳转到首页，后期可以改成跳转到用户中心
                return RedirectToAction("RegisterBindPhone");
            }
            catch (UserCreateException userException)
            {
                switch (userException.StatusCode)
                {
                    case UserCreateException.DuplicateUserName:
                        return Error("用户名已经存在.");
                    case UserCreateException.DuplicateEmail:
                        return Error("电子邮件已经存在.");
                    case UserCreateException.InvaildEmail:
                        return Error("无效的电子邮件地址.");
                    default:
                        throw userException.InnerException;
                }
            }
        }

        [HttpPost]
        public ActionResult PostReg(UserRegModel model)
        {
            if (!ModelState.IsValid) return Error();

            try
            {
                var user = _userService.CreateUser(model.UserName, model.Password, model.Email, true);

                //注册成功后，自动登录
                _authenticationService.SignIn(user.UserName, false);

                SaveLoginInfo(user);

                ViewData["Jump"] = Url.Action("Index", "Home");

                //跳转到首页，后期可以改成跳转到用户中心
                return RedirectToAction("RegSuccess");
            }
            catch (UserCreateException userException)
            {
                switch (userException.StatusCode)
                {
                    case UserCreateException.DuplicateUserName:
                        return Error("用户名已经存在.");
                    case UserCreateException.DuplicateEmail:
                        return Error("电子邮件已经存在.");
                    case UserCreateException.InvaildEmail:
                        return Error("无效的电子邮件地址.");
                    default:
                        throw userException.InnerException;
                }
            }
        }

        [HttpPost]
        public ActionResult PostRegPlus(UserRegPlusModel model)
        {
            if (!ModelState.IsValid) return Error();

            var isHas = DbProvider.D<User>().Any(x => x.IDCard.Equals(model.IdCard) && x.IDCardPassed && x.GroupID==1);

            if (isHas) return Error("身份证号码已存在");
            
            try
            {
                var user = _userService.CreateOfflineUser(model.UserName, model.Password, model.IdCard, model.RealName, model.Phone);

                //注册成功后，自动登录
//                _authenticationService.SignIn(user.UserName, false);
//
//                SaveLoginInfo(user);

                ViewData["Jump"] = Url.Action("Index", "Home");

                //跳转到首页，后期可以改成跳转到用户中心
                return Success("注册成功");
            }
            catch (UserCreateException userException)
            {
                switch (userException.StatusCode)
                {
                    case UserCreateException.DuplicateUserName:
                        return Error("用户名已经存在.");
                    case UserCreateException.DuplicateEmail:
                        return Error("电子邮件已经存在.");
                    case UserCreateException.InvaildEmail:
                        return Error("无效的电子邮件地址.");
                    default:
                        throw userException.InnerException;
                }
            }
        }

        [HttpsRequire]
        public ActionResult LogIn()
        {
            if (LoveBankContext.Current.IsAuthenticated)
            {
                return Redirect("~/");
            }
            return View(new UserLoginModel(){ReturnUrl = Request["ReturnUrl"]});
        }

        [HttpPost]
        [ActionName("LogIn")]
        public ActionResult PostLogIn(UserLoginModel model)//UserLoginModel model
        {
            if (!ModelState.IsValid) return Error();

            User user;

            var vaild = _userService.ValidateUser(model.UserName, model.Password,out user);

            if (!vaild)
            {
                ModelState.AddModelError("", "账号或密码错误!");
                return View(model);
            }

            if (user.GroupID == 1)
            {
                _authenticationService.SignIn(user.UserName, false);
                SaveLoginInfo(user);
                return RedirectToAction("UserCenter", "Offline");
            }

            if (!user.EmailPassed && !user.MobilePassed)
            {
                var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddMinutes(300) };
                Response.AppendCookie(userCookie);
                return RedirectToAction("RegisterBindPhone");
            }

            _authenticationService.SignIn(user.UserName, false);

            SaveLoginInfo(user);

            var returnUrl = Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl : Url.Content("~/");
            return Redirect(returnUrl);
        }

        public ActionResult RegisterBindEmail()
        {
            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return NotFound();
            return PartialView();
        }

        public ActionResult RegisterBindPhone()
        {
            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return NotFound();
            return PartialView();
        }

        public ActionResult LogOut() {
            _authenticationService.SignOut();
            return Redirect(Url.Content("~/"));
        }

        [HttpPost]
        public ActionResult PostRegisterBindPhone(string mobile, string validateCode)
        {
            var phone = Request.Cookies["mobile"];
            var valicode = Request.Cookies["valicode"];

            if (valicode == null || phone == null)
            {
                return Error("验证码不正确！");
            }

            var valicodeValue = valicode.Value;
            var phoneValue = phone.Value;

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

            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return Error("验证超时！请重新登录获取验证");

            var userID = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key));

            var user = _userService.GetUserByID(userID);

            if (user.MobilePassed)
            {
                return Error("用户已绑定手机号，更换请联系客服！");
            }
            user.Mobile = mobile;
            user.BindMobile(mobile);
            _userService.UpdateUser(user);

            var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddDays(-1) };

            Response.Cookies.Add(userCookie);

            return RedirectToAction("RegSuccess");
        }

        public ActionResult PostRegisterBindEmail(int id,string email, long time,string sign)
        {
            string str = email + Des.LoveBank_Key + time.ToString();
            if (str.Hash().Hash() != sign)
            {
                return Error("验证未通过！无效的链接！");
            }
            if ((DateTime.Now - new DateTime(time)).TotalHours >0.5)
            {
                return Error("时间超过半个小时，验证失效，请从新发送邮件进行验证！");
            }
            var user = _userService.GetUserByEmail(email);
            if (user != null)
            {
                return Error("该邮箱已经被使用！");
            }

            user = _userService.GetUserByID(id);

            if (user == null)
            {
                return Error("用户不存在！无效的链接");
            } 
            
            if (user.EmailPassed) return Error("此页面已经过期");

            user.BindEmail(email);
            _userService.UpdateUser(user);

            var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddDays(-1) };

            Response.Cookies.Add(userCookie);

            return RedirectToAction("RegSuccess");
        }

        [HttpPost]
        public ActionResult Email(string Email)
        {
            var isHas = DbProvider.D<User>().Any(x => x.Email == Email && x.EmailPassed);
            if (isHas) return Error("邮箱已经存在，请重新输入！");

            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return Error("验证超时！请重新登录获取验证");

            var Pid = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key)); 
            var Ptime = DateTime.Now.Ticks;
            var Psign = Email + Des.LoveBank_Key + Ptime.ToString();
            var activateUrl = MakeActiveUrl(Url.Action("PostRegisterBindEmail", "Account", new { id = Pid, email = Email, time = Ptime, sign = Psign.Hash().Hash() }));
            var content = PrepareMailBodyWith("Registration", "Email", Email, "ActiveUrl", activateUrl);
            var msg = new MsgQueueFactory().CreateValidatorMsg(Email, "邮箱验证", content);

            DbProvider.Add(msg);
            DbProvider.SaveChanges();

            SendMailMessage(msg);

            return RedirectToAction("EmailRegSuccess", new { id=Pid,email = Email });
        }

        public ActionResult SecondEmail(string Email)
        {
            var isHas = DbProvider.D<User>().Any(x => x.Email == Email && x.EmailPassed);
            if (isHas) return Error("邮箱已经存在，请重新输入！");

            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return Error("验证超时！请重新登录获取验证");

            var Pid = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key)); 
            var Ptime = DateTime.Now.Ticks;
            var Psign = Email + Des.LoveBank_Key + Ptime.ToString();
            string activateUrl = MakeActiveUrl(Url.Action("PostRegisterBindEmail", "Account", new { id = Pid, email = Email, time = Ptime, sign = Psign.Hash().Hash() }));
            var content = PrepareMailBodyWith("Registration", "Email", Email, "ActiveUrl", activateUrl);
            var msg = new MsgQueueFactory().CreateValidatorMsg(Email, "邮箱验证", content);

            DbProvider.Add(msg);
            DbProvider.SaveChanges();

            SendMailMessage(msg);

            return RedirectToAction("EmailRegSuccess", new { email = Email });
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostForgetPassword(string Validate, string UserName)
        {
            var cookie = Request.Cookies["valicode"];

            if (cookie == null)
            {
                return Error("验证码不能为空！");
            }

            var cookieValue = cookie.Value;

            if (Validate.Hash().Hash() != cookieValue)
            {
                ModelState.AddModelError("", "验证码错误!");
                return RedirectToAction("ForgetPassword");
            }
            var user = _userService.GetUserByAll(UserName);
            if (user == null) return Error("不存在此用户！");

            var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddMinutes(300) };

            Response.AppendCookie(userCookie);

            return RedirectToAction("FindPasswordByPhone");
            
        }

        public ActionResult FindPasswordByEmail()
        {
            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return NotFound();
            return View();
        }

        public ActionResult SendPasswordByEmail(string email)
        {

            if (!email.IsEmail()) return Error("请输入正确的电子邮件");

            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return Error("此页面已经过期");

            var Pid = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key));

            var user = DbProvider.GetByID<User>(Pid);
            if (!user.EmailPassed || email != user.Email)
            {
                return Error("电子邮件与绑定邮箱地址不匹配！");
            }

            var Ptime = DateTime.Now.Ticks;
            var Psign = email + Des.LoveBank_Key + Ptime.ToString();
            var resetUrl = MakeActiveUrl(Url.Action("ResetPasswordByEmail", "Account", new { email = email, time = Ptime, sign = Psign.Hash().Hash() }));
            var content = PrepareMailBodyWith("ForgetPwd", "Email", email, "ResetUrl", resetUrl);
            var msg = new MsgQueueFactory().CreateValidatorMsg(email, "找回密码", content);
            msg.IsSend = true;
            
            DbProvider.Add(msg);
            DbProvider.SaveChanges();

            SendMailMessage(msg);

            return View("EmailFindpasswordSuccess", user);
        }

        public ActionResult ResetPasswordByEmail(string email, long time, string sign)
        {
            string str = email + Des.LoveBank_Key + time.ToString();
            if (str.Hash().Hash() != sign)
            {
                return Error("验证未通过！无效的链接！");
            }
            if ((DateTime.Now - new DateTime(time)).TotalHours > 0.5)
            {
                return Error("时间超过半个小时，验证失效，请从新发送邮件进行验证！");
            }

            var des = email.ToDesEncrypt(Des.LoveBank_Key);

            ViewData["ValidCode"] = des;

            return View("ResetPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(string validate, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                return Error("密码不一致！");
            }
            
            var user = _userService.GetUserByAll(validate.ToDesDecrypt(Des.LoveBank_Key));
            
            user.ChangePassword(Password);
            _userService.UpdateUser(user);

            return View("FindPasswordSuccess",user);
        }

        public ActionResult FindPasswordByPhone()
        {
            var qdt_account = Request.Cookies["qdt_account"];
            if (qdt_account == null) return NotFound();
            return View();
        }

        [HttpPost]
        public ActionResult RestPasswordByPhone(string mobile, string validateCode)
        {
            var phone = Request.Cookies["mobile"];
            var valicode = Request.Cookies["valicode"];

            if (valicode == null || phone == null)
            {
                return Error("验证码不正确！");
            }

            var valicodeValue = valicode.Value;
            var phoneValue = phone.Value;

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

            var des = mobile.ToDesEncrypt(Des.LoveBank_Key);

            ViewData["ValidCode"] = des;

            return View("ResetPassword");
        }

        public ActionResult PostBindEmail(int id, string email, long time, string sign)
        {
            var str = email + Des.LoveBank_Key + time;

            if (str.Hash().Hash() != sign) return Error("验证未通过！无效的链接！");

            if ((DateTime.Now - new DateTime(time)).TotalHours > 0.5) return Error("时间超过半个小时，验证失效，请从新发送邮件进行验证！");

            var user = _userService.GetUserByEmail(email);
            if (user != null) return Error("该邮箱已经被使用！");

            user = _userService.GetUserByID(id);
            if (user == null) return Error("用户不存在！无效的链接");

            user.BindEmail(email);
            _userService.UpdateUser(user);

            return Success("绑定邮箱成功！");
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
            string _templateDirectory = HostingEnvironment.IsHosted
            ? HostingEnvironment.MapPath("~/Content/EmailTemp") ?? ""
            : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "EmailTemp");

            string filePath = string.Concat(System.IO.Path.Combine(_templateDirectory, templateName), ".txt");

            string body = System.IO.File.ReadAllText(filePath);

            return body;
        }

        private string MakeActiveUrl(string action)
        {
            Uri requestUrl = Url.RequestContext.HttpContext.Request.Url;
            return "{0}://{1}{2}".FormatWith(requestUrl.Scheme, requestUrl.Authority, action);
        }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="user"></param>
        private void SaveLoginInfo(User user)
        {
            user.LoginTime = DateTime.Now;
            user.LoginIP = Utility.GetIP();
            _userService.UpdateUser(user);
        }

        private void SendMailMessage(MsgQueue msg)
        {
            var config = SettingManager.Get<EmailConfig>();

            IEmailSender sender = new EmailSender(config.SmtpServer, config.SmtpPort, config.Name, config.SmtpUserName, config.SmtpPassword, config.IsSSL);

            sender.SendMail(msg.Dest, msg.Title, msg.Content, msg.ID, (o, e) =>
            {
                var m = DbProvider.GetByID<MsgQueue>(e.UserState);
                if (e.Error != null)
                {
                    m.IsSuccess = false;
                    m.Result = e.Error.Message;
                }
                else
                {
                    m.IsSuccess = true;
                }
                DbProvider.SaveChanges();
            });
        }
    }
    
}