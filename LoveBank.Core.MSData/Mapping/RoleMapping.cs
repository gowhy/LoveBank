using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class RoleMapping : EntityTypeConfiguration<Role>
    {
        public RoleMapping()
        {
            //HasKey(x => x.ID);
            //Property(x => x.IsEffect).HasColumnName("is_effect");
            //Property(x => x.IsDelete).HasColumnName("is_delete");
            //HasMany(x=>x.Accesses).WithOptional().HasForeignKey(x=>x.RoleId).WillCascadeOnDelete(true);
            ToTable(DB.TPref("loveBank_role"));
        }
    }


    public class RoleAccessMapping : EntityTypeConfiguration<RoleAccess>
    {
        public RoleAccessMapping()
        {
            //HasKey(x => x.ID);
            //Property(x => x.RoleId).HasColumnName("role_id");
            ToTable(DB.TPref("loveBank_rolepermission"));
        }
    }
}
