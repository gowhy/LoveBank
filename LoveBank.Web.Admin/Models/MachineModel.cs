using LoveBank.Common;
using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class MachineModel
    {
        public int Id { get; set; }
        //public Machine Machine { get; set; }
        //public Department Department { get; set; }
        public string Title { get; set; }
        public DateTime AddTime { get; set; }

        public int AddUserId { get; set; }

        /// <summary>
        /// 添加机器的人所属社区
        /// </summary>
        public string AddUserDeptId { get; set; }

        /// <summary>
        /// 机器唯一编码，一体机通过该编码找到属于该机器的广告信息
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 机器所属社区
        /// </summary>
        public string DeptId { get; set; }

        public string DeptIdName { get; set; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }
        /// <summary>
        ///数据状态
        /// </summary>
        public RowState State { get; set; }

        /// <summary>
        /// 备注，对机器的使用描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string Name { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        public int? ProductId { get; set; }

        public IPagedList<MachineModel> MachineModelList { get; set; }
    }
}