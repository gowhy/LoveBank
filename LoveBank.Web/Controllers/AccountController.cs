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
            _authenticationService.SignIn(User.ID.ToString(), false);
            return View();
        }

    

        [HttpPost]
        [ActionName("Register")]
        public ActionResult PostRegister(string phone, string Password, string Validate)
        {
         

            //if (!phone.MatchAndNotNull(RegularUtil.Phone)) return Error("手机号错误");

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
                return Redirect("~/");
                //return RedirectToAction("RegSuccess");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 短信发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ActionResult SmsValidateCode(string phone)
        {
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
            string code = value.ToString();
            string msg = "【爱心银行】欢迎你，您的注册验证码是：" + code + ".";

            ReturnEntity res = SMSComm.Send(phone, msg);

            JsonMessage rJson = new JsonMessage();
            if (res.State == 1)
            {
                rJson.Status = true;
                rJson.Data = code.Hash();
            

            }

            var userCookie = new HttpCookie("PostRegister_valicode") { Value = code.Hash().Hash(), Expires = DateTime.Now.AddMinutes(5) };

            Response.AppendCookie(userCookie);
            return Json(rJson, JsonRequestBehavior.AllowGet);
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


            VolModel vModel = null;
            using (LoveBankDBContext db = new LoveBankDBContext())
            {
                var tv = db.T_Vol;

                vModel = (from v in tv
                          where v.Phone == phone
                          select new VolModel
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
      
            var returnUrl = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : Url.Content("~/");
            return Redirect(returnUrl);
        }

    

        public ActionResult LogOut() {
            _authenticationService.SignOut();
            return Redirect(Url.Content("~/"));
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