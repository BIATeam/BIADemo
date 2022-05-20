// <copyright file="IdentityProviderRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace Safran.EZwins.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Meziantou.Framework.Win32;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="Safran.EZwins.Domain.RepoContract.IWorkInstructionRepository" />
    public class IdentityProviderRepository : WebApiRepository, IIdentityProviderRepository
    {
        /// <summary>
        /// The Bearer.
        /// </summary>
        protected const string Bearer = "Bearer";

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
        public IdentityProviderRepository(HttpClient httpClient, IOptions<BiaNetSection> configuration, ILogger<IdentityProviderRepository> logger)
            : base(httpClient, logger)
        {
            this.configuration = configuration.Value;
        }

        /// <inheritdoc cref="IIdentityProviderRepository.SearchAsync"/>
        public virtual async Task<List<UserFromDirectory>> SearchAsync(string search, int returnSize)
        {
            if (string.IsNullOrWhiteSpace(this.configuration.Authentication.Keycloak.BaseUrl) || string.IsNullOrWhiteSpace(search))
            {
                return null;
            }

            await this.FillTokenAsync();

            string searchUrl = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.SearchUserUrl}?first=0&max={returnSize}&search={search}";

            List<SearchUserResponseDto> searchUserResponseDtos = (await this.GetAsync<List<SearchUserResponseDto>>(searchUrl)).Result;
            List<UserFromDirectory> userFromDirectories = this.ConvertToUserDirectory(searchUserResponseDtos);

            return userFromDirectories;
        }

        /// <summary>
        /// Fill the token.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task FillTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(this.configuration.Authentication.Keycloak.BaseUrl) && this.HttpClient.DefaultRequestHeaders.Authorization?.Scheme != Bearer)
            {
                string url = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.TokenConf.Url}";

                Credential cred = CredentialManager.ReadCredential(applicationName: this.configuration.Authentication.Keycloak.Api.TokenConf.CredentialKeyInWindowsVault);

                TokenRequestDto tokenRequestDto = new TokenRequestDto()
                {
                    ClientId = this.configuration.Authentication.Keycloak.Api.TokenConf.ClientId,
                    GrantType = this.configuration.Authentication.Keycloak.Api.TokenConf.GrantType,
                    Username = cred?.UserName,
                    Password = cred?.Password,
                };

                TokenResponseDto tokenResponseDto = (await this.PostAsync<TokenResponseDto, TokenRequestDto>(url, tokenRequestDto, true)).Result;

                if (!string.IsNullOrWhiteSpace(tokenResponseDto?.AccessToken))
                {
                    this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, tokenResponseDto?.AccessToken);
                }
            }
        }

        /// <summary>
        /// Converts to user directory.
        /// </summary>
        /// <param name="searchUserResponseDtos">The search user response dtos.</param>
        /// <returns>List of <see cref="UserFromDirectory"/>.</returns>
        private List<UserFromDirectory> ConvertToUserDirectory(List<SearchUserResponseDto> searchUserResponseDtos)
        {
            List<UserFromDirectory> userFromDirectories = new List<UserFromDirectory>();

            if (searchUserResponseDtos?.Any() == true)
            {
                foreach (SearchUserResponseDto searchUserResponseDto in searchUserResponseDtos)
                {
                    if (!string.IsNullOrWhiteSpace(searchUserResponseDto.Attribute.ObjectSid?.FirstOrDefault()))
                    {
                        string sid = new System.Security.Principal.SecurityIdentifier(System.Convert.FromBase64String(searchUserResponseDto.Attribute.ObjectSid?.FirstOrDefault()), 0).ToString();

                        UserFromDirectory userFromDirectory = new UserFromDirectory
                        {
                            FirstName = searchUserResponseDto.FirstName,
                            LastName = searchUserResponseDto.LastName,
                            Login = searchUserResponseDto.Username,
                            Domain = searchUserResponseDto.Attribute.LdapEntryDn?.FirstOrDefault()?.Split(',')?.FirstOrDefault(x => x.StartsWith("DC="))?.Split('=')?.LastOrDefault()?.ToUpper(),
                            Sid = sid,
                            Guid = new Guid(searchUserResponseDto.Attribute.LdapId?.FirstOrDefault()),
                            Country = searchUserResponseDto.Attribute.Country?.FirstOrDefault(),
                            Company = searchUserResponseDto.Attribute.Company?.FirstOrDefault(),
                            Department = searchUserResponseDto.Attribute.Department?.FirstOrDefault(),
                            DistinguishedName = searchUserResponseDto.Attribute.LdapEntryDn?.FirstOrDefault(),
                            Email = searchUserResponseDto.Email,
                            IsEmployee = true,
                            Manager = searchUserResponseDto.Attribute.Manager?.FirstOrDefault(),
                            Office = searchUserResponseDto.Attribute.PhysicalDeliveryOfficeName?.FirstOrDefault(),
                        };

                        userFromDirectory.Site = userFromDirectory.Domain == "CORP" ? userFromDirectory.Office : searchUserResponseDto.Attribute.Description?.FirstOrDefault();

                        // Set external company
                        string jobTitle = searchUserResponseDto.Attribute.Title?.FirstOrDefault();

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

                        userFromDirectories.Add(userFromDirectory);
                    }
                }
            }

            return userFromDirectories ?? new List<UserFromDirectory>();
        }
    }
}
