// <copyright file="IUserFromDirectoryMapper.cs" company="BIA">
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
    /// <typeparam name="TUserFromDirectoryDto">Type of the user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">Type of the user from directory.</typeparam>
    public interface IUserFromDirectoryMapper<TUserFromDirectoryDto, TUserFromDirectory>
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : IUserFromDirectory
    {
        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        Func<TUserFromDirectory, TUserFromDirectoryDto> EntityToDto();
    }
}