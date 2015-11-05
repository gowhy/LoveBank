

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
            modelBuilder.Configurations.Add(new TeamProjectSummaryMap());

            modelBuilder.Configurations.Add(new WorkGuideMap());
            modelBuilder.Configurations.Add(new NoticeMap());
            modelBuilder.Configurations.Add(new GridMemberMap());
            modelBuilder.Configurations.Add(new SuperviseMap());
            modelBuilder.Configurations.Add(new SuggestionMap());
            modelBuilder.Configurations.Add(new LoveBankProductExchangeLogMap());
            modelBuilder.Configurations.Add(new LBStartOrBottomAdMap());
            modelBuilder.Configurations.Add(new MachineStatisticsMap());

            modelBuilder.Configurations.Add(new MachineHeartbeatMap());
            modelBuilder.Configurations.Add(new LoginStatisticsMap());
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
        public virtual DbSet<TeamProjectSummary> T_TeamProjectSummary { get; set; }

        public virtual DbSet<WorkGuide> T_WorkGuide { get; set; }
        public virtual DbSet<Notice> T_Notice { get; set; }
        public virtual DbSet<GridMember> T_GridMember { get; set; }
        public virtual DbSet<Supervise> T_Supervise { get; set; }
        public virtual DbSet<Suggestion> T_Suggestion { get; set; }


        public virtual DbSet<LoveBankProductExchangeLog> T_LoveBankProductExchangeLogMap { get; set; }
        public virtual DbSet<LBStartOrBottomAd> T_LBStartOrBottomAd { get; set; }

        public virtual DbSet<MachineStatistics> T_MachineStatistics { get; set; }

        public virtual DbSet<MachineHeartbeat> T_MachineHeartbeat { get; set; }
        public virtual DbSet<LoginStatistics> T_LoginStatistics { get; set; }
        
    }
}
