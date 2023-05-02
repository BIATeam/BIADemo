// <copyright file="UserModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
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
            CreatePermissionRoleModel(modelBuilder);
            CreatePermissionModel(modelBuilder);
            CreateMemberRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMemberModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasKey(m => m.Id);
            modelBuilder.Entity<Member>().Property(m => m.SiteId).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.UserId).IsRequired();

            modelBuilder.Entity<Member>().HasOne(m => m.Site).WithMany(s => s.Members).HasForeignKey(m => m.SiteId);
            modelBuilder.Entity<Member>().HasOne(m => m.User).WithMany(u => u.Members).HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Member>().HasIndex(u => new { u.SiteId, u.UserId }).IsUnique();
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
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Code).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Role>().Property(r => r.Label).IsRequired().HasMaxLength(50);

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
            // End BIADemo
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Code = "Site_Admin", Label = "Site administrator" });

            // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Code = "Site_Admin", Label = "Airline administrator" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Code = "Pilot", Label = "Pilot" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePermissionRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionRole>().HasKey(mr => new { mr.PermissionId, mr.RoleId });
            modelBuilder.Entity<PermissionRole>().HasOne(mr => mr.Permission).WithMany(m => m.PermissionRoles).HasForeignKey(mr => mr.PermissionId);
            modelBuilder.Entity<PermissionRole>().HasOne(mr => mr.Role).WithMany(m => m.PermissionRoles).HasForeignKey(mr => mr.RoleId);

            modelBuilder.Entity<PermissionRole>().HasData(new PermissionRole { PermissionId = 1, RoleId = 1 });

            // Begin BIADemo
            modelBuilder.Entity<PermissionRole>().HasData(new PermissionRole { PermissionId = 2, RoleId = 2 });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreatePermissionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasKey(r => r.Id);
            modelBuilder.Entity<Permission>().Property(r => r.Code).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Permission>().Property(r => r.Label).IsRequired().HasMaxLength(50);

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
            // End BIADemo
            modelBuilder.Entity<Permission>().HasData(new Permission { Id = 1, Code = "Site_Admin", Label = "Site administrator" });

            // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

            modelBuilder.Entity<Permission>().HasData(new Permission { Id = 1, Code = "Site_Admin", Label = "Airline administrator" });
            modelBuilder.Entity<Permission>().HasData(new Permission { Id = 2, Code = "Pilot", Label = "Pilot" });

            // End BIADemo
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