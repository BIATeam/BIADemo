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
        /// <param name="groupName">the client group name</param>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendMessage(string groupName,  string action, string jsonContext);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="groupName">the client group name</param>
        /// <param name="action">action to send</param>
        /// <param name="objectToSerialize">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendMessage(string groupName, string action, object objectToSerialize);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="siteId">the site Id</param>
        /// <param name="groupName">the client group name</param>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendSiteMessage(int siteId, string groupName, string action, string jsonContext);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="siteId">the site Id</param>
        /// <param name="groupName">the client group name</param>
        /// <param name="action">action to send</param>
        /// <param name="objectToSerialize">context at json format</param>
        /// <returns>Send message on an action</returns>
        Task SendSiteMessage(int siteId, string groupName, string action, object objectToSerialize);
    }
}