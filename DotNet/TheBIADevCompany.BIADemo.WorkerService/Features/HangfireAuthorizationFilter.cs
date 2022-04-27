// <copyright file="HangfireAuthorizationFilter.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Linq;
    using System.Net;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Http;


    /// <summary>
    /// Manage the authorisation to acced to the dashboard.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
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
        /// <param name="jwtFactory">the jwtFactory.</param>
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
            bool isLocal = httpContext.Connection.RemoteIpAddress.Equals(httpContext.Connection.LocalIpAddress) || IPAddress.IsLoopback(httpContext.Connection.RemoteIpAddress);
            if (this.authorizeAllLocal && isLocal)
            {
                return true;
            }

            if (string.IsNullOrEmpty(this.userPermission))
            {
                return false;
            }

            var jwtToken = string.Empty;

            if (httpContext.Request.Query.ContainsKey("jwt_token"))
            {
                jwtToken = httpContext.Request.Query["jwt_token"].FirstOrDefault();
                this.SetCookie(httpContext, jwtToken, httpContext.Request.Host.Value == "localhost");
            }
            else
            {
                jwtToken = httpContext.Request.Cookies["_hangfireCookie"];
            }

            if (string.IsNullOrEmpty(jwtToken))
            {
                return false;
            }

            var principal = this.jwtFactory.GetPrincipalFromToken(jwtToken, this.secretKey);
            if (principal.IsInRole(this.userPermission))
            {
                return true;
            }

            return false;
        }

        private void SetCookie(HttpContext httpContext, string jwtToken, bool isLocalhost)
        {
            httpContext.Response.Cookies.Append(
                "_hangfireCookie",
                jwtToken,
                new CookieOptions()
                {
                    Secure = !isLocalhost, // TODO : for not localhost
                    SameSite = isLocalhost ? SameSiteMode.Unspecified: SameSiteMode.None, // TODO : for not localhost
                    Path = "/",
                    IsEssential = true,
                    Expires = DateTime.Now.AddMinutes(30),
                });
        }
    }
}
