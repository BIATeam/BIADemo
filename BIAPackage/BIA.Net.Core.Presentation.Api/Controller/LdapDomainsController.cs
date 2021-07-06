// <copyright file="LdapDomainsController.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Presentation.Api.Controllers
{
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Linq;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = BIARights.LdapDomains.List)]
        public IActionResult GetAll()
        {
            var ldapDomains = this.configuration.Authentication.LdapDomains.Where(o => o.ContainsUser);
            return this.Ok(ldapDomains);
        }
    }
}