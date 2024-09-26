// <copyright file="LdapDomainsController.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The API controller used to manage LDAP domains.
    /// </summary>
    public class LdapDomainsController : BiaControllerBase
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapDomainsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LdapDomainsController(IOptions<BiaNetSection> configuration)
        {
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Gets all existing LDAP domains.
        /// </summary>
        /// <returns>The list of LDAP domains.</returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<LdapDomain>>(StatusCodes.Status200OK)]
        [Authorize(Roles = BiaRights.LdapDomains.List)]
        public IActionResult GetAll()
        {
            IEnumerable<LdapDomain> ldapDomains = this.configuration?.Authentication?.LdapDomains?.Where(o => o.ContainsUser);
            ldapDomains = ldapDomains ?? new List<LdapDomain>();

            return this.Ok(ldapDomains);
        }
    }
}