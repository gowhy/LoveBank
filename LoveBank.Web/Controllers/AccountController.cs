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
using LoveBank.Core.Domain;
using LoveBank.Core.MSData;

namespace LoveBank.Web.Controllers {
    public class AccountController : BaseController {

        private readonly IFormsAuthenticationService _authenticationService;

        public AccountController(IFormsAuthenticationService authenticationService) {
       
            Check.Argument.IsNotNull(authenticationService, "authenticationService");

          
            _authenticationService = authenticationService;
        }

        [HttpsRequire]
        public ActionResult Register() {
            return View();
        }

    
        public ActionResult RegSuccess()
        {
            ////注册成功后，自动登录
            //_authenticationService.SignIn(user.UserName, false);
            return View();
        }

    

        [HttpPost]
        public ActionResult PostRegister(string phone, string Password, string Validate)
        {
            if (!ModelState.IsValid) return Error();

            if (!phone.MatchAndNotNull(RegularUtil.Phone)) return Error("手机号错误");

            var cookie = Request.Cookies["PostRegister_valicode"];

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
                Vol model = new Vol();
                model.RealNameState = 1;
                model.State = 1;
                model.Type = "志愿者";
                model.LoveBankScore = 0;
                model.Score = 0;
                model.Source = 2;//来源未2标示是爱心银行网站
                model.Phone = phone;
                model.PassWord = Password;

                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tv = db.T_Vol;

                    if (tv.Count(x => x.Phone == model.Phone) > 0)
                    {
                        return Error("该手机号已经存在");
                    }
                    db.Add<Vol>(model);
                    db.SaveChanges();

                }

                _authenticationService.SignIn(model.ID.ToString(), false);

        

                ViewData["Jump"] = Url.Action("Index", "Home");

                return RedirectToAction("RegSuccess");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
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
        public ActionResult PostLogIn(string phone, string passWord, string ReturnUrl)//UserLoginModel model
        {
          
            
            Vol vModel = null;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tv = db.T_Vol;

                vModel = (from v in tv
                          where v.Phone == phone
                          select new Vol
                              {
                                  PassWord = v.PassWord,
                                  ID = v.ID,
                                  RealName = v.RealName
                              }).SingleOrDefault();
                
                if (vModel==null)
                {
                    return Error("手机号不存在");
                }
                if (vModel.PassWord !=passWord)
                {
                    return Error("登陆失败,密码错误");
                }
            }


            _authenticationService.SignIn(vModel.ID.ToString(), false);

            //SaveLoginInfo(user);

            var returnUrl = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : Url.Content("~/");
            return Redirect(returnUrl);
        }

    

        public ActionResult LogOut() {
            _authenticationService.SignOut();
            return Redirect(Url.Content("~/"));
        }

 
        public ActionResult SecondEmail(string Email)
        {
            //var isHas = DbProvider.D<User>().Any(x => x.Email == Email && x.EmailPassed);
            //if (isHas) return Error("邮箱已经存在，请重新输入！");

            //var qdt_account = Request.Cookies["qdt_account"];
            //if (qdt_account == null) return Error("验证超时！请重新登录获取验证");

            //var Pid = Convert.ToInt32(qdt_account.Value.ToDesDecrypt(Des.LoveBank_Key)); 
            //var Ptime = DateTime.Now.Ticks;
            //var Psign = Email + Des.LoveBank_Key + Ptime.ToString();
            //string activateUrl = MakeActiveUrl(Url.Action("PostRegisterBindEmail", "Account", new { id = Pid, email = Email, time = Ptime, sign = Psign.Hash().Hash() }));
            //var content = PrepareMailBodyWith("Registration", "Email", Email, "ActiveUrl", activateUrl);
            //var msg = new MsgQueueFactory().CreateValidatorMsg(Email, "邮箱验证", content);

            //DbProvider.Add(msg);
            //DbProvider.SaveChanges();

            //SendMailMessage(msg);

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
            //var user = _userService.GetUserByAll(UserName);
            //if (user == null) return Error("不存在此用户！");

            //var userCookie = new HttpCookie("qdt_account") { Value = user.ID.ToString().ToDesEncrypt(Des.LoveBank_Key), Expires = DateTime.Now.AddMinutes(300) };

            //Response.AppendCookie(userCookie);

            return RedirectToAction("FindPasswordByPhone");
            
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