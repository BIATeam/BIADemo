namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.File;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class BiaFileDownloaderService<TNotification, TNotificationDto, TNotificationMapper> : IBiaFileDownloaderService
        where TNotification : BaseNotification, new()
        where TNotificationDto : BaseNotificationDto, new()
        where TNotificationMapper : BaseNotificationMapper<TNotificationDto, TNotification>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public BiaFileDownloaderService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void PrepareDownload(Func<Task<FileDownloadData>> getFileDownloadDataTask, int requestedByUserId)
        {
            Task.Run(async () =>
            {
                using var scope = this.serviceScopeFactory.CreateAsyncScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<BiaFileDownloaderService<TNotification, TNotificationDto, TNotificationMapper>>>();
                var notificationRepository = scope.ServiceProvider.GetRequiredService<ITGenericRepository<TNotification, int>>();
                var clientForHubService = scope.ServiceProvider.GetRequiredService<IClientForHubService>();
                var mapper = scope.ServiceProvider.GetRequiredService<TNotificationMapper>();

                try
                {
                    var fileDownloadData = await getFileDownloadDataTask();
                    fileDownloadData.Token = GenerateFileDownloadToken(fileDownloadData, requestedByUserId);
                    var notification = CreateDownloadReadyNotification(fileDownloadData, requestedByUserId);

                    notificationRepository.Add(notification);
                    await notificationRepository.UnitOfWork.CommitAsync();
                    var notificationDto = await notificationRepository.GetResultAsync(mapper.EntityToDto(MapperMode.Item), notification.Id);
                    _ = clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", notificationDto);
                }
                catch (Exception ex)
                {
                    if (logger.IsEnabled(LogLevel.Error))
                    {
                        logger.LogError(ex, "Error preparing file download for user {UserId}", requestedByUserId);
                    }
                }
            });
        }

        private static string GenerateFileDownloadToken(FileDownloadData fileDownloadData, int requestedByUserId)
        {
            return Convert.ToHexString(Encoding.UTF8.GetBytes($"{fileDownloadData.FileName}:{requestedByUserId}:{DateTime.UtcNow.Ticks}"));
        }

        private static TNotification CreateDownloadReadyNotification(FileDownloadData fileDownloadData, int requestedByUserId)
        {
            return new TNotification
            {
                CreatedById = requestedByUserId,
                CreatedDate = DateTime.UtcNow,
                Description = "You can now download the file !",
                Title = $"Download ready for {fileDownloadData.FileName}",
                TypeId = (int)BiaNotificationTypeId.Info,
                Read = false,
                JData = JsonConvert.SerializeObject(new NotificationDataDto { Display = "Download", Route = [$"/file/download?token={fileDownloadData.Token}"], OpenRouteAsHref = true }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotifiedUsers = [new() { UserId = requestedByUserId }],
                NotificationTranslations = [],
                NotifiedTeams = [],
            };
        }
    }
}
