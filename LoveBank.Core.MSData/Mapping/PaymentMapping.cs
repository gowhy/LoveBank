using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Payments;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveBank.Core.MSData.Mapping
{
    public class PaymentMapping : EntityTypeConfiguration<PaymentInfo>
    {
        public PaymentMapping()
        {
            HasKey(x => x.ID);
            Property(o => o.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(o => o.ClassName).HasColumnName("class_name");
            Property(x => x.IsEffect).HasColumnName("is_effect");
            Property(x => x.OnlinePay).HasColumnName("online_pay");
            Property(x => x.FeeAmount).HasColumnName("fee_amount");
            Property(x => x.TotalAmount).HasColumnName("total_amount");
            Property(x => x.FeeType).HasColumnName("fee_type");
            Property(x => x.SerializarConfig).HasColumnName("config");

            ToTable(DB.TPref("payment"));
        }
    }
}
