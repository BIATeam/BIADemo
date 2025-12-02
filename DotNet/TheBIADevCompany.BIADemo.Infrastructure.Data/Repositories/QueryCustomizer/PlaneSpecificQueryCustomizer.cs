// <copyright file="PlaneSpecificQueryCustomizer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer;

    /// <summary>
    /// Provides query customization for Plane entities.
    /// </summary>
    public class PlaneSpecificQueryCustomizer : TQueryCustomizer<Plane>, IPlaneSpecificQueryCustomizer
    {
        /// <inheritdoc/>
        public override IQueryable<Plane> CustomizeBefore(IQueryable<Plane> objectSet, string queryMode)
        {
            return queryMode switch
            {
                QueryMode.Read => objectSet
                    .Include(p => p.Engines)
                        .ThenInclude(e => e.PrincipalPart)
                    .Include(p => p.Engines)
                        .ThenInclude(e => e.InstalledParts),
                _ => objectSet,
            };
        }
    }
}
