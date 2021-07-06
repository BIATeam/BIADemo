using BIA.Net.Core.Common.Configuration.WorkerFeature;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.WorkerService.Features.HangfireServer
{
    public class HangfireServerOptions : HangfireServerConfiguration
    {
        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; }
    }
}
