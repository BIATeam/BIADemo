// <copyright file="SignalRClientForHubRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class for signalR client.
    /// </summary>
    public class SignalRClientForHubRepository : IClientForHubRepository, IAsyncDisposable
    {
        private static HubConnection connection = null;
        private static SemaphoreSlim isStartingOrStoping = new SemaphoreSlim(1, 1);
        private static bool started = false;
        private static SemaphoreSlim isSendingMessage = new SemaphoreSlim(1, 1);
        private readonly ClientForHubConfiguration clientForHubConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRClientForHubRepository"/> class.
        /// </summary>
        /// <param name="options">Option for SignalR.</param>
        public SignalRClientForHubRepository(IOptions<CommonFeatures> options)
        {
            this.clientForHubConfiguration = options.Value.ClientForHub;
        }

        /// <inheritdoc/>
        public async Task SendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext = null)
        {
            this.TestConfig();
            await SafeSendMessage(targetedFeature, action, jsonContext, this.clientForHubConfiguration.SignalRUrl);
        }

        /// <inheritdoc/>
        public async Task SendMessage(TargetedFeatureDto targetedFeature, string action, object objectToSerialize)
        {
            await this.SendMessage(targetedFeature, action, objectToSerialize == null ? null : JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <inheritdoc/>
        public async Task SendTargetedMessage(string parentKey, string featureName, string action, object objectToSerialize = null)
        {
            await this.SendMessage(new TargetedFeatureDto { ParentKey = parentKey, FeatureName = featureName }, action, JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <summary>
        /// Dispose() calls DisposeAsyncCore().
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            // Perform async cleanup.
            await StopConnection();

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Send Message by secure connection.
        /// </summary>
        /// <param name="targetedFeature">the feature or domain name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="jsonContext">context at json format.</param>
        /// <param name="signalRHubUrl">the signalR Hub Url.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task SafeSendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext, string signalRHubUrl)
        {
            await isSendingMessage.WaitAsync();
            try
            {
                await StartConnection(signalRHubUrl);
                await connection.InvokeAsync("SendMessage", JsonConvert.SerializeObject(targetedFeature), action, jsonContext);
            }
            finally
            {
                isSendingMessage.Release();
            }
        }

        /// <summary>
        /// Start the connection to the signalR Hub.
        /// </summary>
        /// <param name="signalRHubUrl">the signalR Hub Url.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task StartConnection(string signalRHubUrl)
        {
            await isStartingOrStoping.WaitAsync();
            try
            {
                if (!started)
                {
                    connection = new HubConnectionBuilder()
                        .WithUrl(signalRHubUrl)
                        .WithAutomaticReconnect()
                        .Build();
                    connection.Closed += async (error) =>
                    {
                        await Task.Delay(RandomNumberGenerator.GetInt32(1000, 5000));
                        if (started)
                        {
                            await connection.StartAsync();
                        }
                    };

                    started = true;
                    await connection.StartAsync();
                }
            }
            finally
            {
                isStartingOrStoping.Release();
            }
        }

        /// <summary>
        /// Stop the connection to the signalR Hub.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task StopConnection()
        {
            await isSendingMessage.WaitAsync();
            try
            {
                await isStartingOrStoping.WaitAsync();
                try
                {
                    if (started)
                    {
                        started = false;
                        if (connection != null)
                        {
                            await connection.StopAsync();
                            await connection.DisposeAsync();
                        }
                    }
                }
                finally
                {
                    isStartingOrStoping.Release();
                }
            }
            finally
            {
                isSendingMessage.Release();
            }
        }

        private void TestConfig()
        {
            if (this.clientForHubConfiguration?.IsActive != true)
            {
                var message = "The ClientForHub feature is not activated before use ClientForHubRepository. Verify your settings.";
                throw new BadRequestException(message);
            }

            if (string.IsNullOrEmpty(this.clientForHubConfiguration.SignalRUrl))
            {
                var message = "The ClientForHub feature url is not specify before use ClientForHubRepository. Verify your settings.";
                throw new BadRequestException(message);
            }
        }
    }
}
