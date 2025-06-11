// <copyright file="IUserAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using BIA.Net.Core.Application.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// The interface defining the application service for user.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public interface IUserAppService : IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory>
    {
    }
}