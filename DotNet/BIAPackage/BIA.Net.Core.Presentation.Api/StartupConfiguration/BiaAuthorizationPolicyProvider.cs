// <copyright file="BiaAuthorizationPolicyProvider.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Authorization policy provider for BIA.
    /// </summary>
    internal class BiaAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly BiaNetSection biaNetSection = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaAuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="options">Authorization options.</param>
        /// <param name="configuration">Configuration.</param>
        public BiaAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration)
            : base(options)
        {
            configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <inheritdoc cref="DefaultAuthorizationPolicyProvider.GetPolicyAsync(string)"/>
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var authenticationScheme = this.biaNetSection?.Authentication?.Keycloak?.IsActive == true ?
                AuthenticationConfiguration.JwtBearerIdentityProvider :
                NegotiateDefaults.AuthenticationScheme;

            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(authenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            return Task.FromResult(policy);
        }
    }
}
