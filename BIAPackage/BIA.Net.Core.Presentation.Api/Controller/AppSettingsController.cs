namespace BIA.Net.Core.Presentation.Api.Controller
{
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class AppSettingsController : BiaControllerBase
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapDomainsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AppSettingsController(IOptions<BiaNetSection> configuration)
        {
            this.configuration = configuration.Value;
        }

        /// <summary>
        /// Ping to test response.
        /// </summary>
        /// <returns>The Application settings.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return this.Ok(new AppSettingsDto { Environment=this.configuration.Environment, Cultures= this.configuration.Cultures});
        }
    }
}
