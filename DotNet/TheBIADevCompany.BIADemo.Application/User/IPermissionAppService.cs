// <copyright file="IPermissionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for permission.
    /// </summary>
    public interface IPermissionAppService
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync();

        /// <summary>
        /// Return list of ids of the translated permissions.
        /// </summary>
        /// <param name="permissions">the permission at string format.</param>
        /// <returns>List of id.</returns>
        IEnumerable<int> GetPermissionsIds(List<string> permissions);
    }
}