namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Notification.Entities;

    public class BiaFileDownloaderService<TNotificationAppService, TNotificationDto, TNotificationListItemDto, TNotification> : IBiaFileDownloaderService
        where TNotificationAppService : IBaseNotificationAppService<TNotificationDto, TNotificationListItemDto, TNotification>
        where TNotificationDto : BaseNotificationDto, new()
        where TNotificationListItemDto : BaseNotificationListItemDto, new()
        where TNotification : BaseNotification, new()
    {
        private TNotificationAppService notificationAppService;

        public BiaFileDownloaderService(TNotificationAppService notificationAppService)
        {
            this.notificationAppService = notificationAppService;
        }

        public void Start(Func<Task<string>> asyncAction)
        {
            Task.Run(async () =>
            {
                try
                {
                    string fileName = await asyncAction();
                    var notification = new TNotificationDto
                    {
                        Title = "Download ready",
                        Description = $"The file '{fileName}' is ready for download.",
                    };

                    await this.notificationAppService.AddAsync(notification);
                }
                catch (Exception ex)
                {
                    var notification = new TNotificationDto
                    {
                        Title = "Download preprocess failed",
                        Description = $"An error occurred while generating the file: {ex.Message}",
                    };

                    await this.notificationAppService.AddAsync(notification);
                }
            });
        }
    }
}
