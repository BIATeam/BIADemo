// BIADemo only
// <copyright file="IAirportOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for airport option.
    /// </summary>
    public interface IAirportOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}