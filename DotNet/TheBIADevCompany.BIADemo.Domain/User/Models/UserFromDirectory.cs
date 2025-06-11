// <copyright file="UserFromDirectory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Models
{
    using System;
    using BIA.Net.Core.Domain.User.Models;

    /// <summary>
    /// The class representing a user from AD.
    /// </summary>
    [Serializable]
    public class UserFromDirectory : BaseUserFromDirectory
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
#if BIA_USER_CUSTOM_FIELDS_BACK
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
#endif
    }
}