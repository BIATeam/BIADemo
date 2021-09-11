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
            CreateNotificationUserModel(modelBuilder);
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
            modelBuilder.Entity<Notification>().Property(m => m.Description).HasMaxLength(256);
            modelBuilder.Entity<Notification>().Property(m => m.TypeId).IsRequired();
            modelBuilder.Entity<Notification>().Property(m => m.CreatedDate).IsRequired();
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
        /// Create the model for notification users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationRole>().HasKey(nu => new { RoleId = nu.RoleId, NotificationId = nu.NotificationId });
            modelBuilder.Entity<NotificationRole>().HasOne(nu => nu.Role).WithMany(u => u.NotificationRoles).HasForeignKey(nu => nu.RoleId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<NotificationRole>().HasOne(nu => nu.Notification).WithMany(n => n.NotifiedRoles).HasForeignKey(nu => nu.NotificationId);
        }
    }
}