// <copyright file="ICleanService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Clean
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for clean services.
    /// </summary>
    public interface ICleanService
    {
        /// <summary>
        /// Run the clean service.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task RunAsync();
    }
}
