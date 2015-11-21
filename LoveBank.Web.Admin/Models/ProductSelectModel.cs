using LoveBank.Common;
using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class ProductSelectModel
    {
        public string BarCode { get; set; }
        public string Name { get; set; }

        public IPagedList<ProductModel> ProductList { get; set; }
    }
}