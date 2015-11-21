using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class SerTypes
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// Desc
        /// </summary>		
        public string Desc { get; set; }
        public int Type { get; set; }
    }
}
