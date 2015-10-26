using System.Linq;
using System.Web.Mvc;
using LoveBank.Core.Domain;

namespace LoveBank.Web.Controllers
{
    public class RegionController : BaseController
    {
        [OutputCache(Duration = 3600)]
        public ActionResult Region(int pid)
        {
            var regions = DbProvider.Repository<RegionConfig>().Filter(x => x.Pid == pid).ToList();
            return Json(new { total = regions.Count, rows = regions }, JsonRequestBehavior.AllowGet);
        }

   
    }
}
