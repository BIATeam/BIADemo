// <copyright file="UserFromDirectoryMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using BIA.Net.Core.Domain.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// The mapper used from directory for user from directory dto.
    /// </summary>
    /// <typeparam name="TUserFromDirectoryDto">Type of the user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">Type of the user from directory.</typeparam>
    public class UserFromDirectoryMapper : IUserFromDirectoryMapper<UserFromDirectoryDto, UserFromDirectory>
    {
        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        public Func<UserFromDirectory, UserFromDirectoryDto> EntityToDto()
        {
            return entity => new UserFromDirectoryDto
            {
                // If you change it parse all other #IdentityKey to align all (Database, Ldap, Idp, WindowsIdentity).
                IdentityKey = entity.Login,
                DisplayName = entity.LastName + " " + entity.FirstName + "(" + entity.Domain + "\\" + entity.Login + ")",
                Domain = entity.Domain,
            };
        }
    }
}