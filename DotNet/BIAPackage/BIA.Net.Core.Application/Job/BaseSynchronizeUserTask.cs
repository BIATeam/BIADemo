// <copyright file="BaseSynchronizeUserTask.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Task to synchronize users from LDAP.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    public abstract class BaseSynchronizeUserTask<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> : BaseJob
        where TUserDto : BaseUserDto, new()
        where TUser : BaseUser, IEntity<int>, new()
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : IUserFromDirectory, new()
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> userService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection biaNetSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSynchronizeUserTask{TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public BaseSynchronizeUserTask(IConfiguration configuration, IBaseUserAppService<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory> userService, ILogger<BaseSynchronizeUserTask<TUserDto, TUser, TUserFromDirectoryDto, TUserFromDirectory>> logger)
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
