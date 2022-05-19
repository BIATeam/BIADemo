// <copyright file="BIAClaimsPrincipal.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Authentication
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// A <see cref="ClaimsPrincipal"/> implementation with additional utility methods.
    /// </summary>
    /// <seealso cref="ClaimsPrincipal" />
#pragma warning disable S101 // Types should be named in PascalCase
    public class BIAClaimsPrincipal : ClaimsPrincipal
#pragma warning restore S101 // Types should be named in PascalCase
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
            int.TryParse(userId, out int result);

            return result;
        }

        /// <summary>
        /// Get the user login in the claims.
        /// </summary>
        /// <returns>The user login.</returns>
        public virtual string GetUserLogin()
        {
            return GetClaimValue(ClaimTypes.Name);
        }

        public virtual string GetUserFirstName()
        {
            return GetClaimValue(ClaimTypes.GivenName);
        }

        public virtual string GetUserLastName()
        {
            return GetClaimValue(ClaimTypes.Surname);
        }

        public virtual string GetUserCountry()
        {
            return GetClaimValue(ClaimTypes.Country);
        }

        public virtual string GetUserEmail()
        {
            return GetClaimValue(ClaimTypes.Email);
        }

        public virtual string GetSid()
        {
            var sValue = GetClaimValue(ClaimTypes.Sid);

            return new System.Security.Principal.SecurityIdentifier(System.Convert.FromBase64String(sValue), 0).ToString();
        }

        /// <summary>
        /// Get the user rights in the claims.
        /// </summary>
        /// <returns>The user rights.</returns>
        public virtual IEnumerable<string> GetUserPermissions()
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
                return default;
            }

            string json = this.FindFirst(x => x.Type == ClaimTypes.UserData).Value;

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        protected virtual string GetClaimValue(string claimType)
        {
            if (!this.HasClaim(x => x.Type == claimType))
            {
                return string.Empty;
            }

            return this.FindFirst(x => x.Type == claimType).Value;
        }
    }
}
