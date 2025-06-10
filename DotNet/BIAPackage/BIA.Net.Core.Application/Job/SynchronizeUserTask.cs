// <copyright file="SynchronizeUserTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class SynchronizeUserTask<TUserDto, TUser, TUserFromDirectory> : BaseJob
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
        where TUserFromDirectory : IUserFromDirectory, new()
    {
        private readonly IBaseUserAppService<TUserDto, TUser, TUserFromDirectory> userService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection biaNetSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeUserTask{TUserDto, TUser, TUserFromDirectory}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public SynchronizeUserTask(IConfiguration configuration, IBaseUserAppService<TUserDto, TUser, TUserFromDirectory> userService, ILogger<SynchronizeUserTask<TUserDto, TUser, TUserFromDirectory>> logger)
            : base(configuration, logger)
        {
            this.userService = userService;
            this.biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            if (this.biaNetSection?.Authentication?.Keycloak?.IsActive == true)
            {
                await this.userService.SynchronizeWithIdpAsync();
            }
            else
            {
                await this.userService.SynchronizeWithADAsync(true);
            }
        }
    }
}
