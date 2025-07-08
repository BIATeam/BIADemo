// <copyright file="BaseUserFromDirectory.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Models
{
    using System;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The class representing a user from AD.
    /// </summary>
    [Serializable]
    public class BaseUserFromDirectory : IUserFromDirectory
    {
        /// <summary>
        /// Gets or sets the security Id.
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        public Guid Guid { get; set; }
    }
}