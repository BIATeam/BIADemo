namespace BIA.Net.Core.Presentation.Api.Controller
{
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class EnvironmentController : BiaControllerBaseNoToken
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapDomainsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public EnvironmentController(IOptions<BiaNetSection> configuration)
        {
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Ping to test response.
        /// </summary>
        /// <returns>The JWT if authenticated.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return this.Ok(this.configuration.Environment);
        }
    }
}
