using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LoveBank.Common;
using LoveBank.Core.Domain;

namespace LoveBank.Web.Admin.Models
{
    public class AdminUserModel
    {
        public AdminUser AdminUser { get; set; }
        public Role Role { get; set; }
        public Department Department { get; set; }
    }

}