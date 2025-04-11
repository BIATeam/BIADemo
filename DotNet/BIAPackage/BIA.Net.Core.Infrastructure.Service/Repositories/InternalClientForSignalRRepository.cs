// <copyright file="InternalClientForSignalRRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Class for signalR client.
    /// </summary>
    /// <typeparam name="THub">The type of hub.</typeparam>
    public class InternalClientForSignalRRepository<THub> : ClientForSignalRRepositoryBase
        where THub : Hub
    {
        private readonly HubLifetimeManager<THub> hub;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalClientForSignalRRepository{THub}"/> class.
        /// </summary>
        /// <param name="options">Option for the hub.</param>
        /// <param name="hub">hub service manager.</param>
        public InternalClientForSignalRRepository(IOptions<CommonFeatures> options, HubLifetimeManager<THub> hub)
            : base(options)
        {
            this.hub = hub;
        }

        /// <summary>
        /// Send Message to redis using the hub.
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
                await this.hub.SendGroupAsync(
                    targetedFeature.GroupName,
                    action,
                    [jsonContext]);
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
        }
    }
}
