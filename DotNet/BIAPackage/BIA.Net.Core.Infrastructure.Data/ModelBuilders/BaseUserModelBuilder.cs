// <copyright file="BaseUserModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public abstract class BaseUserModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public virtual void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateMemberModel(modelBuilder);
            this.CreateUserModel(modelBuilder);
            this.CreateRoleModel(modelBuilder);
            this.CreateUserRoleModel(modelBuilder);
            this.CreateMemberRoleModel(modelBuilder);
            this.CreateTeamTypeModel(modelBuilder);
            this.CreateTeamTypeRoleModel(modelBuilder);
            this.CreateTeamModel(modelBuilder);
            this.CreateUserDefaultTeamModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateMemberModel(ModelBuilder modelBuilder)
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
        protected virtual void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntityUser>().ToTable("Users");
            modelBuilder.Entity<BaseEntityUser>().HasKey(u => u.Id);
            modelBuilder.Entity<BaseEntityUser>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<BaseEntityUser>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<BaseEntityUser>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<BaseEntityUser>().Property(u => u.LastSyncDate).IsRequired();
            modelBuilder.Entity<BaseEntityUser>().Property(u => u.IsActive).IsRequired();

            modelBuilder.Entity<BaseEntityUser>().HasIndex(u => new { u.Login }).IsUnique();
        }

        /// <summary>
        ///  Create the model for user role.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateUserRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntityUser>()
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
        protected virtual void CreateTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseEntityTeam>().ToTable("Teams");
            modelBuilder.Entity<BaseEntityTeam>().HasKey(t => t.Id);
            modelBuilder.Entity<BaseEntityTeam>().Property(t => t.Title).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<BaseEntityTeam>().Property(u => u.TeamTypeId).IsRequired();
        }

        /// <summary>
        /// Create the model for teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateTeamTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamType>().HasKey(t => t.Id);
            modelBuilder.Entity<TeamType>().Property(r => r.Name).IsRequired().HasMaxLength(32);
            modelBuilder.Entity<TeamType>().HasData(new TeamType { Id = (int)BiaTeamTypeId.Root, Name = "Root" });
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Code).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Role>().Property(r => r.Label).IsRequired().HasMaxLength(50);
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateTeamTypeRoleModel(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateMemberRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberRole>().HasKey(mr => new { mr.MemberId, mr.RoleId });
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Member).WithMany(m => m.MemberRoles).HasForeignKey(mr => mr.MemberId);
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Role).WithMany(m => m.MemberRoles).HasForeignKey(mr => mr.RoleId);
        }

        /// <summary>
        /// Create the model for user default teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateUserDefaultTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDefaultTeam>().HasIndex(udt => new { udt.UserId, udt.TeamId }).IsUnique();
        }
    }
}