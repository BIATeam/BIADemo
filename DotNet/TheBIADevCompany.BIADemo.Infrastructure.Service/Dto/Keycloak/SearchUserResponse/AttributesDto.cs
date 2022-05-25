// <copyright file="AttributesDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Dto.Keycloak.SearchUserResponse
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Attributes Dto.
    /// </summary>
    internal class AttributesDto
    {
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [JsonIgnore]
        internal string Company => this.Companies?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        [JsonIgnore]
        internal string Department => this.Departments?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonIgnore]
        internal string Description => this.Descriptions?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the LDAP entry dn.
        /// </summary>
        [JsonIgnore]
        internal string LdapEntryDn => this.LdapEntryDns?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the name of the physical delivery office.
        /// </summary>
        [JsonIgnore]
        internal string PhysicalDeliveryOfficeName => this.PhysicalDeliveryOfficeNames?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the object sid.
        /// </summary>
        [JsonIgnore]
        internal string ObjectSid => this.ObjectSids?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [JsonIgnore]
        internal string Country => this.Countries?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        [JsonIgnore]
        internal string Manager => this.Managers?.FirstOrDefault();

        /// <summary>
        /// Gets the LDAP identifier.
        /// </summary>
        [JsonIgnore]
        internal string LdapId => this.LdapIds?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonIgnore]
        internal string Title => this.Titles?.FirstOrDefault();

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        [JsonProperty("company")]
        protected List<string> Companies { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        [JsonProperty("Department")]
        protected List<string> Departments { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonProperty("Description")]
        protected List<string> Descriptions { get; set; }

        /// <summary>
        /// Gets or sets the LDAP entry dn.
        /// </summary>
        [JsonProperty("LDAP_ENTRY_DN")]
        protected List<string> LdapEntryDns { get; set; }

        /// <summary>
        /// Gets or sets the name of the physical delivery office.
        /// </summary>
        [JsonProperty("PhysicalDeliveryOfficeName")]
        protected List<string> PhysicalDeliveryOfficeNames { get; set; }

        /// <summary>
        /// Gets or sets the object sid.
        /// </summary>
        [JsonProperty("ObjectSid")]
        protected List<string> ObjectSids { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [JsonProperty("Country")]
        protected List<string> Countries { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        [JsonProperty("Manager")]
        protected List<string> Managers { get; set; }

        /// <summary>
        /// Gets or sets the LDAP identifier.
        /// </summary>
        [JsonProperty("LDAP_ID")]
        protected List<string> LdapIds { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonProperty("title")]
        protected List<string> Titles { get; set; }
    }
}
