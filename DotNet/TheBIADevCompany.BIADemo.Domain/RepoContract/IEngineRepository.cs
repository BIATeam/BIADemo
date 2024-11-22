// BIADemo only
// <copyright file="IEngineRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// Interface Engine Repository.
    /// </summary>
    public interface IEngineRepository : ITGenericRepository<Engine, int>
    {
        /// <summary>
        /// Fills the isToBeMaintained field asynchronous.
        /// </summary>
        /// <param name="nbMonth">The nb month.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task FillIsToBeMaintainedAsync(int nbMonth);

        /// <summary>
        /// TodoShouldBeImpossibleToDeclareDbSetInInetrefaceDomain.
        /// </summary>
        /// <returns>A DBSET.</returns>
        DbSet<Engine> TodoShouldBeImpossibleToDeclareDbSetInInetrefaceDomain();
    }
}