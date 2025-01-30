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
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class ArchiveServiceBase<TEntity, TKey> : IArchiveService where TEntity : class, IEntityArchivable<TKey>
    {
        protected readonly BiaNetSection biaNetSection;
        protected readonly ITGenericArchiveRepository<TEntity, TKey> archiveRepository;
        protected readonly ILogger logger;
        private readonly bool enableDeleteStep;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        protected ArchiveServiceBase(IConfiguration configuration, ITGenericArchiveRepository<TEntity, TKey> archiveRepository, ILogger logger, bool enableDeleteStep = true)
        {
            this.biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(this.biaNetSection);

            this.archiveRepository = archiveRepository;
            this.logger = logger;
            this.enableDeleteStep = enableDeleteStep;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        public async Task RunAsync()
        {
            this.logger.Log(LogLevel.Information, $"Begin archive of {typeof(TEntity).Name} entity");
            await RunArchiveStepAsync();

            if (enableDeleteStep)
            {
                await RunDeleteStepAsync();
            }

            this.logger.Log(LogLevel.Information, $"End archive of {typeof(TEntity).Name} entity");
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

            var targetDirectoryPath = Path.Combine(this.biaNetSection.WorkerFeatures.Archive.TargetDirectoryPath, typeof(TEntity).Name);
            var targetFileName = $"{item.Id}_{DateTime.Now:yyyyMMddHHmmss}.json";
            var targetFilePath = Path.Combine(targetDirectoryPath, targetFileName);
            var sourceFilePath = Path.GetRandomFileName();

            try
            {
                if(!Directory.Exists(targetDirectoryPath))
                {
                    Directory.CreateDirectory(targetDirectoryPath);
                }

                var targetDirectoryFiles = Directory.EnumerateFiles(targetDirectoryPath);
                if (targetDirectoryFiles.Any(f => Path.GetFileNameWithoutExtension(f).Split('_').FirstOrDefault() == item.Id.ToString()))
                {
                    throw new Exception("archive already exists");
                }

                await File.WriteAllTextAsync(sourceFilePath, JsonConvert.SerializeObject(item, this.jsonSerializerSettings));
                var sourceHash = ComputeHash(sourceFilePath);

                File.Copy(sourceFilePath, targetFilePath);

                var targetHash = ComputeHash(targetFilePath);
                if (targetHash != sourceHash)
                {
                    throw new Exception("integrity compromised after copy");
                }

                this.logger.Log(LogLevel.Information, $"Item {item.Id} : saved to server successfully");
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

        public static string ComputeHash(string filePath)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes);
        }
    }
}
