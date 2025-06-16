// <copyright file="UserDataDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.Identity.Client;
    using Newtonsoft.Json;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class UserDataDto : BaseUserDataDto
    {
        // Begin BIADemo

        /// <summary>
        /// Example custom data.
        /// </summary>
        [JsonProperty("customData")]
        public string CustomData { get; set; }

        // End BIADemo
    }
}
