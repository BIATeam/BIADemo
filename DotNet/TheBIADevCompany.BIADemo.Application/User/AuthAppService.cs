// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Security.Principal;
#if BIA_FRONT_FEATURE
    using System.Threading.Tasks;
#endif
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#if BIA_FRONT_FEATURE
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
#endif

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Domain.Api.RolesForApp;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    // End BIADemo

    // End BIADemo

    /// <summary>
    /// Auth App Service.
    /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
    public class AuthAppService(
#if BIA_FRONT_FEATURE
        IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory> userAppService,
        IBaseTeamAppService<TeamTypeId> teamAppService,
        IRoleAppService roleAppService,
        IIdentityProviderRepository<UserFromDirectory> identityProviderRepository,
#endif
        IJwtFactory jwtFactory,
        IPrincipal principal,
        IUserPermissionDomainService userPermissionDomainService,
        ILogger<AuthAppService> logger,
        IConfiguration configuration,
        IOptions<BiaNetSection> biaNetconfiguration,
        IUserDirectoryRepository<UserFromDirectoryDto, UserFromDirectory> userDirectoryHelper,

        // Begin BIADemo
        IBiaDemoRoleApiRepository roleApiRepository,
        ISiteAppService siteAppService,
        IMemberAppService memberAppService,

        // End BIADemo
        ILdapRepositoryHelper ldapRepositoryHelper,
        IEnumerable<IPermissionConverter> permissionConverters)
#if BIA_FRONT_FEATURE
        : BaseFrontAuthAppService<UserDto, User, RoleId, TeamTypeId, UserFromDirectoryDto, UserFromDirectory, AdditionalInfoDto, UserDataDto>(
            userAppService,
            teamAppService,
            roleAppService,
            identityProviderRepository,
            jwtFactory,
            principal,
            userPermissionDomainService,
            logger,
            configuration,
            biaNetconfiguration,
            userDirectoryHelper,
            ldapRepositoryHelper,
            permissionConverters),
        IAuthAppService
#else
        : BaseAuthAppService<UserFromDirectoryDto, UserFromDirectory, AdditionalInfoDto, UserDataDto>(jwtFactory, principal, userPermissionDomainService, logger, configuration, biaNetconfiguration, userDirectoryHelper, ldapRepositoryHelper), IAuthAppService
#endif
#pragma warning restore SA1611 // Element parameters should be documented
    {
#if BIA_FRONT_FEATURE
        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public async Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam)
        {
            // Begin BIADemo
            RoleApi roleApiSection = this.Configuration.GetSection("RoleApi").Get<RoleApi>();

            if (roleApiSection != null && roleApiSection.GetRolesFromApi)
            {
                try
                {
                    await this.GetUserRolesFromApi(loginParam);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "Error during roles update : {ExceptionMessage}", ex.Message);
                }
            }

            // End BIADemo
            return await this.LoginOnTeamsAsync(loginParam, TeamConfig.Config);
        }

        /// <summary>
        /// Gets the identity key.
        /// </summary>
        /// <returns>The identity key.</returns>
        protected override string GetIdentityKey()
        {
            // If you change it parse all other #IdentityKey to align all (Database, Ldap, Idp, WindowsIdentity).
            return this.GetLogin();
        }

        // Begin BIADemo

        /// <inheritdoc/>
        protected override UserDataDto CreateUserData(UserInfoFromDBDto userInfoFromDBDto)
        {
            UserDataDto userDataDto = base.CreateUserData(userInfoFromDBDto);
            userDataDto.CustomData = $"This is a custom user data for user {this.GetLogin()}";
            return userDataDto;
        }

        /// <inheritdoc/>
        protected override AdditionalInfoDto CreateAdditionalInfo()
        {
            return new AdditionalInfoDto
            {
                CustomInfo = $"This is a custom additional info for user {this.GetLogin()}",
            };
        }

        private async Task GetUserRolesFromApi(LoginParamDto loginParam)
        {
            var allSites = await siteAppService.GetAllAsync(accessMode: AccessMode.All);

            RoleApi roleApiSection = this.Configuration.GetSection("RoleApi").Get<RoleApi>();
            Project projectSection = this.Configuration.GetSection("Project").Get<Project>();

            string identityKey = this.GetIdentityKey();

            // Call external api to get all roles for this application
            ApiRolesForApp rolesForApp = await roleApiRepository.GetRolesFromApi(projectSection?.ShortName, identityKey);

            UserInfoFromDBDto userInfoFromDB = await this.GetUserInfoFromDB(loginParam, identityKey);

            if (userInfoFromDB.Id != 0 && !rolesForApp.Sites.Any(site => site.AppRoles.Count > 0 || site.Programs.Any(program => program.AppRoles.Count > 0)))
            {
                // Deactivate the user
                await this.UserAppService.RemoveInGroupAsync(userInfoFromDB.Id);
            }
            else if (userInfoFromDB.Id == 0 || !userInfoFromDB.IsActive)
            {
                userInfoFromDB = await this.CreateOrUpdateUserInDatabase(this.GetSid(), identityKey, userInfoFromDB, ["User"]);
            }

            foreach (SiteDto site in allSites)
            {
                List<string> teamRoles = [];

                // Get list of roles of the user from API if needed
                if (!string.IsNullOrWhiteSpace(site.UniqueIdentifier) && roleApiSection != null && roleApiSection.GetRolesFromApi)
                {
                    ApiSite apiSite = rolesForApp.Sites.FirstOrDefault(apiSite => apiSite.UniqueIdentifier == site.UniqueIdentifier);
                    if (apiSite != null && apiSite.AppRoles.Count > 0)
                    {
                        teamRoles.AddRange(apiSite.AppRoles);
                    }
                }

                if (teamRoles.Count > 0)
                {
                    // Get Roles Id from Roles and RolesTeamTypes
                    var allRoles = await this.RoleAppService.GetAllTeamRolesAsync((int)TeamTypeId.Site);

                    // Check if user is member of the team. Create the link if it doesn't exist.
                    await memberAppService.AddUsers(
                        new MembersDto()
                        {
                            Users = [new OptionDto { Id = userInfoFromDB.Id }],
                            Roles = allRoles.Where(role => teamRoles.Contains(role.Code)).Select(role => new OptionDto { Id = role.Id, DtoState = DtoState.Added }),
                            TeamId = site.Id,
                        },
                        true);
                }
                else
                {
                    await memberAppService.RemoveRolesAndUserFromTeam(userInfoFromDB.Id, site.Id);
                }
            }
        }

        // End BIADemo
#endif
    }
}