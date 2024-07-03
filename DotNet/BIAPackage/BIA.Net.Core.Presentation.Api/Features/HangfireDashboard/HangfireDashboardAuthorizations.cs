// <copyright file="HangfireDashboardAuthorizations.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Features.HangfireDashboard
{
    using System.Collections.Generic;
    using Hangfire.Dashboard;

    public class HangfireDashboardAuthorizations
    {
        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; }

        public IEnumerable<IDashboardAuthorizationFilter> AuthorizationReadOnly { get; set; }
    }
}
