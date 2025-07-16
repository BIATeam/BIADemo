// BIADemo only
// <copyright file="IRemotePlaneAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface RemotePlane App Service.
    /// </summary>
    public interface IRemotePlaneAppService
    {
        /// <summary>
        /// Checks if remote plane exist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return true if plane exist.</returns>
        Task<bool> CheckExistAsync(int id);

        /// <summary>
        /// Creates a Plane.
        /// </summary>
        /// <returns>Return true if plane created.</returns>
        Task<bool> CreateAsync();
    }
}