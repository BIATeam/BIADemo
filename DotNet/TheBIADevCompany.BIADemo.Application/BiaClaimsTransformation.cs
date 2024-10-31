// <copyright file="BiaClaimsTransformation.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Services;

    /// <summary>
    /// Bia Claims Transformation.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authentication.IClaimsTransformation" />
    public class BiaClaimsTransformation : IClaimsTransformation
    {
        /// <summary>
        /// The user permission domain service.
        /// </summary>
        private readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BiaClaimsTransformation> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaClaimsTransformation" /> class.
        /// </summary>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="logger">The logger.</param>
        public BiaClaimsTransformation(
            IUserPermissionDomainService userPermissionDomainService,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILogger<BiaClaimsTransformation> logger)
        {
            this.userPermissionDomainService = userPermissionDomainService;
            this.userDirectoryHelper = userDirectoryHelper;
            this.logger = logger;
        }

        /// <inheritdoc cref="IClaimsTransformation.TransformAsync"/>
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            this.logger.LogDebug("BiaClaimsTransformation.TransformAsync Begin");

            List<string> roles = await this.userDirectoryHelper.GetUserRolesAsync(
                claimsPrincipal: new BiaClaimsPrincipal(principal),
                userInfoDto: null,
                sid: principal.GetClaimValue(ClaimTypes.PrimarySid),
                domain: principal.Identity?.Name?.Split('\\').FirstOrDefault());

            if (roles?.Any() == true)
            {
                this.logger.LogDebug("BiaClaimsTransformation.TransformAsync nb roles: {NbRole}", roles.Count);

                List<string> permissions = this.userPermissionDomainService.TranslateRolesInPermissions(roles);

                if (permissions?.Any() == true)
                {
                    this.logger.LogDebug("BiaClaimsTransformation.TransformAsync nb permissions: {NbPermission}", permissions.Count);

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity();

                    foreach (string permission in permissions)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, permission));
                    }

                    principal.AddIdentity(claimsIdentity);
                }
            }

            return await Task.FromResult(principal);
        }
    }
}
