

namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class HubForClients : Hub
    {
        public async Task SendMessage(string featureName, string action, string jsonContext)
        {
            await this.Clients.Group(featureName).SendAsync(action, jsonContext);
        }
        public async Task SendTargetedMessage(string parentId, string featureName, string action, string jsonContext)
        {
            await this.Clients.Group(parentId.ToString()+ ">" + featureName).SendAsync(action, jsonContext);
        }
        public Task JoinGroup(string featureName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, featureName);
        }
        public Task LeaveGroup(string featureName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, featureName);
        }
    }
}
