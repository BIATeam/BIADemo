// <copyright file="UserRoleSelectResult.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// The select result used to get user role.
    /// </summary>
    public class UserRoleSelectResult
    {
        /// <summary>
        /// The user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The list of roles.
        /// </summary>
        public IEnumerable<KeyValuePair<int, string>> Roles { get; set; }
    }
}