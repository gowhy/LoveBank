using System.Linq;
using LoveBank.Common;
using LoveBank.Core;
using LoveBank.Core.Domain;

namespace LoveBank.Services.LogMoudle
{
    public class AdminLogService : ServiceBase, IAdminLogService
    {
        public void AddAdminLog(AdminUser admin, string logInfo, string bussiness, string ip)
        {
            var log = new AdminLog(logInfo, admin==null? 0 : admin.ID, bussiness, ip);
            DbProvider.Add(log);
            DbProvider.SaveChanges();
        }

        public IPagedList<AdminLog> GetAdminLog(int pageNumber, int pageSize)
        {
            return DbProvider.Repository<AdminLog>()
                             .All()
                             .OrderByDescending(x => x.ID)
                             .ToPagedList(pageNumber-1, pageSize);
        }
    }
}
