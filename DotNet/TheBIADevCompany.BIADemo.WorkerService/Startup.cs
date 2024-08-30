// <copyright file="Startup.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Claims;
    using System.Security.Principal;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Presentation.Common.Features;
    using BIA.Net.Core.WorkerService.Features;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Migrations;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.WorkerService.Features;

    // End BIADemo
#endif

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
        /// <param name="env">The environment.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.biaNetSection = new BiaNetSection();
            this.configuration.GetSection("BiaNet").Bind(this.biaNetSection);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="host">The host.</param>
        public static void Configure(IHost host)
        {
#if BIA_FRONT_FEATURE
            // Begin BIADemo
            PlaneHandlerRepository.Configure(host.Services.GetService<IClientForHubRepository>());

            // End BIADemo
            CommonFeaturesExtensions.UseBiaCommonFeatures<AuditFeature>(host.Services);
#endif
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Used to get a unique identifier for each HTTP request and track it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IPrincipal>(
                provider =>
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, Environment.UserName) };
                    var userIdentity = new ClaimsIdentity(claims, "NonEmptyAuthType");
                    return new BiaClaimsPrincipal(new ClaimsPrincipal(userIdentity));
                });
            services.AddTransient<UserContext>(provider => new UserContext("en-GB", this.biaNetSection.Cultures));

            // Begin BIA Standard service
            services.AddBiaCommonFeatures(this.biaNetSection.CommonFeatures, this.configuration);
            services.AddBiaWorkerFeatures(
                this.biaNetSection.WorkerFeatures,
                this.configuration);
            services.AddSingleton<IDatabaseHandlerRepository, PlaneHandlerRepository>();
            // End BIA Standard service
#if BIA_FRONT_FEATURE
            // Begin BIADemo
            services.AddHostedService<Worker>();
            // End BIADemo
#endif

            // Configure IoC for classes not in the API project.
            IocContainer.ConfigureContainer(services, this.configuration, false);
        }
    }
}