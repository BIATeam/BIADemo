// <copyright file="TranslationModelBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using System.Diagnostics;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// Class used to update the model builder for notification domain.
    /// </summary>
    public class TranslationModelBuilder : BaseTranslationModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public override void CreateModel(ModelBuilder modelBuilder)
        {
            Debug.Assert(modelBuilder != null, "Line to avoid warning empty method");
            base.CreateModel(modelBuilder);

            // Add here the project specific translation model creation.
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateLanguageModel(ModelBuilder modelBuilder)
        {
            base.CreateLanguageModel(modelBuilder);
            modelBuilder.Entity<Language>().HasData(new Language { Id = LanguageId.English, Code = "EN", Name = "English" });
            modelBuilder.Entity<Language>().HasData(new Language { Id = LanguageId.French, Code = "FR", Name = "Français" });
            modelBuilder.Entity<Language>().HasData(new Language { Id = LanguageId.Spanish, Code = "ES", Name = "Española" });

            // Begin BIADemo
            modelBuilder.Entity<Language>().HasData(new Language { Id = LanguageId.German, Code = "DE", Name = "Deutsch" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for notification.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateRoleTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateRoleTranslationModel(modelBuilder);

            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.French, Id = 1000101, Label = "Administrateur" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.Spanish, Id = 1000102, Label = "Administrador" });

            // Begin BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.Admin, LanguageId = LanguageId.German, Id = 1000103, Label = "Administrator" });

            // End BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.French, Id = 1000201, Label = "Administrateur des tâches en arrière-plan" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.Spanish, Id = 1000202, Label = "Administrador de tareas en segundo plano" });

            // Begin BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackAdmin, LanguageId = LanguageId.German, Id = 1000203, Label = "Administrator für Hintergrundaufgaben" });

            // End BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.French, Id = 1000301, Label = "Visualisation des tâches en arrière-plan" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.Spanish, Id = 1000302, Label = "Visualización de tareas en segundo plano" });

            // Begin BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)BiaRoleId.BackReadOnly, LanguageId = LanguageId.German, Id = 1000303, Label = "Visualisierung von Hintergrundaufgaben" });

            // End BIADemo

            // Begin BIADemo
            if (false)
            {
#pragma warning disable CS0162 // Unreachable code detected
            // DO NOT CHANGE INDENTATION (For BIATemplate)
            // End BIADemo
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.SiteAdmin, LanguageId = LanguageId.French, Id = 101, Label = "Administrateur du site" });
            modelBuilder.Entity<RoleTranslation>().HasData(new RoleTranslation { RoleId = (int)RoleId.SiteAdmin, LanguageId = LanguageId.Spanish, Id = 102, Label = "Administrador del sitio" });

            // Begin BIADemo
#pragma warning restore CS0162 // Unreachable code detected
            }

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

            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.French, Id = 101, Label = "Tâche" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.Spanish, Id = 102, Label = "Tarea" });

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Task, LanguageId = LanguageId.German, Id = 103, Label = "Aufgabe" });

            // End BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.French, Id = 201, Label = "Information" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.Spanish, Id = 202, Label = "Información" });

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Info, LanguageId = LanguageId.German, Id = 203, Label = "Information" });

            // End BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.French, Id = 301, Label = "Succès" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.Spanish, Id = 302, Label = "Éxito" });

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Success, LanguageId = LanguageId.German, Id = 303, Label = "Erfolg" });

            // End BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.French, Id = 401, Label = "Avertissement" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.Spanish, Id = 402, Label = "Advertencia" });

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Warning, LanguageId = LanguageId.German, Id = 403, Label = "Erwärmen" });

            // End BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.French, Id = 501, Label = "Erreur" });
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.Spanish, Id = 502, Label = "Culpa" });

            // Begin BIADemo
            modelBuilder.Entity<NotificationTypeTranslation>().HasData(new NotificationTypeTranslation { NotificationTypeId = (int)BiaNotificationTypeId.Error, LanguageId = LanguageId.German, Id = 503, Label = "Fehler" });

            // End BIADemo
        }

        /// <summary>
        /// Create the model for annoucement type translation.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void CreateAnnoucementTypeTranslationModel(ModelBuilder modelBuilder)
        {
            base.CreateAnnoucementTypeTranslationModel(modelBuilder);

            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Info, LanguageId = LanguageId.English, Id = 101, Label = "Information" });
            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Warning, LanguageId = LanguageId.English, Id = 102, Label = "Warning" });

            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Info, LanguageId = LanguageId.French, Id = 103, Label = "Information" });
            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Warning, LanguageId = LanguageId.French, Id = 104, Label = "Avertissement" });

            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Info, LanguageId = LanguageId.Spanish, Id = 105, Label = "Información" });
            modelBuilder.Entity<AnnoucementTranslation>().HasData(new AnnoucementTranslation { AnnoucementTypeId = BiaAnnoucementType.Warning, LanguageId = LanguageId.Spanish, Id = 106, Label = "Advertencia" });
        }
    }
}