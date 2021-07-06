// <copyright file="HangfireAuthorizationFilter.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using Hangfire.Dashboard;
    using TheBIADevCompany.BIADemo.Application.User;

    /// <summary>
    /// Manage the authorisation to acced to the dashboard.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserAppService userAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireAuthorizationFilter"/> class.
        /// </summary>
        /// <param name="userAppService">Service to get user right.</param>
        public HangfireAuthorizationFilter(IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }

        /// <summary>
        /// Authorize use the role Hangfire_Dashboard.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <returns>true if user is authorized.</returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var sid = ((System.Security.Principal.WindowsIdentity)httpContext.User.Identity).User.Value;
                var userRolesFromUserDirectory = this.userAppService.GetUserDirectoryRolesAsync(sid).Result;
                var userMainRights = this.userAppService.TranslateRolesInRights(userRolesFromUserDirectory);
                return userMainRights.Contains("Hangfire_Dashboard");
            }
            else
            {
                return false;
            }
        }
    }
}
