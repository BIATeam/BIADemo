// <copyright file="AirportCleanService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Application.Clean;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// Clean service for airports.
    /// </summary>
    public class AirportCleanService : CleanServiceBase<Airport, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportCleanService"/> class.
        /// </summary>
        /// <param name="cleanRepository">The clean repository for the entity.</param>
        /// <param name="logger">The logger.</param>
        public AirportCleanService(ITGenericCleanRepository<Airport, int> cleanRepository, ILogger<AirportCleanService> logger)
            : base(cleanRepository, logger)
        {
        }

        /// <inheritdoc/>
        protected override Expression<Func<Airport, bool>> CleanRuleFilter()
        {
            return x => x.Name == "TLS";
        }
    }
}
