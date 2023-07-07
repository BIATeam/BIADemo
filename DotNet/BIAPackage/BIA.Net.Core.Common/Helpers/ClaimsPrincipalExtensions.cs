// <copyright file="ClaimsPrincipalExtensions.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

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
    }
}
