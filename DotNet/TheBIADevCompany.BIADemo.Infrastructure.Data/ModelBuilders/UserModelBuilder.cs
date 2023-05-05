// <copyright file="UserModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public static class UserModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateMemberModel(modelBuilder);
            CreateUserModel(modelBuilder);
            CreateRoleModel(modelBuilder);
            CreateUserRoleModel(modelBuilder);
            CreateMemberRoleModel(modelBuilder);
            CreateTeamTypeModel(modelBuilder);
            CreateTeamTypeRoleModel(modelBuilder);
            CreateTeamModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMemberModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasKey(m => m.Id);
            modelBuilder.Entity<Member>().Property(m => m.TeamId).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.UserId).IsRequired();

            modelBuilder.Entity<Member>().HasOne(m => m.Team).WithMany(s => s.Members).HasForeignKey(m => m.TeamId);
            modelBuilder.Entity<Member>().HasOne(m => m.User).WithMany(u => u.Members).HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Member>().HasIndex(u => new { u.TeamId, u.UserId }).IsUnique();
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.DistinguishedName).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<User>().Property(u => u.IsEmployee);
            modelBuilder.Entity<User>().Property(u => u.IsExternal);
            modelBuilder.Entity<User>().Property(u => u.ExternalCompany).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Company).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Site).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Manager).HasMaxLength(250);
            modelBuilder.Entity<User>().Property(u => u.Department).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.SubDepartment).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Office).HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.Country).HasMaxLength(10);
            modelBuilder.Entity<User>().Property(u => u.DaiDate).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Guid).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsActive).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Domain).IsRequired().HasDefaultValue("--");
            modelBuilder.Entity<User>().Property(u => u.Sid).IsRequired().HasDefaultValue("--");

            modelBuilder.Entity<User>().HasIndex(u => new { u.Login }).IsUnique();
        }

        /// <summary>
        ///  Create the model for user role.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateUserRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                    .HasMany(p => p.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(mc =>
                    {
                        mc.ToTable("UserRoles");
                    });
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t => t.Title).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Team>().Property(u => u.TeamTypeId).IsRequired().HasDefaultValue(TeamTypeId.Site);
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateTeamTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamType>().HasKey(t => t.Id);
            modelBuilder.Entity<TeamType>().Property(r => r.Name).IsRequired().HasMaxLength(32);
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.Root, Name = "Root" });
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.Site, Name = "Site" });

            // Begin BIADemo
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.AircraftMaintenanceCompany, Name = "AircraftMaintenanceCompany" });
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.MaintenanceTeam, Name = "MaintenanceTeam" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Code).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Role>().Property(r => r.Label).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Admin, Code = "Admin", Label = "Administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.BackAdmin, Code = "Back_Admin", Label = "Background task administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.BackReadOnly, Code = "Back_Read_Only", Label = "Visualization of background tasks" });

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
            // End BIADemo
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Site administrator" });

            // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Airline administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Pilot, Code = "Pilot", Label = "Pilot" });

            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Supervisor, Code = "Supervisor", Label = "Supervisor" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Expert, Code = "Expert", Label = "Expert" });

            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.TeamLeader, Code = "Team_Leader", Label = "Team leader" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Operator, Code = "Operator", Label = "Operator" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateTeamTypeRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(p => p.TeamTypes)
                .WithMany(r => r.Roles)
                .UsingEntity(rt =>
                {
                    rt.ToTable("RoleTeamTypes");
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Root, RolesId = (int)RoleId.Admin });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Root, RolesId = (int)RoleId.BackAdmin });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Root, RolesId = (int)RoleId.BackReadOnly });

                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.SiteAdmin });

                    // Begin BIADemo
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.Pilot });

                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Supervisor });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Expert });

                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.TeamLeader });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Operator });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Expert });

                    // End BIADemo
                });
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMemberRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberRole>().HasKey(mr => new { mr.MemberId, mr.RoleId });
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Member).WithMany(m => m.MemberRoles).HasForeignKey(mr => mr.MemberId);
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Role).WithMany(m => m.MemberRoles).HasForeignKey(mr => mr.RoleId);
        }
    }
}