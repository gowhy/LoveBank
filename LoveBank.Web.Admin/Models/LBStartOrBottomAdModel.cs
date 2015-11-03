using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class LBStartOrBottomAdModel
    {
        public int ID { get; set; }
        public string Guid { get; set; }

        public string Title { get; set; }
        public DateTime AddTime { get; set; }
        public string LinkUrl { get; set; }

        public int AddUserId { get; set; }

     
        public string DeptId { get; set; }

        public Department Department { get; set; }
        public string Desc { get; set; }

        public string AddUserDeptId { get; set; }

        public string HttpImgUrl { get; set; }
        /// <summary>
        ///数据状态
        /// </summary>
        public RowState State { get; set; }
        public LBStartOrBottomAdPostion Postion { get; set; }
        /// <summary>
        /// 资源文件
        /// </summary>
        public virtual IList<SourceFile> SourceFileList { get; set; }
    }
}