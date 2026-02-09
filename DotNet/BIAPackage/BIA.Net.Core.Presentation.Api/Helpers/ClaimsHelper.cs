// <copyright file="ClaimsHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using BIA.Net.Core.Application.Permission;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper for claims.
    /// </summary>
    public static class ClaimsHelper
    {
        /// <summary>
        /// Returns a distinct collection of permission names for the specified principal using the provided permission
        /// converters.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal whose permissions are to be retrieved.</param>
        /// <param name="permissionService">The permission service.</param>
        /// <returns>A distinct enumerable of permission names associated with the principal.</returns>
        public static IEnumerable<string> GetPermissionNames(this ClaimsPrincipal principal, IPermissionService permissionService)
        {
            var permissionIds = principal.GetPermissionIds();
            if (permissionIds.Any())
            {
                return permissionIds.Select(permissionService.GetPermissionName).Distinct();
            }

            return [];
        }

        /// <summary>
        /// Retrieves the list of permission IDs from the claims of the specified principal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal instance from which to extract permission IDs.</param>
        /// <returns>An enumerable of permission IDs as integers.</returns>
        public static IEnumerable<int> GetPermissionIds(this ClaimsPrincipal principal)
        {
            var permissionIdsClaim = principal.GetClaimValue(BiaClaimsPrincipal.PermissionIds);
            if (!string.IsNullOrEmpty(permissionIdsClaim))
            {
                return JsonConvert.DeserializeObject<IEnumerable<int>>(permissionIdsClaim);
            }

            return [];
        }
    }
}
