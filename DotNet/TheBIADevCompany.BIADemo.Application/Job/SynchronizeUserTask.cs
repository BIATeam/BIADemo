// <copyright file="SynchronizeUserTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Job
{
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Domain.Dto.Base;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SynchronizeUserTask"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    /// <param name="userService">The user app service.</param>
    /// <param name="logger">logger.</param>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class SynchronizeUserTask(IConfiguration configuration, IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory, PagingFilterFormatDto> userService, ILogger<BaseSynchronizeUserTask<UserDto, User, UserFromDirectoryDto, UserFromDirectory, PagingFilterFormatDto>> logger)
        : BaseSynchronizeUserTask<UserDto, User, UserFromDirectoryDto, UserFromDirectory, PagingFilterFormatDto>(configuration, userService, logger)
    {
    }
}
