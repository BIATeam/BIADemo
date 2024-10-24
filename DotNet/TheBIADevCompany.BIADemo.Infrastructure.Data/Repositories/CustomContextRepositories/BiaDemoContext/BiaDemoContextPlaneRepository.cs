// <copyright file="BiaDemoContextPlaneRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.CustomContextRepositories.BiaDemoContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Repository for <see cref="Plane"/> in BIA Demo context.
    /// </summary>
    public class BiaDemoContextPlaneRepository : CustomContextRepositoryBase<Plane, int>, IBiaDemoContextPlaneRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDemoContextPlaneRepository"/> class.
        /// </summary>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public BiaDemoContextPlaneRepository(DataContextFactory dataContextFactory, IServiceProvider serviceProvider)
            : base(dataContextFactory, serviceProvider, "BIADemoDatabase")
        {
        }
    }
}
