// BIADemo only
// <copyright file="AuditUser.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Audit;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The user entity.
    /// </summary>
    public class UserAudit : AuditEntity, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int AuditId { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

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
    }
}