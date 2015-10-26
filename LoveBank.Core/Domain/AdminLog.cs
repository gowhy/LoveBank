using System;

namespace LoveBank.Core.Domain
{
    public class AdminLog : Entity
    {
        /// <summary>
        /// 日志信息
        /// </summary>
        public string LogInfo { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// 相关联的业务
        /// </summary>
        public string Bussiness { get; set; }
        /// <summary>
        /// 记录的IP
        /// </summary>
        public string Ip { get; set; }

        public AdminLog()
        {
            LogTime = DateTime.Now;
        }

        public AdminLog(string loginfo, int adminId, string bussiness, string ip) :this()
        {
            LogInfo = loginfo;
            AdminId = adminId;
            Bussiness = bussiness;
            Ip = ip;
        }

    }
}
