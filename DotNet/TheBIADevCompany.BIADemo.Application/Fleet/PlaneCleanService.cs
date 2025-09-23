// BIADemo only
// <copyright file="PlaneCleanService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Application.Clean;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Clean service for Planes.
    /// </summary>
    public class PlaneCleanService : CleanServiceBase<Plane, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneCleanService"/> class.
        /// </summary>
        /// <param name="cleanRepository">The clean repository for the entity.</param>
        /// <param name="logger">The logger.</param>
        public PlaneCleanService(ITGenericCleanRepository<Plane, int> cleanRepository, ILogger<PlaneCleanService> logger)
            : base(cleanRepository, logger)
        {
        }

        /// <inheritdoc/>
        protected override Expression<Func<Plane, bool>> CleanRuleFilter()
        {
            var currentDateTime = DateTime.UtcNow;
            return x => x.IsArchived && x.ArchivedDate.Value.AddYears(1) < currentDateTime;
        }
    }
}
