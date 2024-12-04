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
            await this.Clients.Group(targetedFeature.GroupName).SendAsync(action, jsonContext);
        }

        /// <summary>
        /// Joins the group.
        /// </summary>
        /// <param name="sTargetedFeature">The targeted feature.</param>
        /// <returns>Task.</returns>
        public Task JoinGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            return this.Groups.AddToGroupAsync(this.Context.ConnectionId, targetedFeature.GroupName);
        }

        /// <summary>
        /// Leaves the group.
        /// </summary>
        /// <param name="sTargetedFeature">The s targeted feature.</param>
        /// <returns>Task.</returns>
        public Task LeaveGroup(string sTargetedFeature)
        {
            TargetedFeatureDto targetedFeature = JsonConvert.DeserializeObject<TargetedFeatureDto>(sTargetedFeature);
            return this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, targetedFeature.GroupName);
        }
    }
}
