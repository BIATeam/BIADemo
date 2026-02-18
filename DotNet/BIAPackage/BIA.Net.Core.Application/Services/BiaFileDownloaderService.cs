namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        private readonly ILogger<BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger;
        private readonly Dictionary<Guid, FileDownloadData> downloadDataByFileGuid = [];
        private readonly Dictionary<string, Guid> fileGuidByDownloadToken = [];

        public BiaFileDownloaderService(IServiceScopeFactory serviceScopeFactory, ILogger<BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public void PrepareDownload(Func<Task<FileDownloadData>> getFileDownloadDataTask, int requestedByUserId)
        {
            Task.Run(async () =>
            {
                using var scope = this.serviceScopeFactory.CreateAsyncScope();
                var notificationAppService = scope.ServiceProvider.GetRequiredService<TINotificationAppService>();

                try
                {
                    var fileDownloadData = await getFileDownloadDataTask();
                    fileDownloadData.FileGuid = Guid.NewGuid();
                    fileDownloadData.RequestByUserId = requestedByUserId;

                    this.downloadDataByFileGuid.Add(fileDownloadData.FileGuid, fileDownloadData);

                    var notification = CreateDownloadReadyNotification(fileDownloadData, requestedByUserId);
                    await notificationAppService.AddAsync(notification);
                }
                catch (Exception ex)
                {
                    if (this.logger.IsEnabled(LogLevel.Error))
                    {
                        this.logger.LogError(ex, "Error preparing file download for user {UserId}", requestedByUserId);
                    }
                }
            });
        }

        public string GenerateDownloadToken(Guid fileGuid, int requestedByUserId)
        {
            try
            {
                if (!this.downloadDataByFileGuid.TryGetValue(fileGuid, out var fileDownloadData))
                {
                    throw new ArgumentException("Invalid file guid");
                }

                if (fileDownloadData.RequestByUserId != requestedByUserId)
                {
                    throw new UnauthorizedAccessException("User is not authorized to download this file");
                }

                var token = Convert.ToHexString(Encoding.UTF8.GetBytes($"{fileDownloadData.FileName}:{requestedByUserId}:{DateTime.UtcNow.Ticks}"));
                this.fileGuidByDownloadToken[token] = fileGuid;
                return token;
            }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error))
                {
                    this.logger.LogError(ex, "Error generating download token for file {FileGuid} and user {UserId}", fileGuid, requestedByUserId);
                }

                throw;
            }
        }

        public FileDownloadData GetFileDownloadData(Guid fileGuid, string token)
        {
            try
            {
                if (!this.fileGuidByDownloadToken.TryGetValue(token, out var registerFileGuid))
                {
                    throw new ArgumentException("Invalid download token");
                }

                if (registerFileGuid != fileGuid)
                {
                    throw new ArgumentException("Download token does not match the file guid");
                }

                if (!this.downloadDataByFileGuid.TryGetValue(registerFileGuid, out var fileDownloadData))
                {
                    throw new ArgumentException("Invalid file guid");
                }

                this.fileGuidByDownloadToken.Remove(token);
                return fileDownloadData;
            }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error))
                {
                    this.logger.LogError(ex, "Error downloading file {FileGuid}", fileGuid);
                }

                throw;
            }
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
                JData = JsonConvert.SerializeObject(new NotificationDataDto { Display = "Download", DownloadFileGuid = fileDownloadData.FileGuid }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotifiedUsers = [new() { Id = requestedByUserId }],
                NotificationTranslations = [],
                NotifiedTeams = [],
            };
        }
    }
}
