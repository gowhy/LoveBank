using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LoveBank.Web.Admin.Models
{
    public class Test
    {

        //[Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public LoveBank.Common.IPagedList<TestProduct> list;
    }
}