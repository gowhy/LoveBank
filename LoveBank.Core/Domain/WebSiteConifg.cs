using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
   public class WebSiteConifg:Entity
    {
       public string Domain { get; set; }
       public string DeptId { get; set; }
       public string DeptIdName { get; set; }
    }
}
