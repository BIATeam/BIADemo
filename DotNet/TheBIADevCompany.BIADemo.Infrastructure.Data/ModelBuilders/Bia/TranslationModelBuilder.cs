// <copyright file="TranslationModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public partial class TranslationModelBuilder : BaseTranslationModelBuilder
    {
        private static void BiaCreateLanguageModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = LanguageId.English, Code = "EN", Name = "English" },
                new Language { Id = LanguageId.French, Code = "FR", Name = "Français" },
                new Language { Id = LanguageId.Spanish, Code = "ES", Name = "Española" });
        }

        private static void BiaCreateRoleTranslationModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleTranslation>().HasData(
                new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.French, Id = 1000101, Label = "Administrateur" },
                new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.Spanish, Id = 1000102, Label = "Administrador" });

            modelBuilder.Entity<RoleTranslation>().HasData(
                new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.French, Id = 1000201, Label = "Administrateur des tâches en arrière-plan" },
                new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.Spanish, Id = 1000202, Label = "Administrador de tareas en segundo plano" });

            modelBuilder.Entity<RoleTranslation>().HasData(
                new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.French, Id = 1000301, Label = "Visualisation des tâches en arrière-plan" },
                new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.Spanish, Id = 1000302, Label = "Visualización de tareas en segundo plano" });
        }

        private static void BiaCreateNotificationTypeTranslationModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.French, Id = 101, Label = "Tâche" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.Spanish, Id = 102, Label = "Tarea" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.French, Id = 201, Label = "Information" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.Spanish, Id = 202, Label = "Información" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.French, Id = 301, Label = "Succès" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.Spanish, Id = 302, Label = "Éxito" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.French, Id = 401, Label = "Avertissement" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.Spanish, Id = 402, Label = "Advertencia" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.French, Id = 501, Label = "Erreur" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.Spanish, Id = 502, Label = "Culpa" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.DownloadReady, LanguageId = LanguageId.French, Id = 601, Label = "Téléchargement prêt" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.DownloadReady, LanguageId = LanguageId.Spanish, Id = 602, Label = "Descarga lista" });
        }

        private static void BiaCreateAnnouncementTypeTranslationModelData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnouncementTypeTranslation>().HasData(
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Information, LanguageId = LanguageId.English, Id = 101, Label = "Information" },
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Warning, LanguageId = LanguageId.English, Id = 102, Label = "Warning" },
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Information, LanguageId = LanguageId.French, Id = 103, Label = "Information" },
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Warning, LanguageId = LanguageId.French, Id = 104, Label = "Avertissement" },
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Information, LanguageId = LanguageId.Spanish, Id = 105, Label = "Información" },
                new AnnouncementTypeTranslation { AnnouncementTypeId = BiaAnnouncementType.Warning, LanguageId = LanguageId.Spanish, Id = 106, Label = "Advertencia" });
        }
    }
}