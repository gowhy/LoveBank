using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;

namespace LoveBank.Core.MSData.Mapping
{
    public class AdminLogMapping : EntityTypeConfiguration<AdminLog>
    {

        public AdminLogMapping()
        {
            HasKey(x => x.ID);
            Property(x => x.LogInfo).HasColumnName("Log_Info");
            Property(x => x.LogTime).HasColumnName("Log_Time");
            Property(x => x.AdminId).HasColumnName("Admin_Id");
            ToTable(DB.TPref("admin_log"));
        }

    }
}
