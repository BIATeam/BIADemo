// BIADemo only
// <copyright file="IPlaneTypeAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPlaneTypeAppService : ICrudAppServiceBase<PlaneTypeDto, PlaneType, int, PagingFilterFormatDto>
    {
    }
}