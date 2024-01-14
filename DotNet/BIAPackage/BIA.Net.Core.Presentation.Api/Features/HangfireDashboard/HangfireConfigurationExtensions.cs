// <copyright file="HangfireConfigurationExtensions.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace Hangfire.Dashboard.JobLogs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using BIA.Net.Core.Presentation.Api.Features;
    using Microsoft.Extensions.FileSystemGlobbing.Internal;

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
        public static IGlobalConfiguration UseDashboardJobLogs(this IGlobalConfiguration configuration, string logfiles)
        {
            var assembly = typeof(ApiFeaturesExtensions).GetTypeInfo().Assembly;
            configuration.UseDashboardStylesheet(assembly, "BIA.Net.Core.Presentation.Api.Features.HangfireDashboard.BIAHangireDashboard.css");

            configuration.UseJobDetailsRenderer(100, dto =>
            {
                List<string> messages = null;
                if (!string.IsNullOrEmpty(logfiles))
                {
                    string fileName = logfiles.Replace("${hangfire-jobid}", dto.JobId);
                    string directory = fileName.Substring(0, fileName.LastIndexOf('\\'));
                    string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    name = name.Substring(0, name.LastIndexOf('.')) + "(\\.[0-9]*)?\\." + name.Substring(name.LastIndexOf('.') + 1);
                    name = Regex.Replace(name, "\\${.*}", ".*");

                    Regex reg = new Regex(name);

                    var files = Directory.GetFiles(directory)
                                         .Where(path => reg.IsMatch(path))
                                         .ToList();

                    if (files.Count > 0)
                    {
                        messages = new List<string>();

                        foreach (string file in files.OrderByDescending(f => f))
                        {
                            try
                            {
                                messages.AddRange(readAllLines(file).ToList());
                            }
                            catch (Exception e)
                            {
                                messages.Add("<div class='level-error'>Error in reading file : " + file + "<br>" + e.Message + "</div>");
                            }
                        }
                    }
                }
                else
                {
                    var jobStorageConnection = JobStorage.Current.GetConnection();
                    var logsMessages = jobStorageConnection.GetAllEntriesFromHash($"joblogs-jobId:{dto.JobId}");
                    if (logsMessages != null)
                    {
                        messages = logsMessages.Values.ToList();
                    }
                }

                var logString = "No log";
                if (messages != null)
                {
                    StringBuilder strBld = new();
                    foreach (string message in messages)
                    {
                        string level = "none";
                        string[] splittedMessage = message.Split('|');
                        if (splittedMessage.Length > 1)
                        {
                            level = splittedMessage[1].ToLower();
                        }

                        strBld.Append("<div class='level-" + level + "'>" + message + "</div>");
                    }

                    logString = strBld.ToString();
                }

                return new NonEscapedString($"<h3>Log messages</h3>" +
                    $"<div class=\"state-card \"><div class=\"state-card-body\">{logString}</div></div>" +
                    $"");
            });

            return configuration;
        }

        private static string[] readAllLines(string file)
        {
            string[] lines = null;
            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string all = sr.ReadToEnd();
                    lines = all.Split('\n');
                }
            }

            return lines;
        }
    }
}
