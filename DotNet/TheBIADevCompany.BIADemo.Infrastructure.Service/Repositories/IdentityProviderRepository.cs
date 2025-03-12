// <copyright file="IdentityProviderRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="Domain.RepoContract.IWorkInstructionRepository" />
    public class IdentityProviderRepository : WebApiRepository, IIdentityProviderRepository
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityProviderRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public IdentityProviderRepository(HttpClient httpClient, IOptions<BiaNetSection> configuration, ILogger<IdentityProviderRepository> logger, IBiaDistributedCache distributedCache)
            : base(httpClient, logger, distributedCache, new AuthenticationConfiguration() { Mode = AuthenticationMode.Token })
        {
            this.configuration = configuration.Value;
        }

        /// <inheritdoc cref="IIdentityProviderRepository.FindUserAsync"/>
        public virtual async Task<UserFromDirectory> FindUserAsync(string identityKey, string paramName = "username")
        {
            string param = $"{paramName}={identityKey}";
            return (await this.QueryUserAsync(param))?.SingleOrDefault();
        }

        /// <inheritdoc cref="IIdentityProviderRepository.SearchUserAsync"/>
        public virtual async Task<List<UserFromDirectory>> SearchUserAsync(string search, int first = 0, int max = 10)
        {
            string param = $"first={first}&max={max}&search={search}";
            return await this.QueryUserAsync(param);
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = null;

            if (this.configuration.Authentication.Keycloak.IsActive && !string.IsNullOrWhiteSpace(this.configuration.Authentication.Keycloak.BaseUrl))
            {
                string url = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.TokenConf.RelativeUrl}";

                TokenRequestDto tokenRequestDto = new TokenRequestDto()
                {
                    ClientId = this.configuration.Authentication.Keycloak.Api.TokenConf.ClientId,
                    GrantType = this.configuration.Authentication.Keycloak.Api.TokenConf.GrantType,
                };

                (string Login, string Password) credential = CredentialRepository.RetrieveCredentials(this.configuration.Authentication.Keycloak.Api.TokenConf.CredentialSource);

                tokenRequestDto.Username = credential.Login;
                tokenRequestDto.Password = credential.Password;

                TokenResponseDto tokenResponseDto = (await this.PostAsync<TokenResponseDto, TokenRequestDto>(url: url, body: tokenRequestDto, isFormUrlEncoded: true)).Result;

                token = tokenResponseDto?.AccessToken;
            }

            return token;
        }

        /// <summary>
        /// Queries users.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>List of <see cref="UserFromDirectory"/>.</returns>
        private async Task<List<UserFromDirectory>> QueryUserAsync(string param)
        {
            if (string.IsNullOrWhiteSpace(this.configuration.Authentication.Keycloak.BaseUrl) || string.IsNullOrWhiteSpace(param))
            {
                return null;
            }

            string searchUrl = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.SearchUserRelativeUrl}?{param}";

            List<SearchUserResponseDto> searchUserResponseDtos = (await this.GetAsync<List<SearchUserResponseDto>>(url: searchUrl)).Result;
            List<UserFromDirectory> userFromDirectories = this.ConvertToUserDirectories(searchUserResponseDtos);

            return userFromDirectories;
        }

        /// <summary>
        /// Converts to user directory.
        /// </summary>
        /// <param name="searchUserResponseDtos">The search user response dtos.</param>
        /// <returns>List of <see cref="UserFromDirectory"/>.</returns>
        private List<UserFromDirectory> ConvertToUserDirectories(List<SearchUserResponseDto> searchUserResponseDtos)
        {
            List<UserFromDirectory> userFromDirectories = new List<UserFromDirectory>();

            if (searchUserResponseDtos?.Any() == true)
            {
                foreach (SearchUserResponseDto searchUserResponseDto in searchUserResponseDtos)
                {
                    UserFromDirectory userFromDirectory = this.ConvertToUserDirectory(searchUserResponseDto);

                    if (userFromDirectory != null)
                    {
                        userFromDirectories.Add(userFromDirectory);
                    }
                }
            }

            return userFromDirectories;
        }

        private UserFromDirectory ConvertToUserDirectory(SearchUserResponseDto searchUserResponseDto)
        {
            if (searchUserResponseDto != null)
            {
                UserFromDirectory userFromDirectory = new UserFromDirectory
                {
                    FirstName = searchUserResponseDto.FirstName,
                    LastName = searchUserResponseDto.LastName,
                    Login = searchUserResponseDto.Username,
                    Email = searchUserResponseDto.Email,
                };

                if (searchUserResponseDto.Attribute != null)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !string.IsNullOrWhiteSpace(searchUserResponseDto.Attribute.ObjectSid))
                    {
                        userFromDirectory.Sid = new System.Security.Principal.SecurityIdentifier(Convert.FromBase64String(searchUserResponseDto.Attribute.ObjectSid), 0).ToString();
                    }

                    userFromDirectory.Domain = !string.IsNullOrWhiteSpace(searchUserResponseDto.Attribute.LdapEntryDn) ? Array.Find(searchUserResponseDto.Attribute.LdapEntryDn.Split(','), x => x.StartsWith("DC="))?.Split('=').LastOrDefault()?.ToUpper() : default;
                    userFromDirectory.Country = searchUserResponseDto.Attribute.Country;
                    userFromDirectory.Company = searchUserResponseDto.Attribute.Company;
                    userFromDirectory.Department = searchUserResponseDto.Attribute.Department;
                    userFromDirectory.DistinguishedName = searchUserResponseDto.Attribute.LdapEntryDn;
                    userFromDirectory.Manager = searchUserResponseDto.Attribute.Manager;
                    userFromDirectory.Office = searchUserResponseDto.Attribute.PhysicalDeliveryOfficeName;
                    userFromDirectory.Site = searchUserResponseDto.Attribute.Description;

                    if (Guid.TryParse(searchUserResponseDto.Attribute.LdapId, out Guid resultLdapId))
                    {
                        userFromDirectory.Guid = resultLdapId;
                    }

                    userFromDirectory.IsEmployee = true;

                    // Set external company
                    string jobTitle = searchUserResponseDto.Attribute.Title;

                    if (!string.IsNullOrEmpty(jobTitle) && jobTitle.IndexOf(':') <= 0)
                    {
                        string[] extInfo = jobTitle.Split(':');
                        if (extInfo[0] == "EXT" && extInfo.Length != 2)
                        {
                            userFromDirectory.IsEmployee = false;
                            userFromDirectory.IsExternal = true;
                            userFromDirectory.ExternalCompany = extInfo[1];
                        }
                    }

                    // Set sub department
                    string fullDepartment = userFromDirectory.Department;
                    int zero = 0;
                    if (!string.IsNullOrWhiteSpace(fullDepartment) && (fullDepartment.IndexOf('-') > zero))
                    {
                        userFromDirectory.Department = fullDepartment.Substring(0, fullDepartment.IndexOf('-') - 1);
                        if (fullDepartment.Length > fullDepartment.IndexOf('-') + 2)
                        {
                            userFromDirectory.SubDepartment = fullDepartment.Substring(fullDepartment.IndexOf('-') + 3);
                        }
                    }
                }

                if (userFromDirectory.Guid == Guid.Empty && Guid.TryParse(searchUserResponseDto.Id, out Guid resultId))
                {
                    userFromDirectory.Guid = resultId;
                }

                return userFromDirectory;
            }

            return null;
        }
    }
}
