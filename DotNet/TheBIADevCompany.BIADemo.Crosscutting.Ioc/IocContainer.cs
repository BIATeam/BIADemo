// <copyright file="IocContainer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using BIA.Net.Core.Ioc;
    using BIA.Net.Core.Ioc.Param;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc.Bia.Param;

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
            BiaIocContainer.ConfigureDomainContainer(param);
            BiaConfigureDomainContainer(param);
            BiaConfigureDomainContainerAutoRegister(GetGlobalParamAutoRegister(param));
        }

        private static void ConfigureCommonContainer(ParamIocContainer param)
        {
            BiaIocContainer.ConfigureCommonContainer(param);
        }

#if BIA_USE_DATABASE
        private static void ConfigureInfrastructureDataContainer(ParamIocContainer param)
        {
            BiaIocContainer.ConfigureInfrastructureDataContainer(param);
            BiaConfigureInfrastructureDataContainer(param);
            BiaConfigureInfrastructureDataContainerAutoRegister(GetGlobalParamAutoRegister(param));
            BiaConfigureInfrastructureDataContainerDbContext(param);
        }
#endif

        private static void ConfigureInfrastructureServiceContainer(ParamIocContainer param)
        {
            BiaIocContainer.ConfigureInfrastructureServiceContainer(param);
            BiaConfigureInfrastructureServiceContainer(param);

#if BIA_FRONT_FEATURE
            // Begin BIADemo
            param.Collection.AddSingleton<Infrastructure.Service.Repositories.DocumentAnalysis.PdfAnalysisRepository>();
            param.Collection.AddSingleton<Domain.RepoContract.DocumentAnalysis.IDocumentAnalysisRepositoryFactory, Infrastructure.Service.Repositories.DocumentAnalysis.DocumentAnalysisRepositoryFactory>();

            param.Collection.AddHttpClient<Domain.RepoContract.IRemoteBiaApiRwRepository, Infrastructure.Service.Repositories.RemoteBiaApiRwRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection)).AddStandardResilienceHandler();
            param.Collection.AddHttpClient<Domain.RepoContract.IRemotePlaneRepository, Infrastructure.Service.Repositories.RemotePlaneRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection, false)).AddStandardResilienceHandler();
            param.Collection.AddHttpClient<Domain.RepoContract.IBiaDemoRoleApiRepository, Infrastructure.Service.Repositories.BiaDemoRoleApiRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection));

            // End BIADemo
#endif
        }
    }
}