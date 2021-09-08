// <copyright file="IClientForHubRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;
    public interface IClientForHubRepository
    {
        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendMessage(string action, string jsonContext);
    }
}