// <copyright file="NotificationModelBuilder.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public static class LanguageModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateLanguageModel(modelBuilder);
            CreateRoleTranslationModel(modelBuilder);
            CreateNotificationTypeTranslationModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateLanguageModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasKey(m => m.Id);
            modelBuilder.Entity<Language>().Property(m => m.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Language>().Property(m => m.Name).HasMaxLength(50);

            modelBuilder.Entity<Language>().HasData(new Language { Id = 1, Code = "EN", Name = "English" });
            modelBuilder.Entity<Language>().HasData(new Language { Id = 2, Code = "FR", Name = "Français" });
            modelBuilder.Entity<Language>().HasData(new Language { Id = 3, Code = "ES", Name = "Española" });
            modelBuilder.Entity<Language>().HasData(new Language { Id = 4, Code = "DE", Name = "Deutsch" });
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateRoleTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<RoleTranslation>().Property(r => r.RoleId).IsRequired();
            modelBuilder.Entity<RoleTranslation>().Property(r => r.LanguageId).IsRequired();
            // Begin BIADemo
            if (false)
            {
            // End BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 2, Id = 101, Label = "Administrateur du site" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 3, Id = 102, Label = "Administrador del sitio" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 4, Id = 103, Label = "Seitenadministrator" });
            // Begin BIADemo
            }

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 2, Id = 101, Label = "Administrateur de la compagnie" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 3, Id = 102, Label = "Administrador de la aerolínea" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 1, LanguageId = 4, Id = 103, Label = "Fluglinienadministrator" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 2, LanguageId = 2, Id = 201, Label = "Pilote" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 2, LanguageId = 3, Id = 202, Label = "Piloto" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = 2, LanguageId = 4, Id = 203, Label = "Pilot" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateNotificationTypeTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTypeTranslation>().HasKey(r => r.Id);
            modelBuilder.Entity<NotificationTypeTranslation>().Property(r => r.NotificationTypeId).IsRequired();
            modelBuilder.Entity<NotificationTypeTranslation>().Property(r => r.LanguageId).IsRequired();

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 1, LanguageId = 2, Id = 101, Label = "Tâche" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 1, LanguageId = 3, Id = 102, Label = "Tarea" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 1, LanguageId = 4, Id = 103, Label = "Aufgabe" });

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 2, LanguageId = 2, Id = 201, Label = "Information" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 2, LanguageId = 3, Id = 202, Label = "Información" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 2, LanguageId = 4, Id = 203, Label = "Information" });

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 3, LanguageId = 2, Id = 301, Label = "Succès" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 3, LanguageId = 3, Id = 302, Label = "Éxito" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 3, LanguageId = 4, Id = 303, Label = "Erfolg" });

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 4, LanguageId = 2, Id = 401, Label = "Avertissement" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 4, LanguageId = 3, Id = 402, Label = "Advertencia" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 4, LanguageId = 4, Id = 403, Label = "Erwärmen" });

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 5, LanguageId = 2, Id = 501, Label = "Erreur" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 5, LanguageId = 3, Id = 502, Label = "Culpa" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = 5, LanguageId = 4, Id = 503, Label = "Fehler" });
        }
    }
}