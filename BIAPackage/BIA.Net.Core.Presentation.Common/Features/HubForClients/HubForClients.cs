

namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class HubForClients : Hub
    {
        public async Task SendMessage(string groupName, string action, string jsonContext)
        {
            await this.Clients.Group(groupName).SendAsync(action, jsonContext);
        }
        public async Task SendSiteMessage(int siteId, string groupName, string action, string jsonContext)
        {
            await this.Clients.Group(siteId.ToString()+ ">" + groupName).SendAsync(action, jsonContext);
        }
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public Task LeaveGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
