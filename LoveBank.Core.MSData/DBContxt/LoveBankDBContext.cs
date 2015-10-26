﻿

using LoveBank.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoveBank.Core.MSData.Mapping.Members;
using LoveBank.Core.MSData.Mapping;
using LoveBank.Core.Members;
using LoveBank.Core.Domain;
namespace LoveBank.Core.MSData
{
    public partial class LoveBankDBContext : BaseContext
    {
        public LoveBankDBContext()
            : base("conn_love_bank_db")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new RoleMapping());
            modelBuilder.Configurations.Add(new AdminMapping());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new MachineMap());
            modelBuilder.Configurations.Add(new LoveBank_AdMap());
            modelBuilder.Configurations.Add(new SourceFileMap());
            modelBuilder.Configurations.Add(new VolMap());
            modelBuilder.Configurations.Add(new VolAddScoreRecordeMap());
            modelBuilder.Configurations.Add(new MachineModuleShowManageMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new MachineProductMap());

            modelBuilder.Configurations.Add(new TeamMap());
            modelBuilder.Configurations.Add(new TeamProjectMap());
            modelBuilder.Configurations.Add(new TeamMembersMap());
            modelBuilder.Configurations.Add(new TeamPojectStationApplyMap());
            modelBuilder.Configurations.Add(new TeamPojectStationMap());

        


        }
        //public virtual DbSet<User> T_User { get; set; }
        public virtual DbSet<Role> T_Role { get; set; }
        public virtual DbSet<AdminUser> T_AdminUser { get; set; }
        public virtual DbSet<Department> T_Department { get; set; }
        public virtual DbSet<Machine> T_Machine { get; set; }
        public virtual DbSet<LoveBank_Ad> T_LoveBank_Ad { get; set; }
        public virtual DbSet<SourceFile> T_SourceFile { get; set; }
        public virtual DbSet<Vol> T_Vol { get; set; }
        public virtual DbSet<VolAddScoreRecorde> T_VolAddScoreRecorde { get; set; }
        public virtual DbSet<MachineModuleShowManage> T_MachineModuleShowManage { get; set; }
        public virtual DbSet<Product> T_Product { get; set; }
        public virtual DbSet<MachineProduct> T_MachineProduct { get; set; }
        public virtual DbSet<TeamProject> T_TeamProject { get; set; }

        public virtual DbSet<Team> T_Team { get; set; }
        public virtual DbSet<TeamMembers> T_TeamMembers { get; set; }
        public virtual DbSet<TeamPojectStationApply> T_TeamPojectStationApply { get; set; }
        public virtual DbSet<TeamPojectStation> T_TeamPojectStation { get; set; }

    }
}