using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class Department 
    {


        public string Id { get; set; }
        public string Ids { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PId { set; get; }
        public string PIds { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
        public string Names { set; get; }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime AddTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? State { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Desc { set; get; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsCheck { get; set; }

        //[System.ComponentModel.DataAnnotations.Schema.NotMapped]
        //public bool  checked { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public Nullable<double> Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public Nullable<double> Lng { get; set; }

        public int Level { get; set; }
    }
}
