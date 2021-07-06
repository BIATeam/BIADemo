// BIADemo only
// <copyright file="IPlaneAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPlaneAppService : ICrudAppServiceBase<PlaneDto, Plane, LazyLoadDto>
    {
        /// <summary>
        /// Return a range to use in Calc SpreadSheet.
        /// </summary>
        /// <param name="filters">The filter.</param>
        /// <returns><see cref="Task"/>Representing the asynchronous operation.</returns>
        Task<(IEnumerable<PlaneDto> Results, int Total)> GetRangeForCalcAsync(LazyLoadDto filters = null);
    }
}