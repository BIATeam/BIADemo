// BIADemo only
// <copyright file="WithPermissionTask.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// Example of task lanched manualy with hangfire.
    /// </summary>
    [AutomaticRetry(Attempts = 0, LogEvents = true)]
    public class WithPermissionTask : BaseJob
    {
        private readonly IPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="WithPermissionTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">logger.</param>
        /// <param name="principal">the identity of the service account.</param>
        /// <param name="userDirectoryHelper">the user directory helper.</param>
        /// <param name="userPermissionDomainService">the user permission domain service.</param>
        public WithPermissionTask(
            IConfiguration configuration,
            ILogger<WithPermissionTask> logger,
            IPrincipal principal,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            IUserPermissionDomainService userPermissionDomainService)
            : base(configuration, logger)
        {
            this.principal = principal;

            // update permission with the service account roles.
            List<string> globalRoles = userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: principal as BiaClaimsPrincipal, userInfoDto: null, sid: null, domain: null).Result;
            List<string> userPermissions = userPermissionDomainService.TranslateRolesInPermissions(globalRoles, false);

            var newIdentity = (ClaimsIdentity)principal.Identity;
            foreach (string permission in userPermissions)
            {
                newIdentity.AddClaim(new Claim(ClaimTypes.Role, permission));
            }
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task RunMonitoredTask()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            bool accessAll = (principal as BiaClaimsPrincipal).GetUserPermissions().Any(x => x == BiaRights.Teams.AccessAll);

            if (accessAll)
            {
                var message = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Hello from the job WithPermissionTask WITH AccessAll.";
                Logger.LogInformation(message);
            }
            else
            {
                var message = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Hello from the job WithPermissionTask WHITHOUT AccessAll.";
                Logger.LogInformation(message);
            }
        }
    }
}
