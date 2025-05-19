// BIADemo only
// <copyright file="IEngineAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    // Begin BIADemo
    using System.Threading.Tasks;

    // End BIADemo
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The interface defining the application service for engine.
    /// </summary>
    public interface IEngineAppService : ICrudAppServiceBase<EngineDto, Engine, int, PagingFilterFormatDto>
    {
        // Begin BIADemo

        /// <summary>
        /// Checks engine to be maintained asynchronous.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task CheckToBeMaintainedAsync();

        /// <summary>
        /// Launches the job manually (example).
        /// </summary>
        void LaunchJobManuallyExample();

        // End BIADemo
    }
}