// BIADemo only
// <copyright file="PlaneAirportRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// PlaneAirport Repository.
    /// </summary>
    public class PlaneAirportRepository : IPlaneAirportRepository
    {
        private readonly DbSet<PlaneAirport> planeAirportDbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAirportRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        public PlaneAirportRepository(IQueryableUnitOfWork unitOfWork)
        {
            this.planeAirportDbSet = unitOfWork.RetrieveSet<PlaneAirport>();
        }

        /// <inheritdoc/>
        public void Add(PlaneAirport planeAirport)
        {
            this.planeAirportDbSet.Add(planeAirport);
        }

        /// <inheritdoc/>
        public void Remove(PlaneAirport planeAirport)
        {
            this.planeAirportDbSet.Remove(planeAirport);
        }
    }
}
