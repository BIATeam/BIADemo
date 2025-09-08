// BIADemo only
// <copyright file="IPilotAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The interface defining the application service for plane.
    /// </summary>
    public interface IPilotAppService : ICrudAppServiceBase<PilotDto, Pilot, Guid, PagingFilterFormatDto>
    {
    }
}