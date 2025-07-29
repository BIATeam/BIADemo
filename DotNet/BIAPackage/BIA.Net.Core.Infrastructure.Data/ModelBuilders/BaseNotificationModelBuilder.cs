// <copyright file="BaseNotificationModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Notification.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public abstract class BaseNotificationModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public virtual void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateNotificationModel(modelBuilder);
            this.CreateNotificationTypeModel(modelBuilder);
            this.CreateNotificationUserModel(modelBuilder);
            this.CreateNotificationTeamModel(modelBuilder);
            this.CreateNotificationTeamRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseNotification>().HasKey(m => m.Id);
            modelBuilder.Entity<BaseNotification>().Property(m => m.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<BaseNotification>().Property(m => m.Description).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<BaseNotification>().Property(m => m.TypeId).IsRequired();
            modelBuilder.Entity<BaseNotification>().Property(m => m.CreatedDate).IsRequired();
        }

        /// <summary>
        /// Create the model for notification types.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationType>().HasKey(nt => nt.Id);
            modelBuilder.Entity<NotificationType>().Property(nt => nt.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<NotificationType>().Property(r => r.Label).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)BiaNotificationTypeId.Task, Code = "task", Label = "Task" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)BiaNotificationTypeId.Info, Code = "info", Label = "Info" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)BiaNotificationTypeId.Success, Code = "success", Label = "Success" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)BiaNotificationTypeId.Warning, Code = "warn", Label = "Warn" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)BiaNotificationTypeId.Error, Code = "error", Label = "Error" });
        }

        /// <summary>
        /// Create the model for notification users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationUser>().HasKey(nu => new { nu.UserId, nu.NotificationId });
            modelBuilder.Entity<NotificationUser>().HasOne(nu => nu.User).WithMany(u => u.NotificationUsers).HasForeignKey(nu => nu.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationUser>().HasOne(nu => nu.Notification).WithMany(n => n.NotifiedUsers).HasForeignKey(nu => nu.NotificationId);
        }

        /// <summary>
        /// Create the model for notification teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTeamModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTeam>().HasKey(nt => nt.Id);
            modelBuilder.Entity<NotificationTeam>().HasOne(nt => nt.Team).WithMany(u => u.NotificationTeams).HasForeignKey(nt => nt.TeamId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationTeam>().HasOne(nt => nt.Notification).WithMany(n => n.NotifiedTeams).HasForeignKey(nt => nt.NotificationId);
            modelBuilder.Entity<NotificationTeam>().HasMany(nt => nt.Roles).WithOne(ntr => ntr.NotificationTeam).HasForeignKey(nt => nt.NotificationTeamId);
        }

        /// <summary>
        /// Create the model for notification team roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTeamRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTeamRole>().HasKey(ntr => new { ntr.NotificationTeamId, ntr.RoleId });
        }
    }
}