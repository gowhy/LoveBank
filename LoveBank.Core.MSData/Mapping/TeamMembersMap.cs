using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class TeamMembersMap : EntityTypeConfiguration<TeamMembers>
    {
        public TeamMembersMap()
        {

            ToTable(DB.TPref("LoveBank_TeamMembers"));
        }
    }
}
