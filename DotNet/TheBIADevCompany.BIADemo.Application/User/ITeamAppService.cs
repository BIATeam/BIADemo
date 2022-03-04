// <copyright file="ITeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for team.
    /// </summary>
    public interface ITeamAppService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userPermissions">The user rights.</param>
        /// <returns>all teams.</returns>
        Task<IEnumerable<TeamDto>> GetAllAsync(int userId = 0, IEnumerable<string> userPermissions = null);
    }
}