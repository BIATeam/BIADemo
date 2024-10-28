// <copyright file="ClientForHubService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The service for client for hub.
    /// </summary>
    public class ClientForHubService : IClientForHubService
    {
        private readonly ICollection<IClientForHubRepository> clientForHubRepositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientForHubService"/> class.
        /// </summary>
        /// <param name="clientForHubRepositories">The injected repositories of client for hub.</param>
        public ClientForHubService(ICollection<IClientForHubRepository> clientForHubRepositories)
        {
            this.clientForHubRepositories = clientForHubRepositories;
        }

        /// <inheritdoc/>
        public virtual async Task SendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext = null)
        {
            await Task.WhenAll(this.clientForHubRepositories.Select(x => x.SendMessage(targetedFeature, action, jsonContext)));
        }

        /// <inheritdoc/>
        public virtual async Task SendMessage(TargetedFeatureDto targetedFeature, string action, object objectToSerialize)
        {
            await Task.WhenAll(this.clientForHubRepositories.Select(x => x.SendMessage(targetedFeature, action, objectToSerialize)));
        }

        /// <inheritdoc/>
        public virtual async Task SendTargetedMessage(string parentKey, string featureName, string action, object objectToSerialize = null)
        {
            await Task.WhenAll(this.clientForHubRepositories.Select(x => x.SendTargetedMessage(parentKey, featureName, action, objectToSerialize)));
        }
    }
}
