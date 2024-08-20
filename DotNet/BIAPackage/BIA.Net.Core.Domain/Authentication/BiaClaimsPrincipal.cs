// <copyright file="BiaClaimsPrincipal.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Helpers;
    using Newtonsoft.Json;

    /// <summary>
    /// A <see cref="ClaimsPrincipal"/> implementation with additional utility methods.
    /// </summary>
    /// <seealso cref="ClaimsPrincipal" />
    public class BiaClaimsPrincipal : ClaimsPrincipal
    {
        /// <summary>
        /// The role identifier.
        /// </summary>
        public const string RoleId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/roleid";

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaClaimsPrincipal"/> class.
        /// </summary>
        public BiaClaimsPrincipal()
        {
            // Do nothing.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaClaimsPrincipal"/> class from the given <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The base principal.</param>
        public BiaClaimsPrincipal(ClaimsPrincipal principal)
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
            return this.GetClaimValue(ClaimTypes.Name);
        }

        /// <summary>
        /// Get the user first name in the claims.
        /// </summary>
        /// <returns>The user first name (given name).</returns>
        public virtual string GetUserFirstName()
        {
            return this.GetClaimValue(ClaimTypes.GivenName);
        }

        /// <summary>
        /// Get the user last name in the claims.
        /// </summary>
        /// <returns>The user last name (surname).</returns>
        public virtual string GetUserLastName()
        {
            return this.GetClaimValue(ClaimTypes.Surname);
        }

        /// <summary>
        /// Gets list of groups where the user is a member.
        /// </summary>
        /// <param name="biaNetSection">The bia net section.</param>
        /// <returns>List of groups.</returns>
        public virtual IEnumerable<string> GetGroups(BiaNetSection biaNetSection = null)
        {
            IEnumerable<string> groupNames = default;

            if (biaNetSection?.Authentication?.Keycloak?.IsActive == true)
            {
                groupNames = this.GetClaimValues(CustomClaimTypes.Group);
            }
            else
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    WindowsIdentity identity = this.Identity as WindowsIdentity;

                    if (identity?.Groups?.Any() == true)
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        groupNames = identity.Groups.AsParallel().Select(id => id.Translate(typeof(NTAccount)).Value).ToList();
#pragma warning restore CA1416 // Validate platform compatibility
                    }
                }
            }

            return groupNames;
        }

        /// <summary>
        /// Get the user roles in the claims.
        /// This method is used to retrieve the roles contained in the token provided by the IdP.
        /// </summary>
        /// <returns>The user roles.</returns>
        public virtual IEnumerable<string> GetRoles()
        {
            return this.GetClaimValues(ClaimTypes.Role);
        }

        /// <summary>
        /// Get the user roles in the claims.
        /// This method is used to retrieve the roles contained in the token provided by the IdP.
        /// </summary>
        /// <returns>The user roles.</returns>
        public virtual IEnumerable<int> GetRoleIds()
        {
            return this.GetClaimValues(RoleId).Select(c => int.Parse(c));
        }

        /// <summary>
        /// Get the user rights in the claims.
        /// This method is called GetUserPermissions while we retrieve the roles. Because we use this claim to store the permissions in the application token.
        /// </summary>
        /// <returns>The user rights.</returns>
        public virtual IEnumerable<string> GetUserPermissions()
        {
            return this.GetClaimValues(ClaimTypes.Role);
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
    }
}
