// <copyright file="BiaFileDownloaderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.File;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Error;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.File;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.File.Entities;
    using BIA.Net.Core.Domain.File.Mappers;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using Hangfire;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// File downloader service that prepares file download and notifies the user when the file is ready to be downloaded.
    /// </summary>
    /// <typeparam name="TFileDownloaderOptions">Type of the file downloader options.</typeparam>
    /// <typeparam name="TINotificationAppService">Interface for the notification application service.</typeparam>
    /// <typeparam name="TNotification">Type of the notification.</typeparam>
    /// <typeparam name="TNotificationDto">Type of the notification DTO.</typeparam>
    /// <typeparam name="TNotificationListItemDto">Type of the notification list item DTO.</typeparam>
    public class BiaFileDownloaderService<TFileDownloaderOptions, TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto> : IBiaFileDownloaderService
        where TFileDownloaderOptions : BiaFileDownloaderOptions, new()
        where TINotificationAppService : IBaseNotificationAppService<TNotificationDto, TNotificationListItemDto, TNotification>
        where TNotification : BaseNotification, new()
        where TNotificationDto : BaseNotificationDto, new()
        where TNotificationListItemDto : BaseNotificationListItemDto, new()
    {
        private readonly TINotificationAppService notificationAppService;
        private readonly ILogger<BiaFileDownloaderService<TFileDownloaderOptions, TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger;
        private readonly IFileDownloadDataAppService fileDownloadDataAppService;
        private readonly IFileDownloadTokenRepository fileDownloadTokenRepository;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly TFileDownloaderOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaFileDownloaderService{TFileDownloaderOptions, TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto}"/> class.
        /// </summary>
        /// <param name="notificationAppService">The notification application service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fileDownloadDataAppService">The file download data application service.</param>
        /// <param name="fileDownloadTokenRepository">The file download token repository.</param>
        /// <param name="backgroundJobClient">Hangfire job client.</param>
        /// <param name="options">The file downloader options (language IDs, etc.).</param>
        public BiaFileDownloaderService(
            TINotificationAppService notificationAppService,
            ILogger<BiaFileDownloaderService<TFileDownloaderOptions, TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger,
            IFileDownloadDataAppService fileDownloadDataAppService,
            IFileDownloadTokenRepository fileDownloadTokenRepository,
            IBackgroundJobClient backgroundJobClient,
            IOptions<TFileDownloaderOptions> options)
        {
            this.notificationAppService = notificationAppService;
            this.logger = logger;
            this.fileDownloadDataAppService = fileDownloadDataAppService;
            this.fileDownloadTokenRepository = fileDownloadTokenRepository;
            this.backgroundJobClient = backgroundJobClient;
            this.options = options.Value;
        }

        /// <inheritdoc/>
        public virtual void PrepareBackgroundDownload<TService>(int requestedByUserId, Expression<Func<TService, Task<FileDownloadDataDto>>> generateFileExpression)
            where TService : class
        {
            if (generateFileExpression.Body is not MethodCallExpression methodCall)
            {
                throw new ArgumentException("Expression body must be a method call", nameof(generateFileExpression));
            }

            var args = methodCall.Arguments
                .Select(argExpr => Expression.Lambda(argExpr).Compile().DynamicInvoke())
                .ToArray();

            var serviceTypeName = typeof(TService).AssemblyQualifiedName;
            var methodName = methodCall.Method.Name;
            var serializedArgs = JsonConvert.SerializeObject(args);
            var serializedArgTypes = JsonConvert.SerializeObject(methodCall.Method.GetParameters().Select(p => p.ParameterType.AssemblyQualifiedName).ToArray());

            this.backgroundJobClient.Enqueue<PrepareDownloadTask>(x => x.Run(serviceTypeName, methodName, serializedArgs, serializedArgTypes, requestedByUserId));
        }

        /// <inheritdoc/>
        public virtual async Task NotifyDownloadReadyAsync(FileDownloadDataDto fileDownloadDataDto, int requestedByUserId)
        {
            try
            {
                if (string.IsNullOrEmpty(fileDownloadDataDto.FilePath) || !File.Exists(fileDownloadDataDto.FilePath))
                {
                    throw new FileNotFoundException("The file path is invalid or the file does not exist.");
                }

                if (string.IsNullOrEmpty(fileDownloadDataDto.FileName))
                {
                    throw new FileNotFoundException("The file name is invalid.");
                }

                if (string.IsNullOrEmpty(fileDownloadDataDto.FileContentType))
                {
                    throw new FileNotFoundException("The file content type is invalid.");
                }

                fileDownloadDataDto.RequestByUser = new OptionDto { Id = requestedByUserId };
                fileDownloadDataDto.RequestDateTime = DateTime.UtcNow;

                await this.fileDownloadDataAppService.AddAsync(fileDownloadDataDto);

                var notification = this.CreateDownloadReadyNotification(fileDownloadDataDto, requestedByUserId);
                await this.notificationAppService.AddAsync(notification);
            }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error))
                {
                    this.logger.LogError(ex, "Error notifying file download for user {UserId}", requestedByUserId);
                }
            }
        }

        /// <inheritdoc/>
        public virtual async Task<string> GenerateDownloadToken(Guid fileGuid, int requestedByUserId)
        {
            try
            {
                var fileDownloadData = await this.fileDownloadDataAppService.GetAsync(fileGuid, isReadOnlyMode: true);
                if (fileDownloadData.RequestByUser.Id != requestedByUserId)
                {
                    throw FrontUserException.Create(BiaErrorId.UnauthorizeFileToDownload);
                }

                await this.EnsureDownloadAvailability(fileDownloadData);

                var token = Convert.ToHexString(Encoding.UTF8.GetBytes($"{fileDownloadData.FileName}:{requestedByUserId}:{DateTime.UtcNow.Ticks}"));
                await this.fileDownloadTokenRepository.AddAsync(new() { FileGuid = fileGuid, Token = token, CreatedAt = DateTime.UtcNow });
                return token;
            }
            catch (ElementNotFoundException)
            {
                throw FrontUserException.Create(BiaErrorId.FileToDownloadNotFound);
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

        /// <inheritdoc/>
        public virtual async Task RemoveFileToDownload(FileDownloadDataDto fileDownloadData, bool deleteAssociatedNotification = false)
        {
            try
            {
                if (deleteAssociatedNotification)
                {
                    var notificationsToDelete = await this.notificationAppService.GetAsync(
                        isReadOnlyMode: true,
                        filter:
                            n => n.TypeId == (int)BiaNotificationTypeId.DownloadReady
                            && n.CreatedById == fileDownloadData.RequestByUser.Id
                            && n.JData.Contains(fileDownloadData.Id.ToString()));

                    await this.notificationAppService.RemoveAsync(notificationsToDelete.Id);
                }

                await this.fileDownloadDataAppService.RemoveAsync(fileDownloadData.Id);

                if (File.Exists(fileDownloadData.FilePath))
                {
                    File.Delete(fileDownloadData.FilePath);
                }
            }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error))
                {
                    this.logger.LogError(ex, "Error deleting expired file with id {Id}", fileDownloadData.Id);
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task<FileDownloadDataDto> GetFileDownloadData(Guid fileGuid, string token)
        {
            try
            {
                var fileDownloadToken = await this.fileDownloadTokenRepository.GetAsync(fileGuid, token) ?? throw new ElementNotFoundException("Unable to retrieve file download token");
                await this.fileDownloadTokenRepository.RemoveAsync(fileDownloadToken);

                var fileDownloadDataDto = await this.fileDownloadDataAppService.GetAsync(fileDownloadToken.FileDownloadData.Id, isReadOnlyMode: true);
                await this.EnsureDownloadAvailability(fileDownloadDataDto);

                return fileDownloadDataDto;
            }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error))
                {
                    this.logger.LogError(ex, "Error getting download data for file {FileGuid} with token {Token}", fileGuid, token);
                }

                throw;
            }
        }

        /// <summary>
        /// Creates a notification DTO indicating that a file download is ready for the specified user.
        /// </summary>
        /// <param name="fileDownloadDataDto">The file download data used to generate the notification.</param>
        /// <param name="requestedByUserId">The ID of the user who requested the file download.</param>
        /// <returns>A notification DTO populated with download-ready information.</returns>
        protected virtual TNotificationDto CreateDownloadReadyNotification(FileDownloadDataDto fileDownloadDataDto, int requestedByUserId)
        {
            var notificationTranslations = this.GetNotificationTranslations(fileDownloadDataDto.FileName);
            var notificationTranslationEnglish = notificationTranslations.First(nt => nt.LanguageId == this.options.EnglishLanguageId);

            return new TNotificationDto
            {
                CreatedBy = new OptionDto { Id = requestedByUserId },
                CreatedDate = DateTime.UtcNow,
                Description = notificationTranslationEnglish.Description,
                Title = notificationTranslationEnglish.Title,
                Type = new OptionDto { Id = (int)BiaNotificationTypeId.DownloadReady },
                Read = false,
                JData = JsonConvert.SerializeObject(new NotificationDataDto { Display = "bia.download", DownloadFileGuid = fileDownloadDataDto.Id }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                NotifiedUsers = [new() { Id = requestedByUserId }],
                NotificationTranslations = [.. notificationTranslations.Except([notificationTranslationEnglish])],
                NotifiedTeams = [],
            };
        }

        /// <summary>
        /// Gets the list of notification translations for the download ready notification. By default, it creates translations in English, French and Spanish using the file name in the description. This method can be overridden to provide custom translations or support additional languages.
        /// </summary>
        /// <param name="fileName">The name of the file being downloaded.</param>
        /// <returns>A list of notification translations for the download ready notification.</returns>
        protected virtual List<NotificationTranslationDto> GetNotificationTranslations(string fileName)
        {
            return
            [
                new NotificationTranslationDto
                {
                    LanguageId = this.options.FrenchLanguageId,
                    Title = "Téléchargement prêt",
                    Description = $"Votre fichier '{fileName}' est prêt à être téléchargé.",
                    DtoState = Domain.Dto.Base.DtoState.Added,
                },
                new NotificationTranslationDto
                {
                    LanguageId = this.options.EnglishLanguageId,
                    Title = "Download ready",
                    Description = $"Your file '{fileName}' is ready to be downloaded.",
                    DtoState = Domain.Dto.Base.DtoState.Added,
                },
                new NotificationTranslationDto
                {
                    LanguageId = this.options.SpanishLanguageId,
                    Title = "Descarga lista",
                    Description = $"Su archivo '{fileName}' está listo para ser descargado.",
                    DtoState = Domain.Dto.Base.DtoState.Added,
                },
            ];
        }

        /// <summary>
        /// Ensures that the file to download is still available based on the request date and the availability duration. If the file is no longer available, it removes the file to download and throws a FrontUserException indicating that the file has expired.
        /// </summary>
        /// <param name="fileDownloadData">The file download data to check for availability.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task EnsureDownloadAvailability(FileDownloadDataDto fileDownloadData)
        {
            if (fileDownloadData.AvailabilityDuration.HasValue && fileDownloadData.RequestDateTime.Add(fileDownloadData.AvailabilityDuration.Value) < DateTime.UtcNow)
            {
                await this.RemoveFileToDownload(fileDownloadData);
                throw FrontUserException.Create(BiaErrorId.FileToDownloadExpired);
            }
        }
    }
}
