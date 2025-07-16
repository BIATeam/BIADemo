// <copyright file="Startup.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api
{
    using System;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Presentation.Api.Features;
    using BIA.Net.Core.Presentation.Api.Features.HangfireDashboard;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using BIA.Net.Core.Presentation.Common.Features;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.CookiePolicy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc;

    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly BiaNetSection biaNetSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.biaNetSection = new BiaNetSection();
            this.configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="builder">The app's builder.</param>
        public void ConfigureServices(IHostApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddControllers();
            services.ConfigureOptions<BiaApiBehaviorOptions>();

            services.AddCors();
            services.AddResponseCompression();
            services.AddRequestDecompression();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true; // Enforce HSTS on all Sub-Domains as well
                options.MaxAge = TimeSpan.FromDays(365); // One year expiry
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            // Begin BIA Standard service
            services.AddBiaCommonFeatures(this.biaNetSection.CommonFeatures, this.configuration);
            services.AddBiaApiFeatures(this.biaNetSection.ApiFeatures, this.configuration);

            // End BIA Standard service

            // Configure IoC for classes not in the API project.
            IocContainer.ConfigureContainer(services, this.configuration, true);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment.</param>
        /// <param name="jwtFactory">The JWT factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IJwtFactory jwtFactory)
        {
            app.UsePathBase("/BIADemo/WebApi");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureApiExceptionHandler(env.IsDevelopment());

            if (!string.IsNullOrEmpty(this.biaNetSection.Security?.Audience))
            {
                // for Front Angular Dev Do not forget to modify the file launchSettings.json to
                // enable windows authentication on IISExpress ("windowsAuthentication": true,
                // "anonymousAuthentication": true,)
                app.UseCors(x => x
                    .WithOrigins(this.biaNetSection.Security.Audience.Split(","))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders(BiaConstants.HttpHeaders.TotalCount));
            }

            app.UseResponseCompression();
            app.UseRequestDecompression();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            HangfireDashboardAuthorizations hangfireDashboardAuthorizations = new ();
            hangfireDashboardAuthorizations.Authorization = new[] { new HangfireAuthorizationFilter(false, "Background_Task_Admin", this.biaNetSection.Jwt.SecretKey, jwtFactory) };
            hangfireDashboardAuthorizations.AuthorizationReadOnly = new[] { new HangfireAuthorizationFilter(true, "Background_Task_Read_Only", this.biaNetSection.Jwt.SecretKey, jwtFactory) };

            if (this.biaNetSection.CommonFeatures?.AuditConfiguration?.IsActive == true)
            {
                app.ApplicationServices.GetRequiredService<IAuditFeatureService>().EnableAuditFeatures();
            }

            app.UseBiaApiFeatures(this.biaNetSection.ApiFeatures, hangfireDashboardAuthorizations);
        }
    }
}