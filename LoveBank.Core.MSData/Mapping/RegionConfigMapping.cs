using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class RegionConfigMapping : EntityTypeConfiguration<RegionConfig>
    {
        public RegionConfigMapping()
        {
            HasKey(x => x.ID);
            Property(x => x.RegionLevel).HasColumnName("region_level");

            ToTable(DB.TPref("region_conf"));
        }
    }
}
