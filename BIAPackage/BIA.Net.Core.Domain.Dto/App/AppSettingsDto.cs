// <copyright file="OptionDto.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Option
{
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.Keycloak;
    using BIA.Net.Core.Domain.Dto.Base;
    using System.Collections.Generic;

    /// <summary>
    /// The DTO used to represent a AppSettings.
    /// </summary>
    public class AppSettingsDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Keycloak configuration.
        /// </summary>
        public Keycloak Keycloak { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public EnvironmentConfiguration Environment { get; set; }

        /// <summary>
        /// Gets or sets the cultures configuration.
        /// </summary>
        public IEnumerable<Culture> Cultures { get; set; }

        /// <summary>
        /// Url to Monitor.
        /// </summary>
        public string MonitoringUrl { get; set; }
    }
}
