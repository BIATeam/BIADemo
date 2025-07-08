// <copyright file="ProlongExpirationTimeAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using Hangfire.Common;
    using Hangfire.States;
    using Hangfire.Storage;

    /// <summary>
    /// Prolong the expiration time of Hanfire succeeded tasks to 1 week.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface)]
    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ProlongExpirationTimeAttribute" /> class.
        /// </summary>
        /// <param name="retentionDays">Number of days to keep succeeded tasks.</param>
        public ProlongExpirationTimeAttribute(int retentionDays)
        {
            this.RetentionDays = retentionDays;
        }

        /// <summary>
        /// Number of days to keep succeeded tasks.
        /// </summary>
        protected int RetentionDays { get; set; }

        /// <summary>
        /// On state unapplied for task event.
        /// </summary>
        /// <param name="context">Apply state context.</param>
        /// <param name="transaction">Write only transaction.</param>
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(this.RetentionDays);
        }

        /// <summary>
        /// On state applied for task event.
        /// </summary>
        /// <param name="context">Apply state context.</param>
        /// <param name="transaction">Write only transaction.</param>
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(this.RetentionDays);
        }
    }
}
