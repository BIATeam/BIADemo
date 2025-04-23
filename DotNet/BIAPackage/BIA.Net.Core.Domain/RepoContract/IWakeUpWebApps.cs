// <copyright file="IWakeUpWebApps.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BIADemoWebApiRepository.
    /// </summary>
    public interface IWakeUpWebApps
    {
        /// <summary>
        /// Wakeup the Apps pools.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public List<Task<(bool IsSuccessStatusCode, string ReasonPhrase)>> WakeUp();
    }
}
