// <copyright file="ExternalClientForSignalRRepository.cs" company="BIA">
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
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// Class for signalR client.
    /// </summary>
    public class ExternalClientForSignalRRepository : ClientForSignalRRepositoryBase, IAsyncDisposable
    {
        private static readonly SemaphoreSlim IsStartingOrStoping = new SemaphoreSlim(1, 1);
        private static HubConnection connection = null;
        private static bool started = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalClientForSignalRRepository"/> class.
        /// </summary>
        /// <param name="options">Option for SignalR.</param>
        public ExternalClientForSignalRRepository(IOptions<CommonFeatures> options)
            : base(options)
        {
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
        /// Start the connection to the signalR Hub.
        /// </summary>
        /// <param name="signalRHubUrl">the signalR Hub Url.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task StartConnection(string signalRHubUrl)
        {
            await IsStartingOrStoping.WaitAsync();
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
                IsStartingOrStoping.Release();
            }
        }

        /// <summary>
        /// Stop the connection to the signalR Hub.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task StopConnection()
        {
            await IsSendingMessage.WaitAsync();
            try
            {
                await IsStartingOrStoping.WaitAsync();
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
                    IsStartingOrStoping.Release();
                }
            }
            finally
            {
                IsSendingMessage.Release();
            }
        }

        /// <summary>
        /// Send Message by secure connection.
        /// </summary>
        /// <param name="targetedFeature">the feature or domain name.</param>
        /// <param name="action">action to send.</param>
        /// <param name="jsonContext">context at json format.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task SafeSendMessage(TargetedFeatureDto targetedFeature, string action, string jsonContext)
        {
            await IsSendingMessage.WaitAsync();
            try
            {
                await StartConnection(this.ClientForHubConfiguration.SignalRUrl);
                await connection.InvokeAsync("SendMessage", JsonConvert.SerializeObject(targetedFeature), action, jsonContext);
            }
            finally
            {
                IsSendingMessage.Release();
            }
        }

        /// <inheritdoc/>
        protected override void TestConfig()
        {
            if (this.ClientForHubConfiguration?.IsActive != true)
            {
                var message = "The ClientForHub feature is not activated before use ClientForHubRepository. Verify your settings.";
                throw new BadRequestException(message);
            }

            if (string.IsNullOrEmpty(this.ClientForHubConfiguration.SignalRUrl))
            {
                var message = "The ClientForHub feature url is not specify before use ClientForHubRepository. Verify your settings.";
                throw new BadRequestException(message);
            }
        }
    }
}
