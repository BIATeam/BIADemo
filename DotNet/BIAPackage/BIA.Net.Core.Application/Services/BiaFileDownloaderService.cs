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

    public class BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto> : IBiaFileDownloaderService
        where TINotificationAppService : IBaseNotificationAppService<TNotificationDto, TNotificationListItemDto, TNotification>
        where TNotification : BaseNotification, new()
        where TNotificationDto : BaseNotificationDto, new()
        where TNotificationListItemDto : BaseNotificationListItemDto, new()
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
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>>>();
                var notificationAppService = scope.ServiceProvider.GetRequiredService<TINotificationAppService>();

                try
                {
                    var fileDownloadData = await getFileDownloadDataTask();
                    fileDownloadData.Token = GenerateFileDownloadToken(fileDownloadData, requestedByUserId);
                    var notification = CreateDownloadReadyNotification(fileDownloadData, requestedByUserId);

                    await notificationAppService.AddAsync(notification);
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

        private static TNotificationDto CreateDownloadReadyNotification(FileDownloadData fileDownloadData, int requestedByUserId)
        {
            return new TNotificationDto
            {
                CreatedBy = new OptionDto { Id = requestedByUserId },
                CreatedDate = DateTime.UtcNow,
                Description = "You can now download the file !",
                Title = $"Download ready for {fileDownloadData.FileName}",
                Type = new OptionDto { Id = (int)BiaNotificationTypeId.DownloadReady },
                Read = false,
                JData = JsonConvert.SerializeObject(new NotificationDataDto { Display = "Download", Route = [$"/files/download?token={fileDownloadData.Token}"], OpenRouteAsHref = true, IsApiRoute = true }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotifiedUsers = [new() { Id = requestedByUserId }],
                NotificationTranslations = [],
                NotifiedTeams = [],
            };
        }

        public async Task<FileDownloadData> GetFileDownloadData(string token)
        {
            return new FileDownloadData
            {
                FileName = "Example.txt",
                FileContent = Encoding.UTF8.GetBytes("This is an example file content."),
                FileContentType = "text/plain",
            };
        }
    }
}
