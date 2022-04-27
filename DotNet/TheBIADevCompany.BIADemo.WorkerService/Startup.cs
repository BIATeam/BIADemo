// <copyright file="Startup.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using BIA.Net.Core.WorkerService.Features;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using BIA.Net.Core.WorkerService.Features.HangfireServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.CookiePolicy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
    using TheBIADevCompany.BIADemo.WorkerService.Features;

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
        /// The current environment.
        /// </summary>
        private readonly IWebHostEnvironment currentEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="env">The environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.currentEnvironment = env;
            this.configuration = configuration;
            this.biaNetSection = new BiaNetSection();
            this.configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddResponseCompression();

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
            });

            // Used to get a unique identifier for each HTTP request and track it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => new BIAClaimsPrincipal() { });
            services.AddTransient<UserContext>(provider => new UserContext("en-GB"));

            services.Configure<ClientForHubConfiguration>(
                this.configuration.GetSection("BiaNet:WorkerFeatures:ClientForHub"));

            services.AddBiaWorkerFeatures(
                this.biaNetSection.WorkerFeatures,
                this.configuration,
                new List<DatabaseHandlerRepository>()
                    {
                        // Add here all the Handler repository.
                        // Begin BIADemo
                        new PlaneHandlerRepository(this.configuration),

                        // End BIADemo
                    });

            services.AddHostedService<Worker>();

            // Configure IoC for classes not in the API project.
            IocContainer.ConfigureContainer(services, this.configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment.</param>
        /// <param name="jwtFactory">The jwt Factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IJwtFactory jwtFactory)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Begin BIADemo
            PlaneHandlerRepository.Configure(app.ApplicationServices.GetService<IClientForHubRepository>());

            // End BIADemo
            HangfireServerAuthorizations hangfireServerAuthorizations = new ();
            hangfireServerAuthorizations.Authorization = new[] { new HangfireAuthorizationFilter(false, "Background_Task_Admin", this.biaNetSection.Jwt.SecretKey, jwtFactory) };
            hangfireServerAuthorizations.AuthorizationReadOnly = new[] { new HangfireAuthorizationFilter(true, "Background_Task_Read_Only", this.biaNetSection.Jwt.SecretKey, jwtFactory) };

            app.UseBiaWorkerFeatures<AuditFeature>(this.biaNetSection.WorkerFeatures, hangfireServerAuthorizations);
        }
    }
}