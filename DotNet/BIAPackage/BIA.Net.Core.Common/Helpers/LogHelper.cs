// <copyright file="LogHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Log Helper.
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Begins the specified logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="text">The text.</param>
        /// <returns><see cref="Stopwatch"/> started.</returns>
        public static Stopwatch Begin(ILogger<object> logger, string className, string methodName, string text = "Begin")
        {
            logger.LogInformation("{ClassName} - {MethodName} - {Text}", className, methodName, text);

            return Stopwatch.StartNew();
        }

        /// <summary>
        /// Ends the specified logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <param name="text">The text.</param>
        /// <param name="warningTime">Time in second after which it logs a warning instead of information (Default 30s).</param>
        public static void End(ILogger<object> logger, string className, string methodName, Stopwatch stopwatch, string text = "End", double warningTime = 30d)
        {
            stopwatch.Stop();

            double time = Math.Round(stopwatch.Elapsed.TotalSeconds, 2);
            const string msg = "{ClassName} - {MethodName} - {Text} - Execution time = {Time}s";

            if (time > warningTime)
            {
                logger.LogWarning(msg, className, methodName, text, time);
            }
            else
            {
                logger.LogInformation(msg, className, methodName, text, time);
            }
        }
    }
}
