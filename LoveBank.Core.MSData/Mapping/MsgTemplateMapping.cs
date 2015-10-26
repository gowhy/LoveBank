
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using LoveBank.P2B.Domain.Messages;

namespace LoveBank.Core.MSData.Mapping {
    public class MsgTemplateMapping:EntityTypeConfiguration<MsgTemplate> {
        public MsgTemplateMapping() {
            HasKey(o => o.ID);
            Ignore(o=>o.MsgType);
            Property(o => o.InnerMsgType).HasColumnName("MsgType");
            ToTable(DB.TPref("msg_template"));
        }
    }
}