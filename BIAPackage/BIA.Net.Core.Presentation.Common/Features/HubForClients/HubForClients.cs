

namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class HubForClients : Hub
    {
        public async Task SendMessage(string action, string jsonContext)
        {
            await this.Clients.All.SendAsync(action, jsonContext);
        }
    }
}
