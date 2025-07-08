// <copyright file="Startup.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Application.Clean;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.Keycloak;
    using BIA.Net.Core.Presentation.Common.Features;
    using BIA.Net.Core.WorkerService.Features;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc;
#if BIA_FRONT_FEATURE
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
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IHost host, IConfiguration configuration)
        {
            BiaNetSection biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(biaNetSection);
            if (biaNetSection.CommonFeatures?.AuditConfiguration?.IsActive == true)
            {
                host.Services.GetRequiredService<IAuditFeatureService>().EnableAuditFeatures();
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Begin BIA Standard service
            services.AddBiaCommonFeatures(this.biaNetSection.CommonFeatures, this.configuration);
            services.AddBiaWorkerFeatures(
                this.biaNetSection.WorkerFeatures,
                this.configuration);

            // End BIA Standard service
#if BIA_FRONT_FEATURE
            // Begin BIADemo
            services.AddHostedService<Worker>();
            services.AddSingleton<IDatabaseHandlerRepository, PlaneHandlerRepository>();
            services.AddSingleton<IDatabaseHandlerRepository, AirportHandlerRepository>();
            services.AddTransient<IArchiveService, Application.Fleet.PlaneArchiveService>();
            services.AddTransient<ICleanService, Application.Fleet.PlaneCleanService>();

            // End BIADemo
#endif

            // Configure IoC for classes not in the API project.
            IocContainer.ConfigureContainer(services, this.configuration, false);
        }
    }
}