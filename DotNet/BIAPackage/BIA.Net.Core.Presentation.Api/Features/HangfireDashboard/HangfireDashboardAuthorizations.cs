// <copyright file="HangfireDashboardAuthorizations.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Features.HangfireDashboard
{
    using System.Collections.Generic;
    using Hangfire.Dashboard;

    /// <summary>
    /// HangfireDashboardAuthorizations.
    /// </summary>
    public class HangfireDashboardAuthorizations
    {
        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>
        /// The authorization.
        /// </value>
        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; }

        /// <summary>
        /// Gets or sets the authorization read only.
        /// </summary>
        /// <value>
        /// The authorization read only.
        /// </value>
        public IEnumerable<IDashboardAuthorizationFilter> AuthorizationReadOnly { get; set; }
    }
}
