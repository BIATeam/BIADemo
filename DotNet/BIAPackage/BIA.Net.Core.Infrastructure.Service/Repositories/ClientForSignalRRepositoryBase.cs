// <copyright file="ClientForSignalRRepositoryBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class for signalR client.
    /// </summary>
    public abstract class ClientForSignalRRepositoryBase : IClientForHubRepository
    {
        /// <summary>
        /// Semaphore for message sending.
        /// </summary>
        protected static readonly SemaphoreSlim IsSendingMessage = new SemaphoreSlim(1, 1);
        private readonly ClientForHubConfiguration clientForHubConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientForSignalRRepositoryBase"/> class.
        /// </summary>
        /// <param name="options">Option for SignalR.</param>
        protected ClientForSignalRRepositoryBase(IOptions<CommonFeatures> options)
        {
            this.clientForHubConfiguration = options.Value.ClientForHub;
        }

        /// <summary>
        /// Gets the configuration of the app.
        /// </summary>
        protected ClientForHubConfiguration ClientForHubConfiguration
        {
            get
            {
                return this.clientForHubConfiguration;
            }
        }

        /// <inheritdoc/>
        public async Task SendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext = null)
        {
            this.TestConfig();
            await this.SafeSendMessage(targetedFeature, action, jsonContext);
        }

        /// <inheritdoc/>
        public async Task SendMessage(TargetedFeatureDto targetedFeature, string action, object objectToSerialize)
        {
            await this.SendMessage(
                targetedFeature,
                action,
                objectToSerialize == null ? null : JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <inheritdoc/>
        public async Task SendTargetedMessage(string parentKey, string featureName, string action, object objectToSerialize = null)
        {
            await this.SendMessage(
                new TargetedFeatureDto { ParentKey = parentKey, FeatureName = featureName },
                action,
                JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <summary>
        /// Send Message by secure connection.
        /// </summary>
        /// <param name="targetedFeature">the feature or domain name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="jsonContext">context at json format.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task SafeSendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext);

        /// <summary>
        /// Check if config is OK for usage.
        /// </summary>
        protected abstract void TestConfig();
    }
}
