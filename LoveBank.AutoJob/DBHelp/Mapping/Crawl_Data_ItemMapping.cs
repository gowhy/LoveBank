using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Domain;
using LoveBank.Core;

namespace LoveBank.AutoJob.DataLayer.Mapping
{
    public class Crawl_Data_ItemMapping : EntityTypeConfiguration<Crawl_Data_Item>
    {
        public Crawl_Data_ItemMapping()
        {
            //HasKey(x => x.ID);
            //Property(x => x.IsRec).HasColumnName("Is_Rec");

           // ToTable(DB.TPref("Crawl_Data_Item_Selector"));
        }
    }
}
