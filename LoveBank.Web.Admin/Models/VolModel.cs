using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Core.Domain;
using LoveBank.Common;

namespace LoveBank.Web.Admin.Models
{
    public class VolModel
    {
     
        public Vol Vol { get; set; }
        public Department Department { get; set; }
    }

    public class VolModelSelect
    {
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string NFC { get; set; }
        public IPagedList<VolModel> VolList { get; set; }
    } 
}