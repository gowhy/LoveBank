using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class MachineModuleShowManage : Entity
    {
        //public int ID { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public int Sort { get; set; }
        public string DeptId { get; set; }
        public short Type { get; set; }

        public string IconUrl { get; set; }
        //public RowState State { get; set; }
        public string Guid { get; set; }
        public string ModuleKey { get; set; }

    }
}
