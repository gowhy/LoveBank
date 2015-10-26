using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using LoveBank.P2B.Domain.Messages;

namespace LoveBank.Core.MSData.Mapping
{
    public class MsgQueueMapping : EntityTypeConfiguration<MsgQueue>
    {
        public MsgQueueMapping() {

            HasKey(x => x.ID);

            ToTable(DB.TPref("msgqueue"));
        }
    }
}
