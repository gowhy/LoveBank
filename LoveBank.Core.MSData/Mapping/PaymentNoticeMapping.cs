using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Payments;

namespace LoveBank.Core.MSData.Mapping
{
    public class PaymentNoticeMapping : EntityTypeConfiguration<PaymentNotice>
    {
        public PaymentNoticeMapping()
        {
            HasKey(x => x.ID);

            ToTable(DB.TPref("payment_notice"));
        }
    }
}
