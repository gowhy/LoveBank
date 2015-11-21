using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
  public  class AppVer
    {

        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string HttpUrl { get; set; }
        public string Ver { get; set; }
        public string Des { get; set; }
        public int State { get; set; }
        public AppVerType Type { get; set; }
    }
}
