

namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    using BIA.Net.Core.Domain.Dto.Base;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class HubForClients : Hub
    {
        public async Task SendMessage(string sTargetedFeature, string action, string jsonContext)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            if (string.IsNullOrEmpty(targetedFeature.ParentKey))
            {
                await this.Clients.Group(targetedFeature.FeatureName).SendAsync(action, jsonContext);
            }
            else
            {
                await this.Clients.Group(targetedFeature.ParentKey.ToString() + ">" + targetedFeature.FeatureName).SendAsync(action, jsonContext);
            }
        }

        public Task JoinGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            if (string.IsNullOrEmpty(targetedFeature.ParentKey))
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, targetedFeature.FeatureName);
            }
            else
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, targetedFeature.ParentKey.ToString() + ">" + targetedFeature.FeatureName);
            }
        }
        public Task LeaveGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            if (string.IsNullOrEmpty(targetedFeature.ParentKey))
            {
                return Groups.RemoveFromGroupAsync(Context.ConnectionId, targetedFeature.FeatureName);
            }
            else
            {
                return Groups.RemoveFromGroupAsync(Context.ConnectionId, targetedFeature.ParentKey.ToString() + ">" + targetedFeature.FeatureName);

            }
        }
    }
}
