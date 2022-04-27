// <copyright file="StartupConfiguration.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Common.Authentication
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authentication.Negotiate;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class StartupConfiguration
    {

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
            .AddNegotiate() // force user to be authenticated if no jwt 
            .AddJwtBearer(configureOptions =>
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
                            context.Response.StatusCode = 498;// (int)HttpStatusCode.PreconditionFailed;
                            await context.Response.WriteAsync("Un-Authorized");
                        });

                        return Task.CompletedTask;
                    },
                    //OnMessageReceived = mrCtx =>
                    //{
                    //    // Look for HangFire stuff
                    //    var path = mrCtx.Request.Path.HasValue ? mrCtx.Request.Path.Value : "";
                    //    var pathBase = mrCtx.Request.PathBase.HasValue ? mrCtx.Request.PathBase.Value : path;
                    //    var isFromHangFire = true;// path.StartsWith(WebsiteConstants.HANG_FIRE_URL) || pathBase.StartsWith(WebsiteConstants.HANG_FIRE_URL);

                    //    //If it's HangFire look for token.
                    //    if (isFromHangFire)
                    //    {
                    //        mrCtx.HttpContext.Response.Cookies
                    //            .Append("HangFireCookie666",
                    //                "Coucou",
                    //                new CookieOptions()
                    //                {
                    //                    Secure = true,
                    //                    Path = "/",
                    //                    SameSite = SameSiteMode.None,
                    //                    IsEssential = true,
                    //                    Expires = DateTime.Now.AddMinutes(10)
                    //                });
                    //        if (mrCtx.Request.Query.ContainsKey("jwt_token"))
                    //        {
                    //            //If we find token add it to the response cookies
                    //            //mrCtx.Token = mrCtx.Request.Query["jwt_token"];
                    //            var Token = mrCtx.Request.Query["jwt_token"];
                    //            mrCtx.HttpContext.Response.Cookies
                    //            .Append("HangFireCookie",
                    //                Token,
                    //                new CookieOptions()
                    //                {
                    //                    Secure = true,
                    //                    Path = "/",
                    //                    SameSite = SameSiteMode.None,
                    //                    IsEssential = true,
                    //                    Expires = DateTime.Now.AddMinutes(10)
                    //                });
                    //        }
                    //        else
                    //        {
                    //            //Check if we have a cookie from the previous request.
                    //            var cookies = mrCtx.Request.Cookies;
                    //            if (cookies.ContainsKey("HangFireCookie"))
                    //            {
                    //                // mrCtx.Token = cookies["HangFireCookie"];
                    //                var Token = cookies["HangFireCookie"];
                    //                bool test = true;
                    //            }
                                    
                    //        }//Else
                    //    }//If

                    //    return Task.CompletedTask;
                    //}
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