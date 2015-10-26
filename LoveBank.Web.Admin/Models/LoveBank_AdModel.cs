using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class LoveBank_AdModel
    {
        public int ID { get; set; }
        public string Guid { get; set; }

        public string Title { get; set; }
        public DateTime AddTime { get; set; }
        public string LinkUrl { get; set; }

        public int AddUserId { get; set; }
        public FileType AdType { get; set; }

        /// <summary>
        /// 机器唯一编码，一体机通过该编码找到属于该机器的广告信息
        /// </summary>
        public string MachineCode { get; set; }
        public int MachineId { get; set; }

        public Machine Machine { get; set; }
        /// <summary>
        /// 添加人所属社区
        /// </summary>
        public string DeptId { get; set; }

        public Department Department { get; set; }
        public string Desc { get; set; }

        /// <summary>
        ///数据状态
        /// </summary>
        public RowState State { get; set; }
        public dynamic AppSourceFileList { get; set; }
        /// <summary>
        /// 资源文件
        /// </summary>
        public virtual IList<SourceFile> SourceFileList { get; set; }
    }
}