// <copyright file="BiaWebApiAuthRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak;

    /// <summary>
    /// BiaWebApiAuth Repository.
    /// </summary>
    public class BiaWebApiAuthRepository : WebApiRepository, IBiaWebApiAuthRepository
    {
        private readonly IBiaWebApiAppSettingsRepository biaWebApiAppSettingsRepository;

        private AppSettingsDto appSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaWebApiAuthRepository" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="biaWebApiAppSettingsRepository">The bia web API application settings repository.</param>
        public BiaWebApiAuthRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<BiaWebApiAuthRepository> logger,
            IBiaDistributedCache distributedCache,
            IIdentityProviderRepository identityProviderRepository,
            IBiaWebApiAppSettingsRepository biaWebApiAppSettingsRepository)
             : base(httpClient, logger, distributedCache, new BIA.Net.Core.Common.Configuration.AuthenticationSection.AuthenticationConfiguration() { Mode = BIA.Net.Core.Common.Configuration.AuthenticationSection.AuthenticationMode.Token })
        {
            this.biaWebApiAppSettingsRepository = biaWebApiAppSettingsRepository;
        }

        /// <inheritdoc cref="IBiaWebApiAuthRepository.GetAsync"/>
        public async Task<string> LoginAsync(string baseAddress, string urlLogin = "/api/Auth/token")
        {
            this.appSetting = await this.biaWebApiAppSettingsRepository.GetAsync(baseAddress);

            var result = await this.GetAsync<string>($"{baseAddress}{urlLogin}");
            return result.IsSuccessStatusCode ? result.Result : null;
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = null;

            if (this.appSetting.Keycloak.IsActive && !string.IsNullOrWhiteSpace(this.appSetting.Keycloak.BaseUrl))
            {
                string url = $"{this.appSetting.Keycloak.BaseUrl}{this.appSetting.Keycloak.Api.TokenConf.RelativeUrl}";

                TokenRequestDto tokenRequestDto = new TokenRequestDto()
                {
                    ClientId = this.appSetting.Keycloak.Api.TokenConf.ClientId,
                    GrantType = this.appSetting.Keycloak.Api.TokenConf.GrantType,
                };

                (string Login, string Password) credential = CredentialRepository.RetrieveCredentials(this.appSetting.Keycloak.Api.TokenConf.CredentialSource);

                tokenRequestDto.Username = credential.Login;
                tokenRequestDto.Password = credential.Password;

                TokenResponseDto tokenResponseDto = (await this.PostAsync<TokenResponseDto, TokenRequestDto>(url: url, body: tokenRequestDto, isFormUrlEncoded: true)).Result;

                token = tokenResponseDto?.AccessToken;
            }

            return token;
        }
    }
}
