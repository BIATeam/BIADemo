// <copyright file="BiaAuthorizeFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Filters
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// BIA authorize filter.
    /// </summary>
    public class BiaAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private const string AuthenticationSchemesNoToken = "NoToken";
        private const string AuthenticationSchemesLDP = "LDP";

        private readonly BiaNetSection biaNetSection = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaAuthorizeFilter"/> class.
        /// </summary>
        /// <param name="configuration">The injected configuration.</param>
        public BiaAuthorizeFilter(IConfiguration configuration)
        {
            configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <inheritdoc cref="IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext)"/>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authenticationSchemes = this.biaNetSection?.Authentication?.AuthenticationSchemes switch
            {
                AuthenticationSchemesLDP => AuthenticationConfiguration.JwtBearerIdentityProvider,
                AuthenticationSchemesNoToken => NegotiateDefaults.AuthenticationScheme,
                _ => NegotiateDefaults.AuthenticationScheme
            };

            var authorizeFilter = new AuthorizeFilter(
                new AuthorizationPolicyBuilder(authenticationSchemes)
                    .RequireAuthenticatedUser()
                    .Build());

            await authorizeFilter.OnAuthorizationAsync(context);
        }
    }
}
