// <copyright file="AttributesDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Attributes Dto.
    /// </summary>
    internal class AttributesDto
    {
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [JsonProperty("company")]
        public List<string> Company { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        [JsonProperty("Department")]
        public List<string> Department { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonProperty("Description")]
        public List<string> Description { get; set; }

        /// <summary>
        /// Gets or sets the LDAP entry dn.
        /// </summary>
        [JsonProperty("LDAP_ENTRY_DN")]
        public List<string> LdapEntryDn { get; set; }

        /// <summary>
        /// Gets or sets the name of the physical delivery office.
        /// </summary>
        [JsonProperty("PhysicalDeliveryOfficeName")]
        public List<string> PhysicalDeliveryOfficeName { get; set; }

        /// <summary>
        /// Gets or sets the object sid.
        /// </summary>
        [JsonProperty("ObjectSid")]
        public List<string> ObjectSid { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [JsonProperty("Country")]
        public List<string> Country { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        [JsonProperty("Manager")]
        public List<string> Manager { get; set; }

        /// <summary>
        /// Gets or sets the LDAP identifier.
        /// </summary>
        [JsonProperty("LDAP_ID")]
        public List<string> LdapId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonProperty("title")]
        public List<string> Title { get; set; }
    }
}
