// BIADemo only
// <copyright file="IPlaneOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for plane option.
    /// </summary>
    public interface IPlaneOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}
