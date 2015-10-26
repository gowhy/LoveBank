using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Members;

namespace LoveBank.Core.MSData.Mapping.Members
{
    public class UserAccountMapping : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountMapping()
        {
            HasKey(x => x.ID);

            Property(x => x.TimeStamp).IsConcurrencyToken();

            ToTable(DB.TPref("user_account"));
        }
    }
}
