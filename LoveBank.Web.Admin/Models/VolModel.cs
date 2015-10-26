using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoveBank.Core.Domain;

namespace LoveBank.Web.Admin.Models
{
    public class VolModel
    {
        public Vol Vol { get; set; }
        public Department Department { get; set; }
    }
}