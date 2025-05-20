// BIADemo only
// <copyright file="ICountryOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for country option.
    /// </summary>
    public interface ICountryOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}