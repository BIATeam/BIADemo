// <copyright file="NotificationModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public static class NotificationModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateNotificationModel(modelBuilder);
            CreateNotificationTypeModel(modelBuilder);
            CreateNotificationUserModel(modelBuilder);
            CreateNotificationTeamModel(modelBuilder);
            CreateNotificationTeamRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().HasKey(m => m.Id);
            modelBuilder.Entity<Notification>().Property(m => m.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Notification>().Property(m => m.Description).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Notification>().Property(m => m.TypeId).IsRequired();
            modelBuilder.Entity<Notification>().Property(m => m.CreatedDate).IsRequired();
        }

        /// <summary>
        /// Create the model for notification types.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationTypeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationType>().HasKey(nt => nt.Id);
            modelBuilder.Entity<NotificationType>().Property(nt => nt.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<NotificationType>().Property(r => r.Label).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)NotificationTypeId.Task, Code = "task", Label = "Task" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)NotificationTypeId.Info, Code = "info", Label = "Info" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)NotificationTypeId.Success, Code = "success", Label = "Success" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)NotificationTypeId.Warning, Code = "warn", Label = "Warn" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = (int)NotificationTypeId.Error, Code = "error", Label = "Error" });
        }

        /// <summary>
        /// Create the model for notification users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationUser>().HasKey(nu => new { UserId = nu.UserId, NotificationId = nu.NotificationId });
            modelBuilder.Entity<NotificationUser>().HasOne(nu => nu.User).WithMany(u => u.NotificationUsers).HasForeignKey(nu => nu.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationUser>().HasOne(nu => nu.Notification).WithMany(n => n.NotifiedUsers).HasForeignKey(nu => nu.NotificationId);
        }

        /// <summary>
        /// Create the model for notification teams.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationTeamModel(ModelBuilder modelBuilder)
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
        private static void CreateNotificationTeamRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTeamRole>().HasKey(ntr => new { ntr.NotificationTeamId, ntr.RoleId });
        }
    }
}