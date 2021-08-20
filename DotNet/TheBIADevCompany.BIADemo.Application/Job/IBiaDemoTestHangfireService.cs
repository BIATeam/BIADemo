// BIADemo only
// <copyright file="BiaDemoTestHangfire.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Sample class to use a hangfire task.
    /// </summary>
    public interface IBiaDemoTestHangfireService
    {
        Task RunLongTask();
    }
}