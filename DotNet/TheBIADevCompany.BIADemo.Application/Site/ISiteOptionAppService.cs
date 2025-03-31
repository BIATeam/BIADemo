// BIADemo only
// <copyright file="ISiteOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The interface defining the application service for site option.
    /// </summary>
    public interface ISiteOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
    }
}
