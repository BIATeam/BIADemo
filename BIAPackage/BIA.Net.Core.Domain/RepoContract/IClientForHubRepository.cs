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
        /// <param name="featureName">the feature or domain name</param>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendMessage(string featureName,  string action, string jsonContext);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="featureName">the client group name</param>
        /// <param name="action">action to send</param>
        /// <param name="objectToSerialize">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendMessage(string featureName, string action, object objectToSerialize);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="parentId">the parent Id</param>
        /// <param name="featureName">the feature or domain name</param>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendTargetedMessage(string parentId, string featureName, string action, string jsonContext);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="parentId">the parent Id</param>
        /// <param name="featureName">the feature or domain name</param>
        /// <param name="action">action to send</param>
        /// <param name="objectToSerialize">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendTargetedMessage(string parentId, string featureName, string action, object objectToSerialize);
    }
}