// <copyright file="UserFromDirectoryMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using BIA.Net.Core.Domain.Dto.User;

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
                Company = dto.Company,
                Country = dto.Country,
                Department = dto.Department,
                DistinguishedName = dto.DistinguishedName,
                Domain = dto.Domain,
                Email = dto.Email,
                ExternalCompany = dto.ExternalCompany,
                FirstName = dto.FirstName,
                Guid = dto.Guid,
                IsEmployee = dto.IsEmployee,
                IsExternal = dto.IsExternal,
                LastName = dto.LastName,
                Login = dto.Login,
                Manager = dto.Manager,
                Office = dto.Office,
                Sid = dto.Sid,
                Site = dto.Site,
                SubDepartment = dto.SubDepartment,
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
                Company = entity.Company,
                Country = entity.Country,
                Department = entity.Department,
                DistinguishedName = entity.DistinguishedName,
                Domain = entity.Domain,
                Email = entity.Email,
                ExternalCompany = entity.ExternalCompany,
                FirstName = entity.FirstName,
                Guid = entity.Guid,
                IsEmployee = entity.IsEmployee,
                IsExternal = entity.IsExternal,
                LastName = entity.LastName,
                Login = entity.Login,
                Manager = entity.Manager,
                Office = entity.Office,
                Sid = entity.Sid,
                Site = entity.Site,
                SubDepartment = entity.SubDepartment,
            };
        }
    }
}