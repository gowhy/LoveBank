using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.MSData.Mapping
{
   public class DepartmentMap:EntityTypeConfiguration<Department>
    {
       public DepartmentMap()
       {
           HasKey(x=>x.Id);
           ToTable("Department");
       }
    }
}
