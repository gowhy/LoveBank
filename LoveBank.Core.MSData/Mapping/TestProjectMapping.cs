using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

namespace LoveBank.Core.MSData.Mapping
{
    public class TestProjectMapping : EntityTypeConfiguration<TestProduct>
    {
        public TestProjectMapping()
        {
            ToTable("TestProduct");
        }
    }
}
