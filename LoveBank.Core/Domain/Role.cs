using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace LoveBank.Core.Domain
{
    public class Role : Entity,IAggregeRoot
    {
        //public Role()
        //{
        //    Accesses = new List<RoleAccess>();
        //}
        public string Name { get; set; }

        public bool IsEffect { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<RoleAccess> Accesses { get; set; }
    }
}
