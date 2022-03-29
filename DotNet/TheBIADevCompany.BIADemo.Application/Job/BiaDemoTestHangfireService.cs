// BIADemo only
// <copyright file="BiaDemoTestHangfireService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using Hangfire.Server;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// Sample class to use a hangfire task.
    /// </summary>
    public class BiaDemoTestHangfireService : BaseJob, IBiaDemoTestHangfireService
    {
        private readonly INotificationDomainService notificationAppService;

        private readonly ITGenericRepository<Team, int> teamRepository;

        /// <summary>
        /// The signalR Service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDemoTestHangfireService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">logger.</param>
        /// <param name="notificationAppService">The notification service.</param>
        /// <param name="clientForHubService">The client for hub (signalR) service.</param>
        /// <param name="teamRepository">The team repository.</param>
        public BiaDemoTestHangfireService(
            IConfiguration configuration,
            ILogger<BiaDemoTestHangfireService> logger,
            INotificationDomainService notificationAppService,
            IClientForHubRepository clientForHubService,
            ITGenericRepository<Team, int> teamRepository)
            : base(configuration, logger)
        {
            this.notificationAppService = notificationAppService;
            this.clientForHubService = clientForHubService;
            this.teamRepository = teamRepository;
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
        /// <param name="createdById">The created By.</param>
        /// <param name="context">The context hangfire.</param>
        /// <returns>The task.</returns>
        public async Task RunLongTaskWithNotification(int teamId, int createdById, PerformContext context)
        {
            await Task.Delay(2000);

            Team targetedTeam = null;

            if (teamId > 0)
            {
                targetedTeam = await this.teamRepository.GetEntityAsync(teamId);
            }

            var data = new NotificationDataDto();

            if (targetedTeam != null)
            {
                data.Teams = new List<NotificationTeamDto>
                {
                    new NotificationTeamDto
                    {
                        TypeId = targetedTeam.TeamTypeId,
                        Id = targetedTeam.Id,
                        Display = targetedTeam.Title,
                    },
                };

                switch (targetedTeam.TeamTypeId)
                {
                    case (int)TeamTypeId.Site:
                        data.Route = new string[] { "sites", teamId.ToString(), "members" };
                        break;
                    case (int)TeamTypeId.AircraftMaintenanceCompany:
                        data.Route = new string[] { "examples", "aircraft-maintenance-companies", teamId.ToString(), "members" };
                        data.Display = "aircraftMaintenanceCompany.goto";
                        break;
                }
            }

            var notification = new NotificationDto
            {
                CreatedBy = new OptionDto { Id = createdById },
                CreatedDate = DateTime.Now,
                Description = "Review the plane with id 30.",
                Title = "Review plane",
                Type = new OptionDto { Id = (int)NotificationTypeId.Task },
                NotifiedRoles = new List<OptionDto> { new OptionDto { Id = (int)RoleId.SiteAdmin, DtoState = DtoState.Added } },
                NotifiedTeams = targetedTeam != null ? new List<NotificationTeamDto>
                    {
                        new NotificationTeamDto
                        {
                            Id = targetedTeam.Id,
                            DtoState = DtoState.Added,
                            TypeId = targetedTeam.TeamTypeId,
                            Display = targetedTeam.Title,
                        },
                    } : null,
                Read = false,
                JData = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotificationTranslations = new List<NotificationTranslationDto>
                {
                    new NotificationTranslationDto() { LanguageId = LanguageId.French, Title = "Revoir l'avion", Description = "Passez en revue l'avion avec l'id 30.", DtoState = DtoState.Added },
                    new NotificationTranslationDto() { LanguageId = LanguageId.Spanish, Title = "Avión de revisión", Description = "Revise el avión con id 30.", DtoState = DtoState.Added },
                    new NotificationTranslationDto() { LanguageId = LanguageId.German, Title = "Flugzeug überprüfen", Description = "Überprüfen Sie das Flugzeug mit der ID 30.", DtoState = DtoState.Added },
                },
            };

            await this.notificationAppService.AddAsync(notification);
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
            this.Logger.LogInformation($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}: BiaDemoTestHangfire => This log is generated by a hangfire task");
        }
    }
}