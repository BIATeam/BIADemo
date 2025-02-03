// <copyright file="PlaneArchiveRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.ArchiveRepositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
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
        protected override Expression<Func<Plane, bool>> ArchiveStepItemsSelector()
        {
            return base.ArchiveStepItemsSelector().CombineSelector(x => !x.IsActive);
        }
    }
}
