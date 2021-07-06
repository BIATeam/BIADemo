// <copyright file="Role.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System.Collections.Generic;

    using BIA.Net.Core.Domain;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class Role : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label in english.
        /// </summary>
        public string LabelEn { get; set; }

        /// <summary>
        /// Gets or sets the label in french.
        /// </summary>
        public string LabelFr { get; set; }

        /// <summary>
        /// Gets or sets the label in spanish.
        /// </summary>
        public string LabelEs { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the member roles.
        /// </summary>
        public virtual ICollection<MemberRole> MemberRoles { get; set; }
    }
}