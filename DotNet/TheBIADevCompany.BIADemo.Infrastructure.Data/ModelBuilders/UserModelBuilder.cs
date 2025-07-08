// <copyright file="UserModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public class UserModelBuilder : BaseUserModelBuilder
    {
        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateUserModel(ModelBuilder modelBuilder)
        {
            base.CreateUserModel(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Email).HasMaxLength(256);
#if BIA_USER_CUSTOM_FIELDS_BACK
                entity.Property(u => u.DistinguishedName).IsRequired().HasMaxLength(250);
                entity.Property(u => u.IsEmployee);
                entity.Property(u => u.IsExternal);
                entity.Property(u => u.ExternalCompany).HasMaxLength(50);
                entity.Property(u => u.Company).HasMaxLength(50);
                entity.Property(u => u.Site).HasMaxLength(50);
                entity.Property(u => u.Manager).HasMaxLength(250);
                entity.Property(u => u.Department).HasMaxLength(50);
                entity.Property(u => u.SubDepartment).HasMaxLength(50);
                entity.Property(u => u.Office).HasMaxLength(20);
                entity.Property(u => u.Country).HasMaxLength(10);
#endif
            });
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeModel(modelBuilder);

            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.Site, Name = "Site" });

            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial TeamTypeModelBuilder AircraftMaintenanceCompany
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.AircraftMaintenanceCompany, Name = "AircraftMaintenanceCompany" });

            // BIAToolKit - End Partial TeamTypeModelBuilder AircraftMaintenanceCompany
            // BIAToolKit - Begin Partial TeamTypeModelBuilder MaintenanceTeam
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)TeamTypeId.MaintenanceTeam, Name = "MaintenanceTeam" });

            // BIAToolKit - End Partial TeamTypeModelBuilder MaintenanceTeam
            // End BIAToolKit Generation Ignore

            // BIAToolKit - Begin TeamTypeModelBuilder
            // BIAToolKit - End TeamTypeModelBuilder
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateRoleModel(modelBuilder);

            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)BiaRoleId.Admin, Code = "Admin", Label = "Administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)BiaRoleId.BackAdmin, Code = "Back_Admin", Label = "Background task administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)BiaRoleId.BackReadOnly, Code = "Back_Read_Only", Label = "Visualization of background tasks" });

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
                // DO NOT CHANGE INDENTATION (For BIATemplate)
                // End BIADemo
                modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Site administrator" });

                // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.SiteAdmin, Code = "Site_Admin", Label = "Airline administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Pilot, Code = "Pilot", Label = "Pilot" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Supervisor, Code = "Supervisor", Label = "Supervisor" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Expert, Code = "Expert", Label = "Expert" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.Operator, Code = "Operator", Label = "Operator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.TeamLeader, Code = "Team_Leader", Label = "Team leader" });

            // End BIADemo
            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial RoleModelBuilder AircraftMaintenanceCompany
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.AircraftMaintenanceCompanyAdmin, Code = "AircraftMaintenanceCompany_Admin", Label = "AircraftMaintenanceCompany administrator" });

            // BIAToolKit - End Partial RoleModelBuilder AircraftMaintenanceCompany
            // BIAToolKit - Begin Partial RoleModelBuilder MaintenanceTeam
            modelBuilder.Entity<Role>().HasData(new Role { Id = (int)RoleId.MaintenanceTeamAdmin, Code = "MaintenanceTeam_Admin", Label = "MaintenanceTeam administrator" });

            // BIAToolKit - End Partial RoleModelBuilder MaintenanceTeam
            // End BIAToolKit Generation Ignore

            // BIAToolKit - Begin RoleModelBuilder
            // BIAToolKit - End RoleModelBuilder
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateTeamTypeRoleModel(ModelBuilder modelBuilder)
        {
            base.CreateTeamTypeRoleModel(modelBuilder);

            modelBuilder.Entity<Role>()
                .HasMany(p => p.TeamTypes)
                .WithMany(r => r.Roles)
                .UsingEntity(rt =>
                {
                    rt.ToTable("RoleTeamTypes");
                    rt.HasData(new { TeamTypesId = (int)BiaTeamTypeId.Root, RolesId = (int)BiaRoleId.Admin });
                    rt.HasData(new { TeamTypesId = (int)BiaTeamTypeId.Root, RolesId = (int)BiaRoleId.BackAdmin });
                    rt.HasData(new { TeamTypesId = (int)BiaTeamTypeId.Root, RolesId = (int)BiaRoleId.BackReadOnly });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.SiteAdmin });

                    // Begin BIADemo
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.Site, RolesId = (int)RoleId.Pilot });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Supervisor });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.Expert });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Operator });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.Expert });
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.TeamLeader });

                    // End BIADemo
                    // Begin BIAToolKit Generation Ignore
                    // BIAToolKit - Begin Partial TeamTypeRoleModelBuilder AircraftMaintenanceCompany
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.AircraftMaintenanceCompany, RolesId = (int)RoleId.AircraftMaintenanceCompanyAdmin });

                    // BIAToolKit - End Partial TeamTypeRoleModelBuilder AircraftMaintenanceCompany
                    // BIAToolKit - Begin Partial TeamTypeRoleModelBuilder MaintenanceTeam
                    rt.HasData(new { TeamTypesId = (int)TeamTypeId.MaintenanceTeam, RolesId = (int)RoleId.MaintenanceTeamAdmin });

                    // BIAToolKit - End Partial TeamTypeRoleModelBuilder MaintenanceTeam
                    // End BIAToolKit Generation Ignore

                    // BIAToolKit - Begin TeamTypeRoleModelBuilder
                    // BIAToolKit - End TeamTypeRoleModelBuilder
                });
        }
    }
}