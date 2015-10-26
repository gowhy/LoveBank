using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class TeamProjectMap : EntityTypeConfiguration<TeamProject>
    {
        public TeamProjectMap()
        {

            ToTable(DB.TPref("LoveBank_TeamProject"));
        }
    }
}
