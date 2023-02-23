using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Application.Helpers
{
    //public class PreserveOriginalQueueAttribute : JobFilterAttribute, IApplyStateFilter, IElectStateFilter
    //{
    //    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    //    {
    //        var id = context.BackgroundJob.Id;
    //        var state = context.Connection.GetStateData(id);
    //        var initialQueue = context.Connection.GetJobParameter(id, JobParam.InitialQueue);
    //        if (state?.Data!= null)
    //        {
    //            state.Data.TryGetValue("Queue", out string name);
    //            if (string.IsNullOrEmpty(initialQueue))
    //            {
    //                context.Connection.SetJobParameter(id, JobParam.InitialQueue, name);
    //            }
    //        }
    //        if (state != null)
    //        {
    //            if (state.Name.Equals(States.Scheduled) && state.Reason.StartsWith(Reason.Retry))
    //            {
    //                //FailedState
    //                if (context.NewState is EnqueuedState newState)
    //                {
    //                    newState.Queue = initialQueue;
    //                    context.Connection.SetJobParameter(context.BackgroundJob.Id, JobParam.QueueReason, Reason.Retry);
    //                }
    //            }
    //        }

    //        if (context.NewState is ProcessingState processingState)
    //        {
    //            //Remove QueueReason Param for subsequent manual triggers.

    //            context.Connection.SetJobParameter(context.BackgroundJob.Id, JobParam.QueueReason, null);
    //        }
    //    }
    //    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    //    {
    //    }

    //    /// For changing state from default to original queue for manual trigger

    //    public void OnStateElection(ElectStateContext context)
    //    {
    //        var initialQueue = context.Connection.GetJobParameter(context.BackgroundJob.Id, JobParam.InitialQueue);
    //        var queueReason = context.Connection.GetJobParameter(context.BackgroundJob.Id, JobParam.QueueReason);
    //        if (context.CurrentState != null && context.CurrentState.Equals(States.Enqueued) && !string.IsNullOrEmpty(initialQueue) && string.IsNullOrEmpty(queueReason))
    //        {
    //            context.Connection.SetJobParameter(context.BackgroundJob.Id, JobParam.QueueReason, Reason.Trigger);
    //            context.CandidateState = new EnqueuedState(initialQueue);
    //        }
    //    }
    //}
    //static internal class States
    //{
    //    public static readonly string Enqueued = "Enqueued";
    //    public static readonly string Scheduled = "Scheduled";
    //}
    //static internal class JobParam
    //{
    //    public static readonly string InitialQueue = "InitialQueue";
    //    public static readonly string QueueReason = "QueueReason";
    //}
    //static internal class Reason
    //{
    //    public static readonly string Trigger = "Trigger";
    //    public static readonly string Retry = "Retry";
    //}
}
