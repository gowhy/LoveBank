using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class ReturnEntity
    {
        /// <summary>
        /// 执行结果,返回true表示执行政策
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 返回状态值,用于逻辑判断
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 返回的提示文本,用于描述执行结果
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回结果需要的传递是实体参数
        /// </summary>
        public object Body { get; set; }

    }
}
