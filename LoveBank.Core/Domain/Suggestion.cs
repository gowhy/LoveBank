using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class Suggestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DepId { get; set; }
        public string Content { get; set; }
        public int AddUser { get; set; }
        public string AddUserName { get; set; }
        public string AddUserPhone { get; set; }

        public string AddUserMachineCode { get; set; }

        public System.DateTime AddTime { get; set; }

    }
}
