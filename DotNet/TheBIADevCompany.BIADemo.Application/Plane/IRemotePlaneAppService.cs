// BIADemo only
// <copyright file="IRemotePlaneAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// Interface RemotePlane App Service.
    /// </summary>
    public interface IRemotePlaneAppService
    {
        /// <summary>
        /// Examples the call API asynchronous.
        /// </summary>
        /// <returns>A plane.</returns>
        Task<Plane> ExampleCallApiAsync();
    }
}