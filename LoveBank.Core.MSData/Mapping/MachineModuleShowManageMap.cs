using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class MachineModuleShowManageMap : EntityTypeConfiguration<MachineModuleShowManage>
    {
        public MachineModuleShowManageMap()
        {
            //HasKey(x => x.ID);
         
            //Property(x => x.RoleID).HasColumnName("role_id");
       
            //Property(x => x.IsDefaultAdmin).HasColumnName("is_default");
            //Property(x => x.LoginTime).HasColumnName("login_time");
            //Property(x => x.LoginIP).HasColumnName("login_ip");

            ToTable(DB.TPref("MachineModuleShowManage"));
        }
    }
}
