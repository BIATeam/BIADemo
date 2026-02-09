namespace BIA.Net.Core.Presentation.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
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
        /// <param name="permissionConverters">A collection of permission converters used to map permission IDs to names.</param>
        /// <returns>A distinct enumerable of permission names associated with the principal.</returns>
        public static IEnumerable<string> GetPermissionNames(this ClaimsPrincipal principal, IEnumerable<IPermissionConverter> permissionConverters)
        {
            var permissionIds = principal.GetPermissionIds();
            if (permissionIds.Any())
            {
                return permissionConverters.SelectMany(converter => converter.ConvertToNames(permissionIds)).Distinct();
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
            var permissionClaims = principal.GetClaimValue(BiaClaimsPrincipal.PermissionIds);
            if (!string.IsNullOrEmpty(permissionClaims))
            {
                return JsonConvert.DeserializeObject<List<int>>(permissionClaims);
            }

            return [];
        }
    }
}
