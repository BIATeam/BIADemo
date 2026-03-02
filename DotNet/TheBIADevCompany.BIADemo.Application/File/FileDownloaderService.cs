// <copyright file="FileDownloaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.File
{
    using BIA.Net.Core.Application.File;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Project-specific implementation of the file downloader service.
    /// </summary>
    public class FileDownloaderService : BiaFileDownloaderService<FileDownloaderOptions, INotificationAppService, Notification, NotificationDto, NotificationListItemDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloaderService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public FileDownloaderService(
            IServiceProvider serviceProvider,
            ILogger<FileDownloaderService> logger)
            : base(serviceProvider, logger)
        {
        }
    }
}
