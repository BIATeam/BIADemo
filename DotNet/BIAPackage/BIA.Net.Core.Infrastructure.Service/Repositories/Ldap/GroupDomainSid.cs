// <copyright file="GroupDomainSid.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// class for Domain and Sid.
    /// </summary>
    [Serializable]
    public class GroupDomainSid
    {
        /// <summary>
        /// The domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The Sid.
        /// </summary>
        public string Sid { get; set; }
    }
}
