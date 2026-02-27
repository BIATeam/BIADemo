// <copyright file="IocContainer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    // Begin BIADemo
    using BIA.Net.Core.Ioc;

    // End BIADemo
    using BIA.Net.Core.Ioc.Param;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc.Bia.Param;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories;

    // End BIADemo

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static partial class IocContainer
    {
        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static void ConfigureContainer(ParamIocContainer param)
        {
            BiaConfigureContainer(param);
        }

        private static ParamAutoRegister GetGlobalParamAutoRegister(ParamIocContainer param)
        {
            return new ParamAutoRegister()
            {
                Collection = param.Collection,
                ExcludedServiceNames = null,
                IncludedServiceNames = null,
            };
        }

        private static void ConfigureApplicationContainer(ParamIocContainer param)
        {
            BiaConfigureApplicationContainer(param);
            BiaConfigureApplicationContainerAutoRegister(GetGlobalParamAutoRegister(param));
        }

        private static void ConfigureDomainContainer(ParamIocContainer param)
        {
            BiaConfigureDomainContainer(param);
            BiaConfigureDomainContainerAutoRegister(GetGlobalParamAutoRegister(param));
        }

        private static void ConfigureCommonContainer(ParamIocContainer param)
        {
            // Common Layer
        }

#if BIA_USE_DATABASE
        private static void ConfigureInfrastructureDataContainer(ParamIocContainer param)
        {
            BiaConfigureInfrastructureDataContainer(param);
            BiaConfigureInfrastructureDataContainerAutoRegister(GetGlobalParamAutoRegister(param));
            BiaConfigureInfrastructureDataContainerDbContext(param);
        }
#endif

        private static void ConfigureInfrastructureServiceContainer(ParamIocContainer param)
        {
            BiaConfigureInfrastructureServiceContainer(param);

            // Begin BIADemo
            param.Collection.AddSingleton<Infrastructure.Service.Repositories.DocumentAnalysis.PdfAnalysisRepository>();
            param.Collection.AddSingleton<IDocumentAnalysisRepositoryFactory, Infrastructure.Service.Repositories.DocumentAnalysis.DocumentAnalysisRepositoryFactory>();

            param.Collection.AddHttpClient<IRemoteBiaApiRwRepository, RemoteBiaApiRwRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection)).AddStandardResilienceHandler();
            param.Collection.AddHttpClient<IRemotePlaneRepository, RemotePlaneRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection, false)).AddStandardResilienceHandler();
            param.Collection.AddHttpClient<IBiaDemoRoleApiRepository, BiaDemoRoleApiRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection));

            // End BIADemo
        }
    }
}