using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace BIA.Net.Core.WorkerService.Features.ClientForHub
{
    public class ClientForHubService : IHostedService, IDisposable
    {
        private static HubConnection connection;
        private bool starting = false;
        private static bool started = false;
        private readonly ClientForHubOptions _options;
        public ClientForHubService(ClientForHubOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!starting)
            {
                starting = true;
                connection = new HubConnectionBuilder()
                    .WithUrl(_options.SignalRUrl)
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
        public static async Task SendMessage(string action, string jsonContext)
        {
            while (!started)
            {
                await Task.Delay(200);
            }
            await connection.InvokeAsync("SendMessage", action, jsonContext);
        }
    }
}
