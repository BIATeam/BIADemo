using BIA.Net.Core.Common.Configuration.WorkerFeature;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Presentation.Api.Features.HangfireDashboard
{
    public class HangfireDashboardAuthorizations
    {
        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; }
        public IEnumerable<IDashboardAuthorizationFilter> AuthorizationReadOnly { get; set; }
    }
}
