namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    internal class BiaAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly BiaNetSection biaNetSection = new ();

        public BiaAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration)
            : base(options)
        {
            configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var authenticationScheme = this.biaNetSection?.Authentication?.Keycloak?.IsActive == true ?
                AuthenticationConfiguration.JwtBearerIdentityProvider :
                NegotiateDefaults.AuthenticationScheme;

            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(authenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            return policy;
        }
    }
}
