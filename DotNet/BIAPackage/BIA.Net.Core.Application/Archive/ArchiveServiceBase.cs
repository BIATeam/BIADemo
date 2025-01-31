namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class ArchiveServiceBase<TEntity, TKey> : IArchiveService where TEntity : class, IEntityArchivable<TKey>
    {
        protected readonly ArchiveEntityConfiguration archiveEntityConfiguration;
        protected readonly ITGenericArchiveRepository<TEntity, TKey> archiveRepository;
        protected readonly ILogger logger;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        protected ArchiveServiceBase(IConfiguration configuration, ITGenericArchiveRepository<TEntity, TKey> archiveRepository, ILogger logger)
        {
            var biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(biaNetSection);
            this.archiveEntityConfiguration = biaNetSection.WorkerFeatures?.Archive?.ArchiveEntityConfigurations.FirstOrDefault(x => x.IsValid && x.IsMatchingEntityType(typeof(TEntity)));

            this.archiveRepository = archiveRepository;
            this.logger = logger;

            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        protected abstract string GetArchiveNameTemplate(TEntity entity);

        public virtual async Task RunAsync()
        {
            try
            {
                this.logger.Log(LogLevel.Information, $"Begin archive of {typeof(TEntity).Name} entity");

                if (this.archiveEntityConfiguration is null)
                {
                    this.logger.Log(LogLevel.Warning, $"No valid archive configuration found for the entity {typeof(TEntity).Name}.");
                    return;
                }

                await RunArchiveStepAsync();

                if (this.archiveEntityConfiguration.EnableDeleteStep)
                {
                    await RunDeleteStepAsync();
                }
            }
            finally
            {
                this.logger.Log(LogLevel.Information, $"End archive of {typeof(TEntity).Name} entity");
            }
        }

        protected virtual async Task RunDeleteStepAsync()
        {
            var items = await this.archiveRepository.GetItemsToDeleteAsync();
            foreach (var item in items)
            {
                await DeleteItemAsync(item);
            }
        }

        protected virtual async Task DeleteItemAsync(TEntity item)
        {
            try
            {
                this.logger.Log(LogLevel.Information, $"Item {item.Id} : deleting");
                await this.archiveRepository.RemoveAsync(item);

                this.logger.Log(LogLevel.Information, $"Item {item.Id} : deleted successfully");
            }
            catch(Exception ex)
            {
                this.logger.Log(LogLevel.Error, $"Item {item.Id} : failed to delete : {ex.Message}");
            }
        }

        protected virtual async Task RunArchiveStepAsync()
        {
            var items = await this.archiveRepository.GetItemsToArchiveAsync();
            foreach (var item in items)
            {
                await ArchiveItemAsync(item);
            }
        }

        protected virtual async Task ArchiveItemAsync(TEntity item)
        {
            this.logger.Log(LogLevel.Information, $"Item {item.Id} : archiving");

            try
            {
                if (await SaveItemToServerAsync(item))
                {
                    await this.archiveRepository.UpdateArchiveStateAsync(item, ArchiveStateEnum.Archived);
                    this.logger.Log(LogLevel.Information, $"Item {item.Id} : archived successfully");
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, $"Item {item.Id} : failed to archive : {ex.Message}");
            }
        }

        protected virtual async Task<bool> SaveItemToServerAsync(TEntity item)
        {
            this.logger.Log(LogLevel.Information, $"Item {item.Id} : saving to server");

            var targetFileName = $"{GetArchiveNameTemplate(item)}.json";
            var targetFilePath = Path.Combine(this.archiveEntityConfiguration.TargetDirectoryPath, targetFileName);
            var sourceFilePath = Path.GetRandomFileName();

            try
            {
                await CopyToServerAsync(item, targetFilePath, sourceFilePath);

                this.logger.Log(LogLevel.Information, $"Item {item.Id} : saved to server successfully into {targetFilePath}");
                return true;
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, $"Item {item.Id} : failed to save to server : {ex.Message}");

                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                return false;
            }
            finally
            {
                if (File.Exists(sourceFilePath))
                {
                    File.Delete(sourceFilePath);
                }
            }
        }

        protected virtual async Task CopyToServerAsync(TEntity item, string targetFilePath, string sourceFilePath)
        {
            var targerDirectoryPath = Path.GetDirectoryName(targetFilePath);
            if (!Directory.Exists(targerDirectoryPath))
            {
                Directory.CreateDirectory(targerDirectoryPath);
            }

            await File.WriteAllTextAsync(sourceFilePath, JsonConvert.SerializeObject(item, this.jsonSerializerSettings));
            var sourceHash = ComputeHash(sourceFilePath);

            File.Copy(sourceFilePath, targetFilePath, true);

            var targetHash = ComputeHash(targetFilePath);
            if (targetHash != sourceHash)
            {
                throw new Exception("integrity compromised after copy");
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
