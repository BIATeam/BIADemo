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
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// Sample class to use a hangfire task.
    /// </summary>
    public class BiaDemoTestHangfireService : BaseJob, IBiaDemoTestHangfireService
    {
        private readonly INotificationDomainService notificationAppService;

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
        public BiaDemoTestHangfireService(
            IConfiguration configuration,
            ILogger<BiaDemoTestHangfireService> logger,
            INotificationDomainService notificationAppService,
            IClientForHubRepository clientForHubService)
            : base(configuration, logger)
        {
            this.notificationAppService = notificationAppService;
            this.clientForHubService = clientForHubService;
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
        /// <param name="siteId">The current Site Id.</param>
        /// <param name="createdById">The created By.</param>
        /// <param name="context">The context hangfire.</param>
        /// <returns>The task.</returns>
        public async Task RunLongTaskWithNotification(int siteId, int createdById, PerformContext context)
        {
            await Task.Delay(2000);

            var target = new NotificationDataDto
            {
                Route = new string[] { "examples", "planes", "30", "edit" },
            };

            var notification = new NotificationDto
            {
                CreatedBy = new OptionDto { Id = createdById },
                CreatedDate = DateTime.Now,
                Description = "Review the plane with id 30.",
                SiteId = siteId,
                Title = "Review plane",
                Type = new OptionDto { Id = (int)NotificationTypeId.Task },
                NotifiedPermissions = new List<OptionDto> { new OptionDto { Id = 1, DtoState = DtoState.Added } },
                Read = false,
                JData = JsonConvert.SerializeObject(target, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotificationTranslations = new List<NotificationTranslationDto>
                {
                    new NotificationTranslationDto() { LanguageId = LanguageId.French, Title = "Revoir l'avion", Description = "Passez en revue l'avion avec l'id 30.", DtoState = DtoState.Added },
                    new NotificationTranslationDto() { LanguageId = LanguageId.Spanish, Title = "Avi�n de revisi�n", Description = "Revise el avi�n con id 30.", DtoState = DtoState.Added },
                    new NotificationTranslationDto() { LanguageId = LanguageId.German, Title = "Flugzeug �berpr�fen", Description = "�berpr�fen Sie das Flugzeug mit der ID 30.", DtoState = DtoState.Added },
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