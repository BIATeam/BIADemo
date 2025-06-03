// <copyright file="SynchronizeUserTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.User.Entities;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Bia.User;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    [AutomaticRetry(Attempts = 2, LogEvents = true)]
    public class SynchronizeUserTask<TUserDto, TUser> : BaseJob
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
    {
        private readonly IBaseUserAppService<TUserDto, TUser> userService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection biaNetSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeUserTask{TUserDto, TUser}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public SynchronizeUserTask(IConfiguration configuration, IBaseUserAppService<TUserDto, TUser> userService, ILogger<SynchronizeUserTask<TUserDto, TUser>> logger)
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
