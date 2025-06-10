// <copyright file="UserFromDirectoryMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The mapper used from directory for user from directory dto.
    /// </summary>
    /// <typeparam name="TUserFromDirectory">Type of the user from directory.</typeparam>
    public static class UserFromDirectoryMapper<TUserFromDirectory>
        where TUserFromDirectory : IUserFromDirectory
    {
        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        public static Func<TUserFromDirectory, UserFromDirectoryDto> EntityToDto()
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