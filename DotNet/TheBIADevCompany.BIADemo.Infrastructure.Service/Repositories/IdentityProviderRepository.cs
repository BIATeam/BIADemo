// <copyright file="IdentityProviderRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Meziantou.Framework.Win32;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
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
        public IdentityProviderRepository(HttpClient httpClient, IOptions<BiaNetSection> configuration, ILogger<IdentityProviderRepository> logger, IDistributedCache distributedCache)
            : base(httpClient, logger, distributedCache)
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

            string searchUrl = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.SearchUserRelativeUrl}?first=0&max={returnSize}&search={search}";

            List<SearchUserResponseDto> searchUserResponseDtos = (await this.GetAsync<List<SearchUserResponseDto>>(url: searchUrl, useBearerToken: true)).Result;
            List<UserFromDirectory> userFromDirectories = this.ConvertToUserDirectory(searchUserResponseDtos);

            return userFromDirectories;
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = null;

            if (!string.IsNullOrWhiteSpace(this.configuration.Authentication.Keycloak.BaseUrl))
            {
                string url = $"{this.configuration.Authentication.Keycloak.BaseUrl}{this.configuration.Authentication.Keycloak.Api.TokenConf.RelativeUrl}";

#pragma warning disable CA1416 // Validate platform compatibility
                Credential cred = CredentialManager.ReadCredential(applicationName: this.configuration.Authentication.Keycloak.Api.TokenConf.CredentialKeyInWindowsVault);
#pragma warning restore CA1416 // Validate platform compatibility

                TokenRequestDto tokenRequestDto = new TokenRequestDto()
                {
                    ClientId = this.configuration.Authentication.Keycloak.Api.TokenConf.ClientId,
                    GrantType = this.configuration.Authentication.Keycloak.Api.TokenConf.GrantType,
                    Username = cred?.UserName,
                    Password = cred?.Password,
                };

                TokenResponseDto tokenResponseDto = (await this.PostAsync<TokenResponseDto, TokenRequestDto>(url: url, body: tokenRequestDto, isFormUrlEncoded: true, useBearerToken: false)).Result;
                token = tokenResponseDto?.AccessToken;
            }

            return token;
        }

        /// <summary>
        /// Converts to user directory.
        /// </summary>
        /// <param name="searchUserResponseDtos">The search user response dtos.</param>
        /// <returns>List of <see cref="UserFromDirectory"/>.</returns>
        private List<UserFromDirectory> ConvertToUserDirectory(List<SearchUserResponseDto> searchUserResponseDtos)
        {
            List<UserFromDirectory> userFromDirectories = new List<UserFromDirectory>();

            if (searchUserResponseDtos != null && searchUserResponseDtos?.Any() == true)
            {
                foreach (SearchUserResponseDto searchUserResponseDto in searchUserResponseDtos)
                {
                    string sid = null;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !string.IsNullOrWhiteSpace(searchUserResponseDto.Attribute.ObjectSid))
                    {
                        sid = new System.Security.Principal.SecurityIdentifier(Convert.FromBase64String(searchUserResponseDto.Attribute.ObjectSid), 0).ToString();
                    }

                    UserFromDirectory userFromDirectory = new UserFromDirectory
                    {
                        FirstName = searchUserResponseDto.FirstName,
                        LastName = searchUserResponseDto.LastName,
                        Login = searchUserResponseDto.Username,
                        Domain = !string.IsNullOrWhiteSpace(searchUserResponseDto.Attribute.LdapEntryDn) ? searchUserResponseDto.Attribute.LdapEntryDn?.Split(',')?.FirstOrDefault(x => x.StartsWith("DC="))?.Split('=')?.LastOrDefault()?.ToUpper() : default,
                        Sid = sid,
                        Country = searchUserResponseDto.Attribute.Country,
                        Company = searchUserResponseDto.Attribute.Company,
                        Department = searchUserResponseDto.Attribute.Department,
                        DistinguishedName = searchUserResponseDto.Attribute.LdapEntryDn,
                        Email = searchUserResponseDto.Email,
                        IsEmployee = true,
                        Manager = searchUserResponseDto.Attribute.Manager,
                        Office = searchUserResponseDto.Attribute.PhysicalDeliveryOfficeName,
                    };

                    if (Guid.TryParse(searchUserResponseDto.Attribute.LdapId, out Guid resultLdapId))
                    {
                        userFromDirectory.Guid = resultLdapId;
                    }
                    else if (Guid.TryParse(searchUserResponseDto.Id, out Guid resultId))
                    {
                        userFromDirectory.Guid = resultId;
                    }

                    userFromDirectory.Site = userFromDirectory.Domain == "CORP" ? userFromDirectory.Office : searchUserResponseDto.Attribute.Description;

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

                    if (!string.IsNullOrWhiteSpace(userFromDirectory.Sid) || userFromDirectory.Guid != Guid.Empty)
                    {
                        userFromDirectories.Add(userFromDirectory);
                    }
                }
            }

            return userFromDirectories ?? new List<UserFromDirectory>();
        }
    }
}
