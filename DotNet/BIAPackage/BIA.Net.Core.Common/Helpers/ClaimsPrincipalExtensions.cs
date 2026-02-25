// <copyright file="ClaimsPrincipalExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension class to extract value form claims.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the claim value.
        /// </summary>
        /// <param name="identity">The ClaimsPrincipal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>The claim value.</returns>
        public static string GetClaimValue(this ClaimsPrincipal identity, string claimType)
        {
            if (!identity.HasClaim(x => x.Type == claimType))
            {
                return string.Empty;
            }

            return identity.FindFirst(x => x.Type == claimType).Value;
        }

        /// <summary>
        /// Gets the claim values.
        /// </summary>
        /// <param name="identity">The ClaimsPrincipal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>The claim values.</returns>
        public static IEnumerable<string> GetClaimValues(this ClaimsPrincipal identity, string claimType)
        {
            if (!identity.HasClaim(x => x.Type == claimType))
            {
                return new List<string>();
            }

            return identity.FindAll(x => x.Type == claimType).Select(s => s.Value).ToList();
        }

        /// <summary>
        /// Gets the claim value stored as JSON as a specific type.
        /// </summary>
        /// <typeparam name="T">Specific type.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>Claim value as specific type, or default.</returns>
        public static T GetClaimValueJsonAs<T>(this ClaimsPrincipal principal, string claimType)
        {
            var permissionIdsClaim = principal.GetClaimValue(claimType);
            if (!string.IsNullOrEmpty(permissionIdsClaim))
            {
                return JsonConvert.DeserializeObject<T>(permissionIdsClaim);
            }

            return default;
        }
    }
}
