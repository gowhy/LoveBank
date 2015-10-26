using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common;

namespace LoveBank.MVC.Security
{
    /// <summary>
    /// 模块信息
    /// </summary>
    public class Module
    {
       
        /// <summary>
        /// 模块的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模块的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 模块的Controller
        /// </summary>
        public string Controller { get; set; }
    }
}
