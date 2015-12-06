using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Common;
using LoveBank.Core.Members;
using LoveBank.Services.Members;
using LoveBank.Core.Domain;
using LoveBank.Core.MSData;
using LoveBank.Web.Models;

namespace LoveBank.Web.Code
{
    public class LoveBankContext {

        private readonly IUserService _UserService;

        private const string LoveBank_CONTEXT_KEY = "loveBank_context";

        public LoveBankContext(IUserService userService) {

          

        }

        public bool IsAuthenticated{get { return HttpContext.Current.User.Identity.IsAuthenticated; }}

        private VolModel _user;

        public VolModel User
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return null;
                }
                int uid=int.Parse(HttpContext.Current.User.Identity.Name);
                if (!IsAuthenticated) return null;
                using (LoveBankDBContext db = new LoveBankDBContext())
                {
                    var tv = db.T_Vol;
                    VolModel vModel = (from v in tv
                                       where v.ID == uid
                                  select new VolModel
                                      {
                                          ID = v.ID,
                                          RealName = v.RealName,
                                          Phone = v.Phone,
                                          UerName = v.UerName,
                                          DepId = v.DepId

                                      }).SingleOrDefault();
                    return _user ?? vModel;

                }
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