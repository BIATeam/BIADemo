// <copyright file="IBiaDemoReadOnlyRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// Interface for implementations of repositories using BIA Demo read only database configuration.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public interface IBiaDemoReadOnlyRepository<TEntity, TKey> : ITGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}
