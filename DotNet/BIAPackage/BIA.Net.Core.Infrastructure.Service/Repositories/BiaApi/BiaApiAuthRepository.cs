// <copyright file="BiaApiAuthRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Dynamic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Common.Configuration.Keycloak;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Dto.Keycloak;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// BiaApiAuth Repository.
    /// </summary>
    public class BiaApiAuthRepository : WebApiRepository, IBiaApiAuthRepository
    {
        /// <summary>
        /// True if authentication Configuration is defined by the child class.
        /// </summary>
        private readonly bool hasAuthConfDefined = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaApiAuthRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="authenticationConfiguration">The authentication configuration.</param>
        public BiaApiAuthRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<BiaApiAuthRepository> logger,
            IBiaDistributedCache distributedCache,
            AuthenticationConfiguration authenticationConfiguration = null)
             : base(httpClient, logger, distributedCache, authenticationConfiguration)
        {
            this.hasAuthConfDefined = authenticationConfiguration != null;
        }

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        protected string BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the keycloak setting.
        /// </summary>
        protected Keycloak KeycloakSetting { get; set; }

        /// <inheritdoc />
        public virtual async Task<string> LoginAsync(string baseAddress, string url = "/api/Auth/login?lightToken=false")
        {
            await this.Init(baseAddress);

            var result = await this.GetAsync<object>($"{baseAddress}{url}");
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                dynamic responseObject = JsonConvert.DeserializeObject<ExpandoObject>(result.Result.ToString());
                return responseObject?.token;
            }

            return null;
        }

        /// <inheritdoc />
        public virtual async Task<string> GetTokenAsync(string baseAddress, string url = "/api/Auth/token")
        {
            await this.Init(baseAddress);

            var result = await this.GetAsync<string>($"{baseAddress}{url}");
            return result.IsSuccessStatusCode ? result.Result : null;
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The application settings.</returns>
        protected virtual async Task<Keycloak> GetKeycloakSettingsAsync(string baseAddress, string url = "/api/AppSettings")
        {
            var result = await this.GetAsync<object>($"{baseAddress}{url}");
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                var responseObject = JsonConvert.DeserializeAnonymousType(result.Result.ToString(), new { keycloak = new Keycloak() });
                return responseObject?.keycloak;
            }

            return null;
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task Init(string baseAddress)
        {
            this.BaseAddress = baseAddress;
            string cacheKey = this.GetKeycloakSettingCacheKey();

            this.KeycloakSetting = await this.DistributedCache.Get<Keycloak>(cacheKey);

            if (this.KeycloakSetting == null)
            {
                this.KeycloakSetting = await this.GetKeycloakSettingsAsync(baseAddress);

                if (this.KeycloakSetting != null)
                {
                    await this.DistributedCache.Add(cacheKey, this.KeycloakSetting, 5);
                }
            }

            if (!this.hasAuthConfDefined)
            {
                if (this.KeycloakSetting?.IsActive == true)
                {
                    this.AuthenticationConfiguration = new AuthenticationConfiguration() { Mode = AuthenticationMode.Token };
                }
                else
                {
                    this.AuthenticationConfiguration = null;
                }
            }
        }

        /// <summary>
        /// Gets the prefix cache key.
        /// </summary>
        /// <returns>The prefix cache key.</returns>
        protected virtual string GetPrefixCacheKey()
        {
            return $"{this.BaseAddress}|{nameof(BiaApiAuthRepository)}|";
        }

        /// <summary>
        /// Gets the keycloak setting cache key.
        /// </summary>
        /// <returns>The keycloak setting cache key.</returns>
        protected virtual string GetKeycloakSettingCacheKey()
        {
            return $"{this.GetPrefixCacheKey()}KeycloakSetting";
        }

        /// <inheritdoc />
        protected override string GetBearerCacheKey()
        {
            return $"{this.GetPrefixCacheKey()}{Bearer}";
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = await BiaKeycloakHelper.GetBearerTokenAsync(this.KeycloakSetting, this.PostAsync<TokenResponseDto, TokenRequestDto>);
            return token;
        }
    }
}
