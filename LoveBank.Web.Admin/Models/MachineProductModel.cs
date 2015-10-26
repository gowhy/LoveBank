using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class MachineProductModel
    {
        //public int ID { get; set; }
        //public int AddUserId { get; set; }
        //public DateTime AddTime { get; set; }

    
        public int MachineId { get; set; }
        public string MachineCode { get; set; }
        public string MachineTitle { get; set; }
        public string DeptId { get; set; }

        public int ProductId { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }

        public IList<Machine> MachineList { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}