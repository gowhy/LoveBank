
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using LoveBank.P2B.Domain.Messages;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveBank.Core.MSData.Mapping {
    public class SmServerMapping:EntityTypeConfiguration<SmsServer> {
        public SmServerMapping() {
            HasKey(o => o.ID);
            Property(o => o.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(o => o.ServerName).HasColumnName("name");
            Property(o => o.Description).HasColumnName("description");
            Property(o => o.ClassName).HasColumnName("class_name");
            Property(o => o.UserName).HasColumnName("user_name");
            Property(o => o.Password).HasColumnName("password");
            Property(o => o.ConfigInfo).HasColumnName("config");
            ToTable(DB.TPref("sms"));
        }
    }
}