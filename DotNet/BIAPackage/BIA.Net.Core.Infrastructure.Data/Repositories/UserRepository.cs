// <copyright file="UserRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides a repository for managing user entities, supporting operations such as retrieving user information by
    /// login. Inherits standard data access functionality for entities identified by an integer key.
    /// </summary>
    /// <typeparam name="TUserEntity">The type of user entity managed by the repository. Must inherit from <see cref="BaseEntityUser"/>.</typeparam>
    public sealed class UserRepository<TUserEntity> : TGenericRepositoryEF<TUserEntity, int>, IUserRepository<TUserEntity>
        where TUserEntity : BaseEntityUser
    {
        private readonly IQueryableUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository{TUserEntity}"/> class using the specified unit of work and service.
        /// provider.
        /// </summary>
        /// <param name="unitOfWork">The unit of work that manages database transactions and provides access to queryable data sources. Cannot be
        /// null.</param>
        /// <param name="serviceProvider">The service provider used to resolve application services and dependencies required by the repository.
        /// Cannot be null.</param>
        public UserRepository(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider)
            : base(unitOfWork, serviceProvider)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GetUserFullNamesPerLogins(IEnumerable<string> logins)
        {
            var userFullNames = this.unitOfWork.RetrieveSet<TUserEntity>()
                .AsNoTracking()
                .Where(x => logins.Contains(x.Login))
                .Select(x => new { x.FirstName, x.LastName, x.Login });

            return await userFullNames.ToDictionaryAsync(user => user.Login, user => $"{user.LastName} {user.FirstName}");
        }
    }
}
