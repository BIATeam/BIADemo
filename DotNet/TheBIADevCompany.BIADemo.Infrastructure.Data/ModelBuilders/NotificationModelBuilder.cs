// <copyright file="NotificationModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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
            CreateNotificationRoleModel(modelBuilder);
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
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = 1, Code = "task", Label = "Task" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = 2, Code = "info", Label = "Info" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = 3, Code = "success", Label = "Success" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = 4, Code = "warn", Label = "Warn" });
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType { Id = 5, Code = "error", Label = "Error" });
        }

        /// <summary>
        /// Create the model for notification roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationRole>().HasKey(nr => new { RoleId = nr.RoleId, NotificationId = nr.NotificationId });
            modelBuilder.Entity<NotificationRole>().HasOne(nr => nr.Role).WithMany(u => u.NotificationRoles).HasForeignKey(nr => nr.RoleId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationRole>().HasOne(nr => nr.Notification).WithMany(n => n.NotifiedRoles).HasForeignKey(nr => nr.NotificationId);
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
            modelBuilder.Entity<NotificationTeam>().HasKey(nu => new { TeamId = nu.TeamId, NotificationId = nu.NotificationId });
            modelBuilder.Entity<NotificationTeam>().HasOne(nu => nu.Team).WithMany(u => u.NotificationTeams).HasForeignKey(nu => nu.TeamId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationTeam>().HasOne(nu => nu.Notification).WithMany(n => n.NotifiedTeams).HasForeignKey(nu => nu.NotificationId);
        }
    }
}