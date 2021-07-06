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

            modelBuilder.Entity<User>().HasIndex(u => new { u.Login, u.Domain }).IsUnique();
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.LabelEn).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Role>().Property(r => r.LabelFr).HasMaxLength(100);
            modelBuilder.Entity<Role>().Property(r => r.LabelEs).HasMaxLength(100);
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, LabelEn = "Site Admin", Code = "Site_Admin" });
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