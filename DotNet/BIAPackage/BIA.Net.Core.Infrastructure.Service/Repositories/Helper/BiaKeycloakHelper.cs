namespace BIA.Net.Core.Infrastructure.Service.Repositories.Helper
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.Keycloak;
    using BIA.Net.Core.Infrastructure.Service.Dto.Keycloak;

    /// <summary>
    /// Bia Keycloak Helper.
    /// </summary>
    public static class BiaKeycloakHelper
    {
        /// <summary>
        /// Gets the bearer token.
        /// </summary>
        /// <param name="keycloak">The keycloak.</param>
        /// <param name="postAsync">The post asynchronous.</param>
        /// <returns>The bearer token.</returns>
        public static async Task<string> GetBearerTokenAsync(
           Keycloak keycloak,
           Func<string, TokenRequestDto, bool, Task<(TokenResponseDto Result, bool IsSuccessStatusCode, string ReasonPhrase)>> postAsync)
        {
            string token = null;

            if (keycloak.IsActive && !string.IsNullOrWhiteSpace(keycloak.BaseUrl))
            {
                string url = $"{keycloak.BaseUrl}{keycloak.Api.TokenConf.RelativeUrl}";

                TokenRequestDto tokenRequestDto = new TokenRequestDto()
                {
                    ClientId = keycloak.Api.TokenConf.ClientId,
                    GrantType = keycloak.Api.TokenConf.GrantType,
                };

                (string Login, string Password) credential = CredentialRepository.RetrieveCredentials(keycloak.Api.TokenConf.CredentialSource);

                tokenRequestDto.Username = credential.Login;
                tokenRequestDto.Password = credential.Password;

                TokenResponseDto tokenResponseDto = (await postAsync(url, tokenRequestDto, true)).Result;

                token = tokenResponseDto?.AccessToken;
            }

            return token;
        }
    }
}
