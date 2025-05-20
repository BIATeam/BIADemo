// BIADemo only
// <copyright file="IRemotePlaneRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Interface RemotePlaneRepository.
    /// </summary>
    public interface IRemotePlaneRepository
    {
        /// <summary>
        /// Deletes a plane.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns true if the deletion worked.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Gets a plane.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>returns the plane corresponding to the Id.</returns>
        Task<Plane> GetAsync(int id);

        /// <summary>
        /// Create a plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns>returns the created plane.</returns>
        Task<Plane> PostAsync(Plane plane);

        /// <summary>
        /// Update a plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns>returns the modified plane.</returns>
        Task<Plane> PutAsync(Plane plane);
    }
}