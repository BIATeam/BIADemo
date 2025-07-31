// <copyright file="UsersController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
#pragma warning disable BIA001 // Forbidden reference to Domain layer in Presentation layer
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#pragma warning restore BIA001 // Forbidden reference to Domain layer in Presentation layer
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.User;

    /// <inheritdoc />
    public class UsersController : BaseUsersController<UserDto, User, UserFromDirectoryDto, UserFromDirectory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="configuration">The configuration.</param>
        public UsersController(IUserAppService userService, IOptions<BiaNetSection> configuration)
            : base(userService, configuration)
        {
        }
    }
}
