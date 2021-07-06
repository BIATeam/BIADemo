// <copyright file="BIAClaimsPrincipal.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Application.Authentication
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// A <see cref="ClaimsPrincipal"/> implementation with additional utility methods.
    /// </summary>
    /// <seealso cref="ClaimsPrincipal" />
    public class BIAClaimsPrincipal : ClaimsPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BIAClaimsPrincipal"/> class.
        /// </summary>
        public BIAClaimsPrincipal()
        {
            // Do nothing.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BIAClaimsPrincipal"/> class from the given <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The base principal.</param>
        public BIAClaimsPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
            // Do nothing.
        }

        /// <summary>
        /// Get the user identifier in the claims.
        /// </summary>
        /// <returns>The user identifier.</returns>
        public virtual int GetUserId()
        {
            if (!this.HasClaim(x => x.Type == ClaimTypes.Sid))
            {
                return 0;
            }

            var userId = this.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }

        /// <summary>
        /// Get the user login in the claims.
        /// </summary>
        /// <returns>The user login.</returns>
        public virtual string GetUserLogin()
        {
            if (!this.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                return string.Empty;
            }

            return this.FindFirst(x => x.Type == ClaimTypes.Name).Value;
        }

        /// <summary>
        /// Get the user rights in the claims.
        /// </summary>
        /// <returns>The user rights.</returns>
        public virtual IEnumerable<string> GetUserRights()
        {
            if (!this.HasClaim(x => x.Type == ClaimTypes.Role))
            {
                return new List<string>();
            }

            return this.FindAll(x => x.Type == ClaimTypes.Role).Select(s => s.Value).ToList();
        }

        /// <summary>
        /// Gets the user data in the claims.
        /// </summary>
        /// <typeparam name="T">Type of data we want to retrieve.</typeparam>
        /// <returns>the user data object.</returns>
        public virtual T GetUserData<T>()
        {
            if (!this.HasClaim(x => x.Type == ClaimTypes.UserData))
            {
                return default(T);
            }

            string json = this.FindFirst(x => x.Type == ClaimTypes.UserData).Value;

            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
