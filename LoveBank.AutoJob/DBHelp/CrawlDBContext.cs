
using LoveBank.AutoJob.DataLayer.Mapping;
using LoveBank.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.AutoJob.DataLayer
{
    public partial class CrawlDBContext : BaseContext
    {
        public CrawlDBContext()
            : base("Conn_Crawl_db")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 禁用多对多关系表的级联删除
            //modelBuilder.Entity<TestMap>();
            modelBuilder.Configurations.Add(new Crawl_Data_Item_SelectorMapping());
            modelBuilder.Configurations.Add(new Crawl_Data_ItemMapping());
          
            // modelBuilder.Entity<VolunteerEntity>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


        }


        /// <summary>
        /// 后台用户管理表
        /// </summary>
        public virtual DbSet<Crawl_Data_Item_Selector> DBSet_Crawl_Data_Item_Selector { get; set; }
        public virtual DbSet<Crawl_Data_Item> DBSet_Crawl_Data_Item { get; set; }

        /// <summary>
     
          
    }
}
