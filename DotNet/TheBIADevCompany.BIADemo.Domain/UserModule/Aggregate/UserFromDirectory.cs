// <copyright file="UserFromDirectory.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
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

        /// <summary>
        /// UpdateUserField with the userFromDirectory object.
        /// </summary>
        /// <param name="user">the user object to update.</param>
        /// <param name="userDirectory">the user from directory object containing values.</param>
        public static void UpdateUserFieldFromDirectory(User user, UserFromDirectory userDirectory)
        {
            user.Guid = userDirectory.Guid;
            user.Login = userDirectory.Login;
            user.Domain = userDirectory.Domain;
            user.Sid = userDirectory.Sid;
            user.FirstName = userDirectory.FirstName?.Length > 50 ? userDirectory.FirstName?.Substring(0, 50) : userDirectory.FirstName ?? string.Empty;
            user.LastName = userDirectory.LastName?.Length > 50 ? userDirectory.LastName?.Substring(0, 50) : userDirectory.LastName ?? string.Empty;
            user.IsActive = true;
            user.Country = userDirectory.Country?.Length > 10 ? userDirectory.Country?.Substring(0, 10) : userDirectory.Country ?? string.Empty;
            user.Department = userDirectory.Department?.Length > 50 ? userDirectory.Department?.Substring(0, 50) : userDirectory.Department ?? string.Empty;
            user.DistinguishedName = userDirectory.DistinguishedName?.Length > 250 ? userDirectory.DistinguishedName?.Substring(0, 250) : userDirectory.DistinguishedName ?? string.Empty;
            user.Manager = userDirectory.Manager?.Length > 250 ? userDirectory.Manager?.Substring(0, 250) : userDirectory.Manager;
            user.Email = userDirectory.Email?.Length > 256 ? userDirectory.Email?.Substring(0, 256) : userDirectory.Email ?? string.Empty;
            user.ExternalCompany = userDirectory.ExternalCompany?.Length > 50 ? userDirectory.ExternalCompany?.Substring(0, 50) : userDirectory.ExternalCompany;
            user.IsEmployee = userDirectory.IsEmployee;
            user.IsExternal = userDirectory.IsExternal;
            user.Company = userDirectory.Company?.Length > 50 ? userDirectory.Company?.Substring(0, 50) : userDirectory.Company ?? string.Empty;
            user.DaiDate = DateTime.Now;
            user.Office = userDirectory.Office?.Length > 20 ? userDirectory.Office?.Substring(0, 20) : userDirectory.Office ?? string.Empty;
            user.Site = userDirectory.Site?.Length > 50 ? userDirectory.Site?.Substring(0, 50) : userDirectory.Site ?? string.Empty;
            user.SubDepartment = userDirectory.SubDepartment?.Length > 50 ? userDirectory.SubDepartment?.Substring(0, 50) : userDirectory.SubDepartment;
        }
    }
}