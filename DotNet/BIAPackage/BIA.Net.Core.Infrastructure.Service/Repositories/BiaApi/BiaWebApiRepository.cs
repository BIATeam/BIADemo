// <copyright file="BiaWebApiRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;
    using BIA.Net.Core.Common.Configuration.Keycloak;
    using BIA.Net.Core.Infrastructure.Service.Dto.Keycloak;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// BiaApiAuth Repository.
    /// </summary>
    public class BiaWebApiRepository : WebApiRepository
    {
        private readonly BiaWebApi biaWebApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaWebApiRepository" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="biaWebApi">The bia web API configuration.</param>
        public BiaWebApiRepository(
            HttpClient httpClient,
            ILogger<BiaWebApiRepository> logger,
            IBiaDistributedCache distributedCache,
            BiaWebApi biaWebApi)
             : base(httpClient, logger, distributedCache)
        {
            this.biaWebApi = biaWebApi;
        }

        /// <summary>
        /// Gets the base address.
        /// </summary>
        protected string BaseAddress => this.biaWebApi?.BaseAddress;

        /// <summary>
        /// Gets the bia web API.
        /// </summary>
        protected BiaWebApi BiaWebApi => this.biaWebApi;

        /// <summary>
        /// Gets or sets the keycloak setting.
        /// </summary>
        protected Keycloak KeycloakSetting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [call application setting].
        /// </summary>
        protected bool CallAppSetting { get; set; } = true;

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The application settings.</returns>
        protected virtual async Task<Keycloak> GetKeycloakSettingsAsync(string url = "/api/AppSettings")
        {
            var result = await this.GetAsync<object>($"{this.biaWebApi.BaseAddress}{url}");
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                var responseObject = JsonConvert.DeserializeAnonymousType(result.Result.ToString(), new { keycloak = new Keycloak() });
                return responseObject?.keycloak;
            }

            return null;
        }

        /// <summary>
        /// Gets the prefix cache key.
        /// </summary>
        /// <returns>The prefix cache key.</returns>
        protected virtual string GetPrefixCacheKey()
        {
            return $"{this.biaWebApi.BaseAddress}|{nameof(BiaWebApiRepository)}|";
        }

        /// <summary>
        /// Gets the keycloak setting cache key.
        /// </summary>
        /// <returns>The keycloak setting cache key.</returns>
        protected virtual string GetKeycloakSettingCacheKey()
        {
            return $"{this.GetPrefixCacheKey()}KeycloakSetting";
        }

        /// <summary>
        /// Gets the keycloak setting.
        /// </summary>
        /// <returns>The keycloak setting.</returns>
        protected virtual async Task<Keycloak> GetKeycloakSettingAsync()
        {
            string cacheKey = this.GetKeycloakSettingCacheKey();
            Keycloak setting = await this.DistributedCache.Get<Keycloak>(cacheKey);

            if (setting == null)
            {
                setting = await this.GetKeycloakSettingsAsync();

                if (setting != null)
                {
                    await this.DistributedCache.Add(cacheKey, setting, 60);
                }
            }

            return setting;
        }

        /// <summary>
        /// Sets the authentication configuration.
        /// </summary>
        protected virtual void SetAuthenticationConfiguration()
        {
            if (this.KeycloakSetting?.IsActive == true)
            {
                this.AuthenticationConfiguration = new AuthenticationConfiguration() { Mode = AuthenticationMode.Token };
            }
            else
            {
                this.AuthenticationConfiguration = new AuthenticationConfiguration { Mode = AuthenticationMode.Anonymous };
            }
        }

        /// <inheritdoc />
        protected override string GetBearerCacheKey()
        {
            return $"{this.GetPrefixCacheKey()}{Bearer}";
        }

        /// <inheritdoc />
        protected override async Task ConfigureHttpClientAsync()
        {
            if (this.CallAppSetting)
            {
                this.CallAppSetting = false;
                this.KeycloakSetting = await this.GetKeycloakSettingAsync();
                this.SetAuthenticationConfiguration();
            }

            await base.ConfigureHttpClientAsync();
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = await BiaKeycloakHelper.GetBearerTokenAsync(this.KeycloakSetting, this.PostAsync<TokenResponseDto, TokenRequestDto>, this.biaWebApi.CredentialSource);
            string cacheKey = this.GetKeycloakSettingCacheKey();
            await this.DistributedCache.Remove(cacheKey);
            return token;
        }
    }
}
