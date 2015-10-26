using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain.Enums
{
    /// <summary>
    /// 数据库中记录状态
    /// 表示记录是否被删除或者停用等状态
    /// </summary>
    public enum RowState
    {
        有效 = 0,
        删除 = 1,   
        停用 = 2
    }
}
