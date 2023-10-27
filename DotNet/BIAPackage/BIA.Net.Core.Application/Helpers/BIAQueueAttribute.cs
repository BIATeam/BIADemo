// <copyright file="BIAQueueAttribute.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hangfire.Common;
    using Hangfire.States;
    //public sealed class BIAQueueAttribute : JobFilterAttribute, IElectStateFilter
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="QueueAttribute"/> class
    //    /// using the specified queue name.
    //    /// </summary>
    //    /// <param name="queue">Queue name.</param>
    //    public BIAQueueAttribute()
    //    {
    //        Order = Int32.MaxValue;
    //    }

    //    /// <summary>
    //    /// Gets or sets the configuration.
    //    /// </summary>
    //    public static string QueueName { get; set; }

    //    public void OnStateElection(ElectStateContext context)
    //    {
    //        if (context.CandidateState is EnqueuedState enqueuedState)
    //        {
    //            enqueuedState.Queue = String.Format(QueueName, context.BackgroundJob.Job.Args.ToArray());
    //        }
    //    }
    //}
}
