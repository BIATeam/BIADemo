// <copyright file="User.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Collections.Generic;

    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The user entity.
    /// </summary>
    public class User : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the security id.
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        public string Domain { get; set; }

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

        /// <summary>
        /// Gets or sets the DAI date.
        /// </summary>
        public DateTime DaiDate { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the last login date.
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of view user.
        /// </summary>
        public ICollection<ViewUser> ViewUsers { get; set; }
    }
}