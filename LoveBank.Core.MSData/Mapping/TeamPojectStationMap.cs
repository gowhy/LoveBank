using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class TeamPojectStationMap : EntityTypeConfiguration<TeamPojectStation>
    {
        public TeamPojectStationMap()
        {
          
            ToTable(DB.TPref("LoveBank_TeamPojectStation"));
        }
    }
}
