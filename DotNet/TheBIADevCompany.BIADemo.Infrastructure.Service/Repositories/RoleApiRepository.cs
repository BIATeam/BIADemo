// <copyright file="RoleApiRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Infrastructure.Service.Repositories.BiaApi;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Api.RolesForApp;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Poppler Service Remote Repository.
    /// </summary>
    public class RoleApiRepository : BiaWebApiRepository, IRoleApiRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleApiRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public RoleApiRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<RoleApiRepository> logger,
            IBiaDistributedCache distributedCache)
            : base(
                httpClient,
                logger,
                distributedCache,
                configuration.GetSection("RoleApi").Get<RoleApi>())
        {
            this.RoleApi = configuration.GetSection("RoleApi").Get<RoleApi>();
        }

        /// <summary>
        /// Gets the bia web API.
        /// </summary>
        protected RoleApi RoleApi { get; set; }

        /// <inheritdoc />
        public async Task<ApiRolesForApp> GetRolesFromApi(string appName, string userLogin)
        {
            string url = this.RoleApi.EndpointUrl;
            var result = await this.GetAsync<ApiRolesForApp>(
                $"{this.BiaWebApi.BaseAddress}{url}?appName={appName}&userLogin={userLogin}");
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                return result.Result;
            }
            else
            {
                this.Logger.LogError(
                    "[RoleApiRepository.GetRolesFromApi] Error getting the roles for the user {UserLogin} for application {AppName}. Error : {ReasonPhrase}",
                    userLogin,
                    appName,
                    result.ReasonPhrase);
                return default;
            }
        }
    }
}
