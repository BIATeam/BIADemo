// BIADemo only
// <copyright file="PlaneArchiveService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.IO.Compression;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Archive service for plane entity.
    /// </summary>
    public class PlaneArchiveService : ArchiveServiceBase<Plane, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneArchiveService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="archiveRepository">The archive repository.</param>
        /// <param name="logger">The logger.</param>
        public PlaneArchiveService(IConfiguration configuration, ITGenericArchiveRepository<Plane, int> archiveRepository, ILogger<PlaneArchiveService> logger)
            : base(configuration, archiveRepository, logger)
        {
        }

        /// <inheritdoc/>
        protected override string GetArchiveNameTemplate(Plane entity)
        {
            return $"plane_{entity.Msn}";
        }

        /// <inheritdoc/>
        protected override Task<string> SerializeItem(Plane item)
        {
            // Customize the content before serializing the item to archive, or return your own JSON
            return base.SerializeItem(item);
        }

        /// <inheritdoc/>
        protected override async Task AddEntriesToArchiveAsync(Plane item, ZipArchive archive)
        {
            // Fill here the list of additional entries to add to the archive
            await this.AppendEntriesToArchiveAsync(archive, []);
        }
    }
}
