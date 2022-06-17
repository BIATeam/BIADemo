namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    public class ApiFeatures
    {
        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public Swagger Swagger { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public HubForClientsConfiguration HubForClients { get; set; }

        /// <summary>
        /// Gets or sets the Hangfire dashboard feature configuration
        /// </summary>
        public HangfireDashboardConfiguration HangfireDashboard { get; set; }

        /// <summary>
        /// Gets or sets the DelegateJobToWorker feature configuration.
        /// </summary>
        public DelegateJobToWorker DelegateJobToWorker { get; set; }
    }
}
