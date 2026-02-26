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
    /// <typeparam name="TINotificationAppService">Interface for the notification application service.</typeparam>
    /// <typeparam name="TNotification">Type of the notification.</typeparam>
    /// <typeparam name="TNotificationDto">Type of the notification DTO.</typeparam>
    /// <typeparam name="TNotificationListItemDto">Type of the notification list item DTO.</typeparam>
    public sealed class BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto> : IBiaFileDownloaderService
        where TINotificationAppService : IBaseNotificationAppService<TNotificationDto, TNotificationListItemDto, TNotification>
        where TNotification : BaseNotification, new()
        where TNotificationDto : BaseNotificationDto, new()
        where TNotificationListItemDto : BaseNotificationListItemDto, new()
    {
        private readonly TINotificationAppService notificationAppService;
        private readonly ILogger<BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger;
        private readonly FileDownloadDataMapper fileDownloadDataMapper;
        private readonly ITGenericRepository<FileDownloadData, Guid> fileDownloadDataRepository;
        private readonly IFileDownloadTokenRepository fileDownloadTokenRepository;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly BiaFileDownloaderOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaFileDownloaderService{TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto}"/> class.
        /// </summary>
        /// <param name="notificationAppService">The notification application service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fileDownloadDataMapper">The file download data mapper.</param>
        /// <param name="fileDownloadDataRepository">The file download data repository.</param>
        /// <param name="fileDownloadTokenRepository">The file download token repository.</param>
        /// <param name="backgroundJobClient">Hangfire job client.</param>
        /// <param name="options">The file downloader options (language IDs, etc.).</param>
        public BiaFileDownloaderService(
            TINotificationAppService notificationAppService,
            ILogger<BiaFileDownloaderService<TINotificationAppService, TNotification, TNotificationDto, TNotificationListItemDto>> logger,
            FileDownloadDataMapper fileDownloadDataMapper,
            ITGenericRepository<FileDownloadData, Guid> fileDownloadDataRepository,
            IFileDownloadTokenRepository fileDownloadTokenRepository,
            IBackgroundJobClient backgroundJobClient,
            IOptions<BiaFileDownloaderOptions> options)
        {
            this.notificationAppService = notificationAppService;
            this.logger = logger;
            this.fileDownloadDataMapper = fileDownloadDataMapper;
            this.fileDownloadDataRepository = fileDownloadDataRepository;
            this.fileDownloadTokenRepository = fileDownloadTokenRepository;
            this.backgroundJobClient = backgroundJobClient;
            this.options = options.Value;
        }

        /// <inheritdoc/>
        public void PrepareBackgroundDownload<TService>(int requestedByUserId, Expression<Func<TService, Task<FileDownloadDataDto>>> generateFileExpression)
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
        public async Task NotifyDownloadReadyAsync(FileDownloadDataDto fileDownloadDataDto, int requestedByUserId)
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

                FileDownloadData fileDownloadData = null;
                this.fileDownloadDataMapper.DtoToEntity(fileDownloadDataDto, ref fileDownloadData);
                this.fileDownloadDataRepository.Add(fileDownloadData);
                await this.fileDownloadDataRepository.UnitOfWork.CommitAsync();

                fileDownloadDataDto.Id = fileDownloadData.Id;
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
        public async Task<string> GenerateDownloadToken(Guid fileGuid, int requestedByUserId)
        {
            try
            {
                var fileDownloadData = await this.fileDownloadDataRepository.GetResultAsync(this.fileDownloadDataMapper.EntityToDto(), fileGuid, isReadOnlyMode: true)
                    ?? throw new ElementNotFoundException("Unable to retrieve file download data");

                if (fileDownloadData.RequestByUser.Id != requestedByUserId)
                {
                    throw new UnauthorizedAccessException("User is not authorized to download this file");
                }

                await this.EnsureDownloadAvailability(fileDownloadData);

                var token = Convert.ToHexString(Encoding.UTF8.GetBytes($"{fileDownloadData.FileName}:{requestedByUserId}:{DateTime.UtcNow.Ticks}"));
                await this.fileDownloadTokenRepository.AddAsync(new() { FileGuid = fileGuid, Token = token, CreatedAt = DateTime.UtcNow });
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

        /// <inheritdoc/>
        public async Task RemoveFileToDownload(FileDownloadDataDto fileDownloadData, bool deleteAssociatedNotification = false)
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

                await this.fileDownloadDataRepository.DeleteByIdsAsync([fileDownloadData.Id]);

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
        public async Task<FileDownloadDataDto> GetFileDownloadData(Guid fileGuid, string token)
        {
            try
            {
                var fileDownloadToken = await this.fileDownloadTokenRepository.GetAsync(fileGuid, token) ?? throw new ElementNotFoundException("Unable to retrieve file download token");
                await this.fileDownloadTokenRepository.RemoveAsync(fileDownloadToken);

                var fileDownloadDataDto = this.fileDownloadDataMapper.EntityToDto().Compile().Invoke(fileDownloadToken.FileDownloadData);
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

        private TNotificationDto CreateDownloadReadyNotification(FileDownloadDataDto fileDownloadDataDto, int requestedByUserId)
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

        private List<NotificationTranslationDto> GetNotificationTranslations(string fileName)
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

        private async Task EnsureDownloadAvailability(FileDownloadDataDto fileDownloadData)
        {
            if (fileDownloadData.AvailabilityDuration.HasValue && fileDownloadData.RequestDateTime.Add(fileDownloadData.AvailabilityDuration.Value) < DateTime.UtcNow)
            {
                await this.RemoveFileToDownload(fileDownloadData);
                throw FrontUserException.Create(BiaErrorId.FileToDownloadExpired);
            }
        }
    }
}
