using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class MachineProduct
    {
        public int ID { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public int ProductId { get; set; }
        public int? MachineId { get; set; }
        public string MachineCode { get; set; }

    }
}
