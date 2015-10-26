using LoveBank.Common;
using LoveBank.Core.Domain;

namespace LoveBank.Services.LogMoudle
{
    public interface IAdminLogService
    {
        /// <summary>
        /// 添加管理员日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        /// <param name="admin">管理员</param>
        /// <param name="bussiness">相关联的业务</param>
        /// <param name="ip">Ip</param>
        void AddAdminLog(AdminUser admin, string logInfo, string bussiness, string ip);

        /// <summary>
        /// 获取管理员日志
        /// </summary>
        /// <param name="pageNumber">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>管理员日志</returns>
        IPagedList<AdminLog> GetAdminLog(int pageNumber, int pageSize);
    }
}
