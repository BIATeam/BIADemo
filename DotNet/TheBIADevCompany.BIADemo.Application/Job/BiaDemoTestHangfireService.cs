// BIADemo only
// <copyright file="BiaDemoTestHangfireService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;
    using Hangfire.Server;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Application.Bia.Notification;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// Sample class to use a hangfire task.
    /// </summary>
    public class BiaDemoTestHangfireService : BaseJob, IBiaDemoTestHangfireService
    {
        private readonly INotificationAppService notificationAppService;

        private readonly ITGenericRepository<Team, int> teamRepository;

        private readonly IPlaneAppService planeAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDemoTestHangfireService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">logger.</param>
        /// <param name="notificationAppService">The notification service.</param>
        /// <param name="teamRepository">The team repository.</param>
        /// <param name="planeAppService">The plane repository.</param>
        /// <param name="principal">The principal.</param>
        public BiaDemoTestHangfireService(
            IConfiguration configuration,
            ILogger<BiaDemoTestHangfireService> logger,
            INotificationAppService notificationAppService,
            ITGenericRepository<Team, int> teamRepository,
            IPlaneAppService planeAppService)
            : base(configuration, logger)
        {
            this.notificationAppService = notificationAppService;
            this.teamRepository = teamRepository;
            this.planeAppService = planeAppService;
        }

        /// <summary>
        /// Test function for run task on hangfire.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task RunLongTask()
        {
            await Task.Delay(5000);
        }

        /// <summary>
        /// Test function for run task on hangfire with notification.
        /// </summary>
        /// <param name="teamId">The current Team Id.</param>
        /// <param name="currentSite">The current site.</param>
        /// <param name="createdById">The created By.</param>
        /// <param name="context">The context hangfire.</param>
        /// <returns>The task.</returns>
        public async Task RandomReviewPlane(int teamId, CurrentTeamDto currentSite, int createdById, PerformContext context)
        {
            await Task.Delay(2000);

            Team targetedTeam = null;
            if (teamId > 0)
            {
                targetedTeam = await this.teamRepository.GetEntityAsync(teamId);
            }

            int selectPlaneOnSiteId = 0;
            string selectPlaneOnSiteTitle = string.Empty;
            int targetRoleId = -1;
            if (targetedTeam?.TeamTypeId == (int)TeamTypeId.Site)
            {
                // if the task is launch for a team site use this site. (demonstarte the swith of site)
                selectPlaneOnSiteId = targetedTeam.Id;
                selectPlaneOnSiteTitle = targetedTeam.Title;
                targetRoleId = (int)RoleId.SiteAdmin;
            }
            else if (currentSite != null)
            {
                // else use the current site.
                selectPlaneOnSiteId = currentSite.TeamId;
                selectPlaneOnSiteTitle = currentSite.TeamTitle;
            }

            List<PlaneDto> targetPlanes = this.planeAppService.GetAllAsync(accessMode: AccessMode.All, filter: p => p.SiteId == selectPlaneOnSiteId, isReadOnlyMode: true).Result.ToList();
            if (targetPlanes.Count > 0)
            {
                // send notification review plane only if there is plane on the site
                var rand = new Random();
                int targetPlaneId = targetPlanes[rand.Next(targetPlanes.Count)].Id;

                var data = new NotificationDataDto
                {
                    Teams = new List<NotificationTeamDto>
                    {
                        new NotificationTeamDto
                        {
                            TeamTypeId = (int)TeamTypeId.Site,
                            Team = new OptionDto
                            {
                                Id = selectPlaneOnSiteId,
                                Display = selectPlaneOnSiteTitle,
                            },
                            Roles = new List<OptionDto>
                            {
                                new OptionDto { Id = (int)RoleId.SiteAdmin, Display = "SiteAdmin" },
                            },
                        },
                    },
                    Route = new string[] { "examples", "planes", targetPlaneId.ToString(), "edit" },
                };

                var roles = targetRoleId != -1 ? new List<OptionDto>
                    {
                        new OptionDto { Id = targetRoleId, DtoState = DtoState.Added },
                    }
                    : null;
                var notification = new NotificationDto
                {
                    CreatedBy = new OptionDto { Id = createdById },
                    CreatedDate = DateTime.Now,
                    Description = "Review the plane with id " + targetPlaneId + ".",
                    Title = "Review plane",
                    Type = new OptionDto { Id = (int)NotificationTypeId.Task },
                    NotifiedTeams = targetedTeam != null ? new List<NotificationTeamDto>
                    {
                       new NotificationTeamDto
                       {
                           DtoState = DtoState.Added,
                           Team = new OptionDto
                           {
                                Id = targetedTeam.Id,
                                Display = targetedTeam.Title,
                           },
                           TeamTypeId = targetedTeam.TeamTypeId,
                           Roles = roles,
                       },
                    }
                    : null,
                    Read = false,
                    JData = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    NotificationTranslations = new List<NotificationTranslationDto>
                    {
                        new NotificationTranslationDto() { LanguageId = LanguageId.French, Title = "Revoir l'avion", Description = "Passez en revue l'avion avec l'id " + targetPlaneId + ".", DtoState = DtoState.Added },
                        new NotificationTranslationDto() { LanguageId = LanguageId.Spanish, Title = "Avión de revisión", Description = "Revise el avión con id " + targetPlaneId + ".", DtoState = DtoState.Added },
                        new NotificationTranslationDto() { LanguageId = LanguageId.German, Title = "Flugzeug überprüfen", Description = "Überprüfen Sie das Flugzeug mit der ID " + targetPlaneId + ".", DtoState = DtoState.Added },
                    },
                };
                await this.notificationAppService.AddAsync(notification);
            }
            else
            {
                var data = new NotificationDataDto();

                var notification = new NotificationDto
                {
                    CreatedBy = new OptionDto { Id = createdById },
                    CreatedDate = DateTime.Now,
                    Description = "There is no plane to review on site '" + selectPlaneOnSiteTitle + "'.",
                    Title = "No plane to review",
                    Type = new OptionDto { Id = (int)NotificationTypeId.Info },

                    NotifiedTeams = targetedTeam != null ? new List<NotificationTeamDto>
                    {
                        new NotificationTeamDto
                        {
                            DtoState = DtoState.Added,
                            Team = new OptionDto
                            {
                                 Id = targetedTeam.Id,
                                 Display = targetedTeam.Title,
                            },
                            TeamTypeId = targetedTeam.TeamTypeId,
                        },
                    }
                    : null,
                    Read = false,
                    JData = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                    NotificationTranslations = new List<NotificationTranslationDto>
                    {
                        new NotificationTranslationDto() { LanguageId = LanguageId.French, Title = "Pas d'avion à revoir", Description = "Il n'y a pas d'avion à revoir sur le site '" + selectPlaneOnSiteTitle + "'.", DtoState = DtoState.Added },
                        new NotificationTranslationDto() { LanguageId = LanguageId.Spanish, Title = "No hay avión para revisar", Description = "No hay ningún avión para revisar en el sitio '" + selectPlaneOnSiteTitle + "'.", DtoState = DtoState.Added },
                        new NotificationTranslationDto() { LanguageId = LanguageId.German, Title = "Kein Flugzeug zu überprüfen", Description = "An Standort '" + selectPlaneOnSiteTitle + "' sind keine Flugzeuge zu überprüfen.", DtoState = DtoState.Added },
                    },
                };
                await this.notificationAppService.AddAsync(notification);
            }
        }

        /// <summary>
        /// Run the processes that are waiting.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task RunMonitoredTask()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            await Task.Delay(5000);
            var message = $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}: BiaDemoTestHangfire => This log is generated by a hangfire task";
            this.Logger.LogInformation(message);
        }
    }
}