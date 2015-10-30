using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
   public class Product:Entity
    {
        //public int Id { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public int CostScore { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string BarCode { get; set; }
        public string DeptId { get; set; }

        public string Guid { get; set; }

        public ProductType Type { get; set; }
    }
}
