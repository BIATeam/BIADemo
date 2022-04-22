// <copyright file="HangfireAuthorizationFilter.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Net;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using Hangfire.Dashboard;
    using TheBIADevCompany.BIADemo.Application.User;

    /// <summary>
    /// Manage the authorisation to acced to the dashboard.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private static readonly string HangFireCookieName = "HangFireCookie";
        private readonly bool authorizeAllLocal;
        private readonly string userPermission;
        private readonly string secretKey;
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireAuthorizationFilter"/> class.
        /// </summary>
        /// <param name="authorizeAllLocal">True if local connection authorize all user.</param>
        /// <param name="userPermission">right to use.</param>
        /// <param name="secretKey">the secret Key.</param>
        public HangfireAuthorizationFilter(bool authorizeAllLocal, string userPermission, string secretKey, IJwtFactory jwtFactory)
        {
            this.userPermission = userPermission;
            this.authorizeAllLocal = authorizeAllLocal;
            this.secretKey = secretKey;
            this.jwtFactory = jwtFactory;
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

            var access_token = httpContext.Request.Cookies[HangFireCookieName];

            if (string.IsNullOrEmpty(access_token))
            {
                return false;
            }

            try
            {
                var principal = this.jwtFactory.GetPrincipalFromToken(access_token, secretKey);
                if (!string.IsNullOrEmpty(this.userPermission) && !principal.IsInRole(this.userPermission))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
    }
}
