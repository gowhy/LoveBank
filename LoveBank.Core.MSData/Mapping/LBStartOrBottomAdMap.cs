using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.MSData.Mapping
{
    public class LBStartOrBottomAdMap : EntityTypeConfiguration<LBStartOrBottomAd>
    {
        public LBStartOrBottomAdMap()
        {
            ToTable(DB.TPref("LoveBank_StartOrBottomAd"));
        }
    }
}
