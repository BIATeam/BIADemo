namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Features.ClientForHub;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class SignalRClientForHubRepository : IClientForHubRepository
    {
        private static HubConnection connection = null;
        private static bool starting = false;
        private static bool started = false;

        public SignalRClientForHubRepository()
        {

        }

        public Task StartAsync()
        {
            if (!ClientForHubOptions.IsActive)
            {
                throw new Exception("The ClientForHub feature is not activated before use ClientForHubRepository. Verify your settings.");
            }

            if (!starting)
            {
                starting = true;
                connection = new HubConnectionBuilder()
                    .WithUrl(ClientForHubOptions.SignalRUrl)
                    .WithAutomaticReconnect()
                    .Build();
                connection.Closed += async (error) =>
                {
                    await Task.Delay(RandomNumberGenerator.GetInt32(1000, 5000));
                    await connection.StartAsync();
                };

                connection.StartAsync();

                started = true;
            }
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection.StopAsync();
        }

        public virtual void Dispose()
        {
            if (connection != null)
            {
                connection.StopAsync();
            }
        }

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        public async Task SendMessage(string featureName, string action, string jsonContext)
        {
            if (!starting && ! started)
            {
                _ = StartAsync();
            }
            while (!started)
            {
                await Task.Delay(200);
            }
            await connection.InvokeAsync("SendMessage", featureName, action, jsonContext);
        }

        public async Task SendMessage(string featureName, string action, object objectToSerialize)
        {
            await SendMessage(featureName, action, JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        public async Task SendTargetedMessage(string parentId, string featureName, string action, string jsonContext)
        {
            if (!starting && !started)
            {
                _ = StartAsync();
            }
            while (!started)
            {
                await Task.Delay(200);
            }
            await connection.InvokeAsync("SendTargetedMessage", parentId, featureName, action, jsonContext);
        }

        public async Task SendTargetedMessage(string parentId, string featureName, string action, object objectToSerialize)
        {
            await SendTargetedMessage(parentId, featureName, action, JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}
