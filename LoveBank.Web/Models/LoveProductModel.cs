using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Models
{
    public class LoveProductModel
    {
        public List<ProductModel> ProductList { get; set; }
        public List<ProductModel> ProductListHot { get; set; }
    }
}