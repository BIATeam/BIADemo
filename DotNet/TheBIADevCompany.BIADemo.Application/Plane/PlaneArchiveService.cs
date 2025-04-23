// BIADemo only
// <copyright file="PlaneArchiveService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

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
    }
}
