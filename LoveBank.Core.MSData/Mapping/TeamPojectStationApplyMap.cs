using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class TeamPojectStationApplyMap : EntityTypeConfiguration<TeamPojectStationApply>
    {
        public TeamPojectStationApplyMap()
        {

            ToTable(DB.TPref("LoveBank_TeamPojectStationApply"));
        }
    }
}
