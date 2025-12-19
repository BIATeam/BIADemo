// <copyright file="ArchiveServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Entity.Interface;
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
        where TEntity : class, IEntityArchivable, IEntity<TKey>
    {
        /// <summary>
        /// The JSON serializer settings.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected readonly JsonSerializerSettings jsonSerializerSettings;
#pragma warning restore SA1401 // Fields should be private

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
        public virtual async Task RunAsync()
        {
            try
            {
                this.Logger.LogInformation("Begin archive of {EntityTypeName} entity", typeof(TEntity).Name);

                if (this.ArchiveEntityConfiguration is null)
                {
                    this.Logger.LogWarning("No valid archive configuration found for the entity {EntityTypeName}.", typeof(TEntity).Name);
                    return;
                }

                var items = await this.ArchiveRepository.GetAllAsync(this.ArchiveRuleFilter());
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
        /// Append additional entries to the given <paramref name="archive"/>.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="archiveEntries">Optionnal entries to add to the archive.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        protected async Task AppendEntriesToArchiveAsync(ZipArchive archive, IEnumerable<ArchiveEntry> archiveEntries)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            foreach (var archiveEntry in archiveEntries)
            {
                try
                {
                    var entry = archive.CreateEntry(archiveEntry.EntryName);
                    await using var entryStream = entry.Open();
                    await archiveEntry.ContentStream.CopyToAsync(entryStream);
                }
                catch (Exception ex)
                {
                    throw new JobException($"Error when adding entry {archiveEntry.EntryName} into archive : {ex.Message}", ex);
                }
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
        protected virtual Expression<Func<TEntity, bool>> ArchiveRuleFilter()
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
                if (await this.CreateArchiveAsync(item, this.ArchiveEntityConfiguration.TargetDirectoryPath))
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
        /// <param name="targetArchiveDirectoryPath">Target directory path.</param>
        /// <returns><see cref="Task{bool}"/> that indicates success.</returns>
        protected virtual async Task<bool> CreateArchiveAsync(TEntity item, string targetArchiveDirectoryPath)
        {
            var targetArchiveFileName = $"{this.GetArchiveNameTemplate(item)}.zip";
            var targetArchiveFilePath = Path.Combine(targetArchiveDirectoryPath, targetArchiveFileName);
            var sourceArchiveFilePath = Path.GetRandomFileName();

            try
            {
                var jsonContent = await this.SerializeItem(item);

                if (!Directory.Exists(targetArchiveDirectoryPath))
                {
                    Directory.CreateDirectory(targetArchiveDirectoryPath);
                }

                await using (var zipStream = new FileStream(sourceArchiveFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: false);
                    var jsonEntry = archive.CreateEntry(targetArchiveFileName.Replace(".zip", ".json"));
                    await using (var entryStream = jsonEntry.Open())
                    {
                        await using var writer = new StreamWriter(entryStream);
                        await writer.WriteAsync(jsonContent);
                    }

                    await this.AddEntriesToArchiveAsync(item, archive);
                }

                var sourceHash = ComputeHash(sourceArchiveFilePath);

                File.Copy(sourceArchiveFilePath, targetArchiveFilePath, true);

                var targetHash = ComputeHash(targetArchiveFilePath);
                if (targetHash != sourceHash)
                {
                    throw new JobException("Integrity compromised after copy");
                }

                return true;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to save item {ItemId} : {ExceptionMessage}", item.Id, ex.Message);

                if (File.Exists(targetArchiveFilePath))
                {
                    File.Delete(targetArchiveFilePath);
                }

                return false;
            }
            finally
            {
                if (File.Exists(sourceArchiveFilePath))
                {
                    File.Delete(sourceArchiveFilePath);
                }
            }
        }

        /// <summary>
        /// Serialize the <typeparamref name="TEntity"/> item.
        /// </summary>
        /// <param name="item">The item to serialize.</param>
        /// <returns>A <see cref="Task"/> resulting of the serialized item as <see cref="string"/>.</returns>
        protected virtual Task<string> SerializeItem(TEntity item)
        {
            return Task.FromResult(JsonConvert.SerializeObject(item, this.jsonSerializerSettings));
        }

        /// <summary>
        /// Override this method to allow the add of new <see cref="ArchiveEntry"/> entries to the given <paramref name="archive"/> based on current <typeparamref name="TEntity"/> item when creating the archive into <see cref="CreateArchiveAsync(TEntity, string)"/>.
        /// </summary>
        /// <param name="item">Current <typeparamref name="TEntity"/> item to archive.</param>
        /// <param name="archive">The archive.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        protected virtual Task AddEntriesToArchiveAsync(TEntity item, ZipArchive archive)
        {
            return Task.CompletedTask;
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
