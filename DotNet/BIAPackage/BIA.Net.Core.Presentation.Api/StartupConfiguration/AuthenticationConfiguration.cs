// <copyright file="AuthenticationConfiguration.cs" company="BIA">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.StartupConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// The JWT bearer default.
        /// </summary>
        public const string JwtBearerDefault = "Default";

        /// <summary>
        /// The JWT bearer keycloak.
        /// </summary>
        public const string JwtBearerIdentityProvider = "IdentityProvider";

        /// <summary>
        /// Configure the authentication.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void ConfigureAuthentication(this IServiceCollection services, BiaNetSection configuration)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.Jwt.SecretKey));

            // Configure JwtIssuerOptions
            services.Configure<Jwt>(options =>
            {
                options.Issuer = configuration.Jwt.Issuer;
                options.Audience = configuration.Security?.Audience;
                options.SecretKey = configuration.Jwt.SecretKey;
                options.ValidTime = configuration.Jwt.ValidTime;
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration.Jwt.Issuer,

                ValidateAudience = true,
                ValidAudience = configuration.Security?.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var jwtBearerEvents = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.OnStarting(async () =>
                    {
                        context.NoResult();
                        context.Response.Headers.Append("Token-Expired-Or-Invalid", "true");
                        context.Response.ContentType = "text/plain";
                        context.Response.StatusCode = 498; // 498 = Token expired/invalid
                        await context.Response.WriteAsync("Un-Authorized");
                    });

                    return Task.CompletedTask;
                },
            };

            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefault, configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration.Jwt.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.Events = jwtBearerEvents;
            });

            if (configuration?.Authentication?.Keycloak?.IsActive == true)
            {
                IdentityModelEventSource.ShowPII = true;

                authenticationBuilder.AddJwtBearer(JwtBearerIdentityProvider, o =>
                {
                    if (configuration != null)
                    {
                        if (configuration.Security?.DisableTlsVerify == true)
                        {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
                            o.BackchannelHttpHandler = new System.Net.Http.HttpClientHandler { ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) => true };
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
                        }

                        if (configuration.Authentication.Keycloak.Configuration?.Authority?.StartsWith("http", StringComparison.InvariantCultureIgnoreCase) == true)
                        {
                            o.Authority = configuration.Authentication.Keycloak.Configuration.Authority;
                        }
                        else
                        {
                            o.Authority = configuration.Authentication.Keycloak.BaseUrl + configuration.Authentication.Keycloak.Configuration.Authority;
                        }

                        o.RequireHttpsMetadata = configuration.Authentication.Keycloak.Configuration.RequireHttpsMetadata;

                        string certFileName = configuration.Authentication.Keycloak.Configuration.CertFileName;
                        if (!string.IsNullOrWhiteSpace(certFileName))
                        {
                            // Set MetadataAddress to a dummy value to prevent automatic metadata fetching
                            o.MetadataAddress = "about:blank";
                            o.RequireHttpsMetadata = false;

                            o.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidIssuer = configuration.Authentication.Keycloak.BaseUrl + configuration.Authentication.Keycloak.Configuration.Authority,
                                ValidAudience = configuration.Authentication.Keycloak.Configuration.ValidAudience,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKeys = RetrieveKeycloakKeySet(certFileName),
                            };
                        }
                        else
                        {
                            o.RequireHttpsMetadata = configuration.Authentication.Keycloak.Configuration.RequireHttpsMetadata;

                            o.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidAudience = configuration.Authentication.Keycloak.Configuration.ValidAudience,
                                ValidateIssuerSigningKey = true,
                            };
                        }
                    }

                    o.IncludeErrorDetails = true;

                    o.Events = jwtBearerEvents;
                });
            }
            else
            {
                authenticationBuilder.AddNegotiate(); // force user to be authenticated if no jwt
            }

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy => policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
                AddConfigurationPolicies(configuration, options);
            });

            services.AddSingleton<IAuthorizationPolicyProvider, BiaAuthorizationPolicyProvider>();
        }

        private static void AddConfigurationPolicies(BiaNetSection configuration, AuthorizationOptions options)
        {
            if (configuration?.Policies != null)
            {
                foreach (Policy policy in configuration.Policies.Where(p => !string.IsNullOrWhiteSpace(p.Name) && p.RequireClaims?.Any() == true))
                {
                    options.AddPolicy(policy.Name, policyBuilder =>
                    {
                        policyBuilder.RequireAssertion(context =>
                        {
                            return policy.RequireClaims.Any(requireClaim =>
                                context.User.HasClaim(claim => claim.Type == requireClaim.Type && requireClaim.AllowedValues.Contains(claim.Value)));
                        });
                    });
                }
            }

            if (configuration?.Policies?.Any(confPolicy => confPolicy.Name == BiaConstants.Policy.ServiceApiRW) != true)
            {
                options.AddPolicy(BiaConstants.Policy.ServiceApiRW, policy => policy.RequireAssertion(_ => true));
            }
        }

        /// <summary>
        /// Retrieves the keycloak key set.
        /// </summary>
        /// <param name="certFileName">Name of the cert file.</param>
        /// <returns>The keycloak key set.</returns>
        private static IEnumerable<SecurityKey> RetrieveKeycloakKeySet(string certFileName)
        {
            // download key from the Keycloak wih this url:
            // The URL might look like: https://mykeycloak.mycompany/realms/BIA-Realm/protocol/openid-connect/certs
            JsonWebKeySet keySet = default;
            using (StreamReader streamReader = new StreamReader(certFileName))
            {
                string json = streamReader.ReadToEnd();
                keySet = new JsonWebKeySet(json);
            }

            return keySet.Keys;
        }
    }
}