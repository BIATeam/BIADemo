// <copyright file="PlaneArchiveRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.ArchiveRepositories
{
    using System.Linq;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Archive repository for <see cref="Plane"/> entity.
    /// </summary>
    public class PlaneArchiveRepository : TGenericArchiveRepository<Plane, int>, IPlaneArchiveRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneArchiveRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The <see cref="IQueryableUnitOfWork"/> context.</param>
        public PlaneArchiveRepository(IQueryableUnitOfWork dataContext)
            : base(dataContext)
        {
        }

        /// <inheritdoc/>
        protected override IQueryable<Plane> GetAllWithIncludes()
        {
            return base.GetAllWithIncludes()
                .Include(p => p.ConnectingAirports)
                .Include(p => p.ConnectingPlaneAirports)
                .Include(p => p.PlaneType)
                .Include(p => p.SimilarPlaneType)
                .Include(p => p.Engines)
                .ThenInclude(e => e.InstalledParts)
                .Include(p => p.Engines)
                .ThenInclude(e => e.PrincipalPart);
        }
    }
}
