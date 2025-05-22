// <copyright file="UserFromDirectory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User.Models
{
    using System;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The class representing a user from AD.
    /// </summary>
    [Serializable]
    public class UserFromDirectory : IUserFromDirectory
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

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the distinguished name.
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is employee.
        /// </summary>
        public bool IsEmployee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is external.
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// Gets or sets the external company.
        /// </summary>
        public string ExternalCompany { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the sub department.
        /// </summary>
        public string SubDepartment { get; set; }

        /// <summary>
        /// Gets or sets the office.
        /// </summary>
        public string Office { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }
    }
}