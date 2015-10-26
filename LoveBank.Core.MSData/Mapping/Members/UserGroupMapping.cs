using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Members;

namespace LoveBank.Core.MSData.Mapping.Members
{
    public class UserGroupMapping : EntityTypeConfiguration<UserGroup>
    {
        public UserGroupMapping()
        {
            HasKey(x => x.ID);

            Property(x => x.IsSystem).HasColumnName("is_system");

            ToTable(DB.TPref("user_group"));
        }
    }
}
