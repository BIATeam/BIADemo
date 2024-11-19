// <copyright file="IClientForHubService.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// Interface for Client For Hub Repository.
    /// </summary>
    public interface IClientForHubService
    {
        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="targetedFeature">the feature or domain name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="jsonContext">context at json format.</param>
        /// <returns>Send message on an action.</returns>
        Task SendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext = null);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="targetedFeature">the client group name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="objectToSerialize">context at json format.</param>
        /// <returns>Send message on an action.</returns>
        Task SendMessage(TargetedFeatureDto targetedFeature, string action, object objectToSerialize);

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="parentKey">the parent key.</param>
        /// <param name="featureName">the feature or domain name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="objectToSerialize">context at json format.</param>
        /// <returns>Send message on an action.</returns>
        Task SendTargetedMessage(string parentKey, string featureName, string action, object objectToSerialize = null);
    }
}