// <copyright file="PlaneArchiveService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The <see cref="Plane"/> entity archive service.
    /// </summary>
    public class PlaneArchiveService : ArchiveServiceBase<Plane, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneArchiveService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="archiveRepository">The <see cref="ITGenericArchiveRepository{TEntity, TKey}"/> archive repository.</param>
        /// <param name="logger">The logger.</param>
        //public PlaneArchiveService(IConfiguration configuration, IPlaneArchiveRepository archiveRepository, ILogger<PlaneArchiveService> logger
        public PlaneArchiveService(IConfiguration configuration, ITGenericArchiveRepository<Plane, int> archiveRepository, ILogger<PlaneArchiveService> logger)

            : base(configuration, archiveRepository, logger)
        {
        }

        /// <inheritdoc/>
        protected override string GetArchiveNameTemplate(Plane entity)
        {
            return $"plane_{entity.Msn}";
        }
    }
}
