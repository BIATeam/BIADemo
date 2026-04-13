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
        /// <inheritdoc />
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateLanguageModel(ModelBuilder modelBuilder)
        {
            base.CreateLanguageModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateLanguageModelData(ModelBuilder modelBuilder)
        {
            base.CreateLanguageModelData(modelBuilder);
            BiaCreateLanguageModelData(modelBuilder);

            // Begin BIADemo
            modelBuilder.Entity<Language>().HasData(new Language { Id = LanguageId.German, Code = "DE", Name = "Deutsch" });

            // End BIADemo
        }

        /// <inheritdoc />
        protected override void CreateRoleTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateRoleTranslationModel(modelBuilder);
        }

        /// <inheritdoc />
        protected override void CreateRoleTranslationModelData(ModelBuilder modelBuilder)
        {
            base.CreateRoleTranslationModelData(modelBuilder);
            BiaCreateRoleTranslationModelData(modelBuilder);

            // Begin BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.German, Id = 1000103, Label = "Administrator" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.German, Id = 1000203, Label = "Administrator für Hintergrundaufgaben" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.German, Id = 1000303, Label = "Visualisierung von Hintergrundaufgaben" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.SiteAdmin, LanguageId = LanguageId.French, Id = 101, Label = "Administrateur de la compagnie" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.SiteAdmin, LanguageId = LanguageId.Spanish, Id = 102, Label = "Administrador de la aerolínea" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.SiteAdmin, LanguageId = LanguageId.German, Id = 103, Label = "Fluglinienadministrator" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Pilot, LanguageId = LanguageId.French, Id = 201, Label = "Pilote" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Pilot, LanguageId = LanguageId.Spanish, Id = 202, Label = "Piloto" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Pilot, LanguageId = LanguageId.German, Id = 203, Label = "Pilot" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Supervisor, LanguageId = LanguageId.French, Id = 10101, Label = "Superviseur" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Supervisor, LanguageId = LanguageId.Spanish, Id = 10102, Label = "Supervisor" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Supervisor, LanguageId = LanguageId.German, Id = 10103, Label = "Supervisor" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Expert, LanguageId = LanguageId.French, Id = 10201, Label = "Expert" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Expert, LanguageId = LanguageId.Spanish, Id = 10202, Label = "Experto" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Expert, LanguageId = LanguageId.German, Id = 10203, Label = "Experte" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.TeamLeader, LanguageId = LanguageId.French, Id = 20101, Label = "Chef d'equipe" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.TeamLeader, LanguageId = LanguageId.Spanish, Id = 20102, Label = "Jefe de equipo" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.TeamLeader, LanguageId = LanguageId.German, Id = 20103, Label = "Teamleiter" });

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Operator, LanguageId = LanguageId.French, Id = 20201, Label = "Operateur" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Operator, LanguageId = LanguageId.Spanish, Id = 20202, Label = "Operador" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.Operator, LanguageId = LanguageId.German, Id = 20203, Label = "Operator" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateNotificationTypeTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTypeTranslationModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateNotificationTypeTranslationModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTypeTranslationModelData(modelBuilder);
            BiaCreateNotificationTypeTranslationModelData(modelBuilder);

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.German, Id = 103, Label = "Aufgabe" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.German, Id = 203, Label = "Information" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.German, Id = 303, Label = "Erfolg" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.German, Id = 403, Label = "Erwärmen" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.German, Id = 503, Label = "Fehler" },
                new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.DownloadReady, LanguageId = LanguageId.German, Id = 603, Label = "Download bereit" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateNotificationTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTranslationModel(modelBuilder);
        }

        /// <summary>
        /// Create the model data for notification translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateNotificationTranslationModelData(ModelBuilder modelBuilder)
        {
            base.CreateNotificationTranslationModelData(modelBuilder);
        }

        /// <summary>
        /// Create the model for announcement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateAnnouncementTypeTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateAnnouncementTypeTranslationModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for announcement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateAnnouncementTypeTranslationModelData(ModelBuilder modelBuilder)
        {
            base.CreateAnnouncementTypeTranslationModelData(modelBuilder);
            BiaCreateAnnouncementTypeTranslationModelData(modelBuilder);
        }
    }
}