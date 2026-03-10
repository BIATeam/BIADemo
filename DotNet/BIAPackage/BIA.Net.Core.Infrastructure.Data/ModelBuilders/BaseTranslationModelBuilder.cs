// <copyright file="BaseTranslationModelBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Domain.Translation.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public class BaseTranslationModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public virtual void CreateModel(ModelBuilder modelBuilder)
        {
            this.CreateLanguageModel(modelBuilder);
            this.CreateLanguageModelData(modelBuilder);
            this.CreateRoleTranslationModel(modelBuilder);
            this.CreateRoleTranslationModelData(modelBuilder);
            this.CreateNotificationTypeTranslationModel(modelBuilder);
            this.CreateNotificationTypeTranslationModelData(modelBuilder);
            this.CreateNotificationTranslationModel(modelBuilder);
            this.CreateNotificationTranslationModelData(modelBuilder);
            this.CreateAnnouncementTypeTranslationModel(modelBuilder);
            this.CreateAnnouncementTypeTranslationModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateLanguageModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasKey(m => m.Id);
            modelBuilder.Entity<Language>().Property(m => m.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Language>().Property(m => m.Name).HasMaxLength(50);

            modelBuilder.Entity<Language>().HasIndex(u => new { u.Code }).IsUnique();
        }

        /// <summary>
        /// Creates the language model data.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateLanguageModelData(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateRoleTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<RoleTranslation>().Property(r => r.RoleId).IsRequired();
            modelBuilder.Entity<RoleTranslation>().Property(r => r.LanguageId).IsRequired();
            modelBuilder.Entity<RoleTranslation>().Property(r => r.Label).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<RoleTranslation>().HasIndex(u => new { u.RoleId, u.LanguageId }).IsUnique();
        }

        /// <summary>
        /// Create model data for role translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateRoleTranslationModelData(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTypeTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTypeTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<NotificationTypeTranslation>().Property(r => r.NotificationTypeId).IsRequired();
            modelBuilder.Entity<NotificationTypeTranslation>().Property(r => r.LanguageId).IsRequired();
            modelBuilder.Entity<NotificationTypeTranslation>().HasIndex(u => new { u.NotificationTypeId, u.LanguageId }).IsUnique();

            modelBuilder.Entity<NotificationTypeTranslation>().Property(r => r.Label).IsRequired().HasMaxLength(50);
        }

        /// <summary>
        /// Create model data for notification type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTypeTranslationModelData(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<NotificationTranslation>().Property(r => r.NotificationId).IsRequired();
            modelBuilder.Entity<NotificationTranslation>().Property(r => r.LanguageId).IsRequired();
            modelBuilder.Entity<NotificationTranslation>().HasIndex(u => new { u.NotificationId, u.LanguageId }).IsUnique();

            modelBuilder.Entity<NotificationTranslation>().Property(m => m.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<NotificationTranslation>().Property(m => m.Description).IsRequired().HasMaxLength(256);
        }

        /// <summary>
        /// Creates the notification translation model data.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateNotificationTranslationModelData(ModelBuilder modelBuilder)
        {
        }

        /// <summary>
        /// Create the model for announcement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateAnnouncementTypeTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementTypeTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<AnnouncementTypeTranslation>().Property(r => r.AnnouncementTypeId).IsRequired();
            modelBuilder.Entity<AnnouncementTypeTranslation>().Property(r => r.LanguageId).IsRequired();
            modelBuilder.Entity<AnnouncementTypeTranslation>().HasIndex(u => new { u.AnnouncementTypeId, u.LanguageId }).IsUnique();

            modelBuilder.Entity<AnnouncementTypeTranslation>().Property(m => m.Label).IsRequired().HasMaxLength(150);
        }

        /// <summary>
        /// Create model data for announcement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateAnnouncementTypeTranslationModelData(ModelBuilder modelBuilder)
        {
        }
    }
}