// <copyright file="UserRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories
{
    using System;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Provides a repository implementation for managing <see cref="User"/> entities, supporting data access operations
    /// within a unit of work context.
    /// </summary>
    public class UserRepository : BaseUserRepository<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class using the specified unit of work and service.
        /// provider.
        /// </summary>
        /// <param name="unitOfWork">The unit of work that manages database transactions and queries for the repository. Cannot be null.</param>
        /// <param name="serviceProvider">The service provider used to resolve dependencies required by the repository. Cannot be null.</param>
        public UserRepository(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider)
            : base(unitOfWork, serviceProvider)
        {
        }
    }
}
