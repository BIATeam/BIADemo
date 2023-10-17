// BIADemo only
// <copyright file="IRemotePlaneAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface RemotePlane App Service.
    /// </summary>
    public interface IRemotePlaneAppService
    {
        /// <summary>
        /// Examples the call API asynchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExampleCallApiAsync();
    }
}