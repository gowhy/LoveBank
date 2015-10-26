

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveBank.Core.MSData.Mapping {
    public class MailServerMapping:EntityTypeConfiguration<MailServer> {
        public MailServerMapping() {
            HasKey(o => o.ID);
            Property(o => o.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(o => o.SmtpName).HasColumnName("smtp_name");
            Property(o => o.SmtpServer).HasColumnName("smtp_server");
            Property(o => o.SmtpPassword).HasColumnName("smtp_pwd");
            Property(o => o.IsSsl).HasColumnName("is_ssl");
            Property(o => o.SmtpPort).HasColumnName("smtp_port");
            Property(o => o.UseLimit).HasColumnName("use_limit");
            Property(o => o.IsReset).HasColumnName("is_reset");
            Property(o => o.IsEffect).HasColumnName("is_effect");
            Property(o => o.TotalUse).HasColumnName("total_use");
            Property(o => o.IsVerify).HasColumnName("is_verify");
            ToTable(DB.TPref("mail_server"));
        }
    }
}