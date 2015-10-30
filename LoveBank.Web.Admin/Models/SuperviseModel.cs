using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class SuperviseModel
    {
        public int Id { get; set; }
        public string DeptId { get; set; }
        public string VDeptId { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public int AddUser { get; set; }
        public string DepName { get; set; }
        public Vol volEntity { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Msg { get; set; }
        public SuperviseRowState State { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 网格管理员
        /// </summary>
        public GridMember GridMember { get; set; }
    }
}