namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    public class WorkerFeatures
    {
        /// <summary>
        /// Gets or sets the DatabaseHandler feature configuration.
        /// </summary>
        public DatabaseHandlerConfiguration DatabaseHandler { get; set; }

        /// <summary>
        /// Gets or sets the Hangfire Server feature configuration
        /// </summary>
        public HangfireServerConfiguration HangfireServer { get; set; }
    }
}
