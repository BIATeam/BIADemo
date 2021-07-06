// <copyright file="INotificationRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface base for INotificationRepository.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Sends the mail asynchronous.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="bodyText">The body text.</param>
        /// <param name="tos">The tos.</param>
        /// <param name="ccs">The CCS.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendNotificationAsync(string subject, string bodyText, IEnumerable<string> tos, IEnumerable<string> ccs = null);
    }
}