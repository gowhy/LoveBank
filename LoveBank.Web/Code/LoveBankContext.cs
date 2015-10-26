using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Common;
using LoveBank.Core.Members;
using LoveBank.Services.Members;

namespace LoveBank.Web.Code
{
    public class LoveBankContext {

        private readonly IUserService _UserService;

        private const string LoveBank_CONTEXT_KEY = "loveBank_context";

        public LoveBankContext(IUserService userService) {

            Check.Argument.IsNotNull(userService, "userService");

            _UserService = userService;

        }

        public bool IsAuthenticated{get { return HttpContext.Current.User.Identity.IsAuthenticated; }}

        private User _user;

        public User User {
            get {

                if (!IsAuthenticated) return null;

                return _user ?? (_user = _UserService.GetUserByName(HttpContext.Current.User.Identity.Name));
            }
        }

        public static LoveBankContext Current {
            get {
                var items = HttpContext.Current.Items;

                if(items[LoveBank_CONTEXT_KEY]==null) {
                    var context = new LoveBankContext(IoC.Resolve<IUserService>());
                    items[LoveBank_CONTEXT_KEY] = context;
                }

                return items[LoveBank_CONTEXT_KEY] as LoveBankContext;
            }
        }
    }
}