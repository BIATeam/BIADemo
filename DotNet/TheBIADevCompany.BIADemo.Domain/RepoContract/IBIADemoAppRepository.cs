// <copyright file="IBIADemoAppRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BIADemoWebApiRepository.
    /// </summary>
#pragma warning disable S101 // Types should be named in PascalCase
    public interface IBIADemoAppRepository
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Wakeup the App pool.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public Task<(bool IsSuccessStatusCode, string ReasonPhrase)> WakeUp();
    }
}
