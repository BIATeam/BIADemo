// <copyright file="ArchiveServiceBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using static BIA.Net.Core.Common.Configuration.WorkerFeature.ArchiveConfiguration;

    /// <summary>
    /// The base service for the archive services of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity key type.</typeparam>
    public abstract class ArchiveServiceBase<TEntity, TKey> : IArchiveService
        where TEntity : class, IEntityArchivable<TKey>
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveServiceBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="archiveRepository">The <see cref="ITGenericArchiveRepository{TEntity, TKey}"/> archive repository.</param>
        /// <param name="logger">The logger.</param>
        protected ArchiveServiceBase(IConfiguration configuration, ITGenericArchiveRepository<TEntity, TKey> archiveRepository, ILogger logger)
        {
            var biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(biaNetSection);
            this.ArchiveEntityConfiguration = biaNetSection.WorkerFeatures?.Archive?.ArchiveEntityConfigurations.FirstOrDefault(x => x.IsValid && x.IsMatchingEntityType(typeof(TEntity)));

            this.ArchiveRepository = archiveRepository;
            this.Logger = logger;

            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        /// <summary>
        /// The entity archive configuration.
        /// </summary>
        protected ArchiveEntityConfiguration ArchiveEntityConfiguration { get; }

        /// <summary>
        /// The entity archive repository.
        /// </summary>
        protected ITGenericArchiveRepository<TEntity, TKey> ArchiveRepository { get; }

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc/>
        public async Task RunAsync()
        {
            try
            {
                this.Logger.LogInformation("Begin archive of {EntityTypeName} entity", typeof(TEntity).Name);

                if (this.ArchiveEntityConfiguration is null)
                {
                    this.Logger.LogWarning("No valid archive configuration found for the entity {EntityTypeName}.", typeof(TEntity).Name);
                    return;
                }

                var items = await this.ArchiveRepository.GetAllAsync(this.ArchiveFilterRule());
                foreach (var item in items)
                {
                    await this.ArchiveItemAsync(item);
                }
            }
            finally
            {
                this.Logger.LogInformation("End archive of {EntityTypeName} entity", typeof(TEntity).Name);
            }
        }

        /// <summary>
        /// Retrive the archive file name template for an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><see cref="string"/>.</returns>
        protected abstract string GetArchiveNameTemplate(TEntity entity);

        /// <summary>
        /// The rule to filter the entities to archive.
        /// </summary>
        /// <returns><see cref="Expression"/>.</returns>
        protected virtual Expression<Func<TEntity, bool>> ArchiveFilterRule()
        {
            return x => x.IsFixed && x.FixedDate != null && (x.ArchivedDate == null || x.ArchivedDate.Value < x.FixedDate.Value);
        }

        /// <summary>
        /// Archive an entity.
        /// </summary>
        /// <param name="item">The entity to archive.</param>
        /// <returns><see cref="Task"/>.</returns>
        protected virtual async Task ArchiveItemAsync(TEntity item)
        {
            try
            {
                if (await this.SaveItemAsFlatTextCompressedAsync(item, this.ArchiveEntityConfiguration.TargetDirectoryPath))
                {
                    await this.ArchiveRepository.SetAsArchivedAsync(item);
                    this.Logger.LogInformation("Item {ItemId} archived successfully", item.Id);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to archive item {ItemId} : {ExceptionMessage}", item.Id, ex.Message);
            }
        }

        /// <summary>
        /// Save an entity to the target as flat text JSON into a ZIP archive.
        /// </summary>
        /// <param name="item">The entity to save.</param>
        /// <param name="targetDirectoryPath">Target directory path.</param>
        /// <returns><see cref="Task{bool}"/> that indicates success.</returns>
        protected async Task<bool> SaveItemAsFlatTextCompressedAsync(TEntity item, string targetDirectoryPath)
        {
            var targetZipFileName = $"{this.GetArchiveNameTemplate(item)}.zip";
            var targetZipFilePath = Path.Combine(targetDirectoryPath, targetZipFileName);
            var sourceZipFilePath = Path.GetRandomFileName();

            try
            {
                var jsonContent = JsonConvert.SerializeObject(item, this.jsonSerializerSettings);

                var targerDirectoryPath = Path.GetDirectoryName(targetZipFilePath);
                if (!Directory.Exists(targerDirectoryPath))
                {
                    Directory.CreateDirectory(targerDirectoryPath);
                }

                await using (var zipStream = new FileStream(sourceZipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: false);
                    var jsonEntry = archive.CreateEntry(targetZipFileName.Replace(".zip", ".json"));
                    await using var entryStream = jsonEntry.Open();
                    await using var writer = new StreamWriter(entryStream);
                    await writer.WriteAsync(jsonContent);
                }

                var sourceHash = ComputeHash(sourceZipFilePath);

                File.Copy(sourceZipFilePath, targetZipFilePath, true);

                var targetHash = ComputeHash(targetZipFilePath);
                if (targetHash != sourceHash)
                {
                    throw new JobException("Integrity compromised after copy");
                }

                return true;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to save item {ItemId} : {ExceptionMessage}", item.Id, ex.Message);

                if (File.Exists(targetZipFilePath))
                {
                    File.Delete(targetZipFilePath);
                }

                return false;
            }
            finally
            {
                if (File.Exists(sourceZipFilePath))
                {
                    File.Delete(sourceZipFilePath);
                }
            }
        }

        private static string ComputeHash(string filePath)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes);
        }
    }
}
