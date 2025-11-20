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
            this.CreateRoleTranslationModel(modelBuilder);
            this.CreateNotificationTypeTranslationModel(modelBuilder);
            this.CreateNotificationTranslationModel(modelBuilder);
            this.CreateAnnoucementTypeTranslationModel(modelBuilder);
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
        /// Create the model for annoucement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected virtual void CreateAnnoucementTypeTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnoucementTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<AnnoucementTranslation>().Property(r => r.AnnoucementTypeId).IsRequired();
            modelBuilder.Entity<AnnoucementTranslation>().Property(r => r.LanguageId).IsRequired();
            modelBuilder.Entity<AnnoucementTranslation>().HasIndex(u => new { u.AnnoucementTypeId, u.LanguageId }).IsUnique();

            modelBuilder.Entity<AnnoucementTranslation>().Property(m => m.Label).IsRequired().HasMaxLength(150);
        }
    }
}