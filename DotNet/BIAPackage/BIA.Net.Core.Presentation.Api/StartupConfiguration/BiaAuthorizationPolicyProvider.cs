// <copyright file="BiaAuthorizationPolicyProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Authorization policy provider for BIA.
    /// </summary>
    public class BiaAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        /// <summary>
        /// The default authorization policy name for BIA.
        /// </summary>
        public const string DefaultBiaAuthorizationPolicyName = "BiaAuthorizationPolicy";

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
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var basePolicy = await base.GetPolicyAsync(policyName);
            if (basePolicy != null)
            {
                return basePolicy;
            }

            if (!policyName.Equals(DefaultBiaAuthorizationPolicyName))
            {
                throw new ElementNotFoundException($"Unable to find policy {policyName}.");
            }

            var authenticationScheme = this.biaNetSection?.Authentication?.Keycloak?.IsActive == true ?
                AuthenticationConfiguration.JwtBearerIdentityProvider :
                NegotiateDefaults.AuthenticationScheme;

            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(authenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        }
    }
}
