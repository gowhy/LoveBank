using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.MSData.Mapping
{


    public class ConvenienceServicesMap : EntityTypeConfiguration<ConvenienceServices>
    {

        public ConvenienceServicesMap()
        {
            ToTable("convenienceservices");
        }

    }
}
