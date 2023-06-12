// <copyright file="HangfireAuthorizationFilter.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System.Linq;
    using System.Net;
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

        private readonly string userPermission;

        private readonly bool authorizeAllLocal;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireAuthorizationFilter"/> class.
        /// </summary>
        /// <param name="userAppService">Service to get user right.</param>
        /// <param name="authorizeAllLocal">True if local connection authorize all user.</param>
        /// <param name="userPermission">right to use.</param>
        public HangfireAuthorizationFilter(IUserAppService userAppService, bool authorizeAllLocal, string userPermission)
        {
            this.userAppService = userAppService;
            this.userPermission = userPermission;
            this.authorizeAllLocal = authorizeAllLocal;
        }

        /// <summary>
        /// Authorize use the role Hangfire_Dashboard.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <returns>true if user is authorized.</returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (this.authorizeAllLocal &&
                (httpContext.Connection.RemoteIpAddress.Equals(httpContext.Connection.LocalIpAddress) || IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress)))
            {
                return true;
            }

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Principal.WindowsIdentity)httpContext.User.Identity;
                var sid = identity.User.Value;
                var domain = identity.Name.Split('\\').FirstOrDefault();
                var userRolesFromUserDirectory = this.userAppService.GetUserDirectoryRolesAsync(false, sid, domain).Result;
                var userMainPermissions = this.userAppService.TranslateRolesInPermissions(userRolesFromUserDirectory);
                return userMainPermissions.Contains(this.userPermission);
            }
            else
            {
                return false;
            }
        }
    }
}
