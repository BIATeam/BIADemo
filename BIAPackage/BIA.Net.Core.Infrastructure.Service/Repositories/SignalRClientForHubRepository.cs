namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Features.ClientForHub;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.AspNetCore.SignalR.Client;


    public class SignalRClientForHubRepository : IClientForHubRepository
    {
        private HubConnection connection;
        private bool starting = false;
        private bool started = false;

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

        }

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="action">action to send</param>
        /// <param name="jsonContext">context at json format</param>
        /// <returns>Send message on an action</returns>
        public async Task SendMessage(string action, string jsonContext)
        {
            if (!starting && ! started)
            {
                _ = StartAsync();
            }
            while (!started)
            {
                await Task.Delay(200);
            }
            await connection.InvokeAsync("SendMessage", action, jsonContext);
        }
    }
}
