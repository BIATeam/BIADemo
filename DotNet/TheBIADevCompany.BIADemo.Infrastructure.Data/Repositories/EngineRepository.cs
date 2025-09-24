// BIADemo only
// <copyright file="EngineRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Engine Repository.
    /// </summary>
    public class EngineRepository : TGenericRepositoryEF<Engine, int>, IEngineRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="auditFeature">The audit feature.</param>
        public EngineRepository(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAuditFeature auditFeature)
            : base(unitOfWork, serviceProvider, auditFeature)
        {
        }

        /// <inheritdoc />
        public async Task FillIsToBeMaintainedAsync(int nbMonth)
        {
            await this.RetrieveSetNoTracking()
                .Where(x => SqlServerDbFunctionsExtensions.DateDiffDay(EF.Functions, x.LastMaintenanceDate, DateTime.Today) > nbMonth)
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.IsToBeMaintained, true));

            await this.RetrieveSetNoTracking()
                .Where(x => SqlServerDbFunctionsExtensions.DateDiffDay(EF.Functions, x.LastMaintenanceDate, DateTime.Today) <= nbMonth)
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.IsToBeMaintained, false));
        }
    }
}
