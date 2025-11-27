// <copyright file="IRoleAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for role option.
    /// </summary>
    public interface IRoleOptionAppService : IOptionAppServiceBase<OptionDto, int>
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <param name="teamTypeId">The team type id.</param>
        /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync(int teamTypeId);
    }
}