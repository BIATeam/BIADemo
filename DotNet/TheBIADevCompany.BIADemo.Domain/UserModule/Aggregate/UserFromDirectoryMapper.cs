// <copyright file="UserFromDirectoryMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used from directory for user from directory dto.
    /// </summary>
    public static class UserFromDirectoryMapper
    {
        /// <summary>
        /// Create a member entity from a DTO.
        /// </summary>
        /// <returns>The user Entity.</returns>
        public static Func<UserFromDirectoryDto, UserFromDirectory> DtoToEntity()
        {
            return dto => new UserFromDirectory
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Login = dto.Login,
                Domain = dto.Domain,
                Guid = dto.Guid,
                Sid = dto.Sid,
            };
        }

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        public static Func<UserFromDirectory, UserFromDirectoryDto> EntityToDto()
        {
            return entity => new UserFromDirectoryDto
            {
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Login = entity.Login,
                Domain = entity.Domain,
                Guid = entity.Guid,
                Sid = entity.Sid,
            };
        }
    }
}