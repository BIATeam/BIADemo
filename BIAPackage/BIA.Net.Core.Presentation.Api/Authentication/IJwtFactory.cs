// <copyright file="IJwtFactory.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Authentication
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the JWT factory.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Generate the identity for a user.
        /// </summary>
        /// <param name="userName">The user name (login).</param>
        /// <param name="id">The user identifier.</param>
        /// <param name="roles">The role list of the user.</param>
        /// <returns>The claims identity.</returns>
        ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IEnumerable<string> roles, object userData = null);

        /// <summary>
        /// Generate an encoded JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <returns>The encoded JWT as string.</returns>
        Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity);

        /// <summary>
        /// Generate a JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <param name="additionalInfos">
        /// The additional information we want to let visible in the token.
        /// </param>
        /// <returns>The JWT as string.</returns>
        Task<object> GenerateJwtAsync(ClaimsIdentity identity, object additionalInfos);
    }
}