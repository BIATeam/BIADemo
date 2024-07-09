// <copyright file="HubForClients.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Presentation.Common.Features.HubForClients
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;

    /// <summary>
    /// HubForClients.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
    public class HubForClients : Hub
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="sTargetedFeature">The targeted feature.</param>
        /// <param name="action">The action.</param>
        /// <param name="jsonContext">The json context.</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// Joins the group.
        /// </summary>
        /// <param name="sTargetedFeature">The targeted feature.</param>
        /// <returns>Task.</returns>
        public Task JoinGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            if (string.IsNullOrEmpty(targetedFeature.ParentKey))
            {
                return this.Groups.AddToGroupAsync(this.Context.ConnectionId, targetedFeature.FeatureName);
            }
            else
            {
                return this.Groups.AddToGroupAsync(this.Context.ConnectionId, targetedFeature.ParentKey.ToString() + ">" + targetedFeature.FeatureName);
            }
        }

        /// <summary>
        /// Leaves the group.
        /// </summary>
        /// <param name="sTargetedFeature">The s targeted feature.</param>
        /// <returns>Task.</returns>
        public Task LeaveGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            if (string.IsNullOrEmpty(targetedFeature.ParentKey))
            {
                return this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, targetedFeature.FeatureName);
            }
            else
            {
                return this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, targetedFeature.ParentKey.ToString() + ">" + targetedFeature.FeatureName);
            }
        }
    }
}
