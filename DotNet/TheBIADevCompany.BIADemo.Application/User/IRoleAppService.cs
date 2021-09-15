// <copyright file="IRoleAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using BIA.Net.Core.Domain.Dto.Option;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for role.
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync();

        /// <summary>
        /// Get all member roles.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of roles for this member.</returns>
        Task<IEnumerable<OptionDefaultDto>> GetMemberRolesAsync(int siteId, int userId);
    }
}