using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.MSData.Mapping
{
    public class LoveBank_AdMap : EntityTypeConfiguration<LoveBank_Ad>
    {
        public LoveBank_AdMap()
        {
            ToTable(DB.TPref("LoveBank_Ad"));
        }
    }
}
