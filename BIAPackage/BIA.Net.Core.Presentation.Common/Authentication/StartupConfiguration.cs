// <copyright file="StartupConfiguration.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Common.Authentication
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class StartupConfiguration
    {
        /// <summary>
        /// The JWT bearer default.
        /// </summary>
        public const string JwtBearerDefault = "Default";

        /// <summary>
        /// The JWT bearer keycloak
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
            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = configuration.Jwt.Issuer;
                options.Audience = configuration.Security?.Audience;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
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

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerIdentityProvider, o =>
                {
                    o.Authority = configuration.Keycloak.BaseUrl + configuration.Keycloak.Configuration.Authority;
                    o.Audience = configuration.Keycloak.Configuration.Audience;
                    o.RequireHttpsMetadata = configuration.Keycloak.Configuration.RequireHttpsMetadata;
#if DEBUG
                    o.IncludeErrorDetails = true;
#endif
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = configuration.Keycloak.Configuration.ValidAudience
                    };
                })
            //.AddNegotiate() // force user to be authenticated if no jwt 
            .AddJwtBearer(JwtBearerDefault, configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration.Jwt.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.OnStarting(async () =>
                        {
                            context.NoResult();
                            context.Response.Headers.Add("Token-Expired-Or-Invalid", "true");
                            context.Response.ContentType = "text/plain";
                            context.Response.StatusCode = 498; // 498 = Token expired/invalid
                            await context.Response.WriteAsync("Un-Authorized");
                        });

                        return Task.CompletedTask;
                    },
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy => policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
            });
        }
    }
}