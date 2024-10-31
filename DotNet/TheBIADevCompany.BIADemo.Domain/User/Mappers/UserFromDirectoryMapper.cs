// <copyright file="UserFromDirectoryMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// The mapper used from directory for user from directory dto.
    /// </summary>
    public static class UserFromDirectoryMapper
    {
        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        public static Func<UserFromDirectory, UserFromDirectoryDto> EntityToDto()
        {
            return entity => new UserFromDirectoryDto
            {
                // If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
                IdentityKey = entity.Login,
                DisplayName = entity.LastName + " " + entity.FirstName + "(" + entity.Domain + "\\" + entity.Login + ")",
                Domain = entity.Domain,
            };
        }
    }
}