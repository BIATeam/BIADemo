// <copyright file="BiaControllerBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Controller.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The base class for BIA controllers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = AuthenticationConfiguration.JwtBearerDefault)]
    public abstract class BiaControllerBase : ControllerBase
    {
        /// <summary>
        /// The option controller existence cache.
        /// </summary>
        private static readonly Dictionary<string, bool> OptionControllerExistenceCache = new Dictionary<string, bool>();

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        protected string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the entity name plural.
        /// </summary>
        protected string EntityNamePlural { get; set; }

        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="role">the role.</param>
        /// <returns>true if authorized.</returns>
        /// <summary>
        /// Gets the type of the current controller and checks if an associated Option controller exists.
        /// </summary>
        /// <returns>True if an associated Option controller exists, otherwise false.</returns>
        protected bool HasAssociatedOptionController()
        {
            System.Type currentType = this.GetType();
            string currentTypeName = currentType.Name;
            string currentNamespace = currentType.Namespace;

            string cacheKey = currentNamespace + "." + currentTypeName;
            if (OptionControllerExistenceCache.ContainsKey(cacheKey))
            {
                return OptionControllerExistenceCache[cacheKey];
            }

            string optionControllerName = string.Empty;
            if (currentTypeName.EndsWith("Controller"))
            {
                string baseName = currentTypeName.Substring(0, currentTypeName.Length - "Controller".Length);
                if (baseName.EndsWith('s'))
                {
                    baseName = baseName.Substring(0, baseName.Length - 1);
                }

                optionControllerName = baseName + "OptionsController";
            }

            System.Type optionControllerType = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == optionControllerName && t.Namespace == currentNamespace);
            bool found = optionControllerType != null;

            OptionControllerExistenceCache.Add(cacheKey, found);

            return found;
        }

        /// <summary>
        /// Notifies clients that an entity has changed.
        /// </summary>
        /// <param name="clientForHubService">The client for hub service.</param>
        /// <param name="parentKey">The parent key (optional).</param>
        /// <param name="parentKeys">The parent keys (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected async Task SendEntityChangedAsync(
            IClientForHubService clientForHubService,
            string parentKey = null,
            List<string> parentKeys = null)
        {
            if (clientForHubService == null ||
                string.IsNullOrWhiteSpace(this.EntityName) ||
                string.IsNullOrWhiteSpace(this.EntityNamePlural))
            {
                return;
            }

            string actionRefresh = "refresh-" + this.EntityNamePlural;
            if (!string.IsNullOrWhiteSpace(parentKey))
            {
                await clientForHubService.SendTargetedMessage(parentKey, this.EntityNamePlural, actionRefresh);
            }
            else if (parentKeys?.Any() == true)
            {
                foreach (string key in parentKeys.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    await clientForHubService.SendTargetedMessage(key, this.EntityNamePlural, actionRefresh);
                }
            }
            else
            {
                await clientForHubService.SendTargetedMessage(string.Empty, this.EntityNamePlural, actionRefresh);
            }

            if (this.HasAssociatedOptionController())
            {
                await clientForHubService.SendEntityChangedAsync($"domain-{this.EntityName}-options");
            }
        }

        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="role">the role.</param>
        /// <returns>true if authorized.</returns>
        protected bool IsAuthorize(string role)
        {
            if (!this.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role))
            {
                return false;
            }

            return true;
        }
    }
}