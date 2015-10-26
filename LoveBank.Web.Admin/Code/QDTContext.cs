using System.Web;
using QDT.Common;
using QDT.Core.Domain;
using QDT.Services.AdminModule;

namespace QDT.Web.Admin.Code
{
    public class QDTContext {

        private readonly IAdminService _adminService;

        private const string QdtContextKey = "qdt_context";

        public QDTContext(IAdminService adminService)
        {

            Check.Argument.IsNotNull(adminService, "adminService");

            _adminService = adminService;

        }

        public bool IsAuthenticated{get { return HttpContext.Current.User.Identity.IsAuthenticated; }}

        private AdminUser _admin;

        public AdminUser Admin
        {
            get {

                if (!IsAuthenticated) return null;

                return _admin ?? (_admin = _adminService.GetAdmin(HttpContext.Current.User.Identity.Name));
            }
        }

        public static QDTContext Current {
            get {
                var items = HttpContext.Current.Items;

                if(items[QdtContextKey]==null) {
                    var context = new QDTContext(IoC.Resolve<IAdminService>());
                    items[QdtContextKey] = context;
                }

                return items[QdtContextKey] as QDTContext;
            }
        }
    }
}