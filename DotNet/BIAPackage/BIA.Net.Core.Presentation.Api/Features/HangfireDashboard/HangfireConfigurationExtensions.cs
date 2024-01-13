// <copyright file="HangfireConfigurationExtensions.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace Hangfire.Dashboard.JobLogs
{
    using System.Reflection;
    using System.Text;
    using BIA.Net.Core.Presentation.Api.Features;

    /// <summary>
    /// Provides extension methods to setup Hangfire.JobLogs.
    /// </summary>
    public static class HangfireConfigurationExtensions
    {
        /// <summary>
        /// Configures Hangfire to use JobLogs.
        /// </summary>
        /// <param name="configuration">Global configuration.</param>
        /// <returns>The global configuration.</returns>
        public static IGlobalConfiguration UseDashboardJobLogs(this IGlobalConfiguration configuration)
        {
            var assembly = typeof(ApiFeaturesExtensions).GetTypeInfo().Assembly;
            configuration.UseDashboardStylesheet(assembly, "BIA.Net.Core.Presentation.Api.Features.HangfireDashboard.BIAHangireDashboard.css");

            configuration.UseJobDetailsRenderer(100, dto =>
            {
                var jobStorageConnection = JobStorage.Current.GetConnection();
                var logsMessages = jobStorageConnection.GetAllEntriesFromHash($"joblogs-jobId:{dto.JobId}");

                var logString = "No log";
                if (logsMessages != null)
                {
                    StringBuilder strBld = new ();
                    foreach (string message in logsMessages.Values)
                    {
                        string level = "none";
                        string[] splittedMessage = message.Split('|');
                        if (splittedMessage.Length > 1)
                        {
                            level = splittedMessage[1].ToLower();
                        }

                        strBld.Append("<br><div class='level-" + level + "'>" + message + "</div>");
                    }

                    logString = strBld.ToString();
                }

                return new NonEscapedString($"<h3>Log messages</h3>" +
                    $"<div class=\"state-card \"><div class=\"state-card-body\">{logString}</div></div>" +
                    $"");
            });

            return configuration;
        }
    }
}
