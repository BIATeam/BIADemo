// BIADemo only
// <copyright file="IPlaneTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for plane type option.
    /// </summary>
    public interface IPlaneTypeOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}