using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveBank.Core.Domain
{
    public class RegionConfig : Entity
    {
        /// <summary>
        /// 父id
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地区等级
        /// </summary>
        public int RegionLevel { get; set; }
    }
}
