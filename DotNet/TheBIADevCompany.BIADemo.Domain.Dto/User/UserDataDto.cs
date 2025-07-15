// <copyright file="UserDataDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.User;
    using Newtonsoft.Json;

    /// <summary>
    /// UserData Dto.
    /// </summary>
#pragma warning disable S2094 // Classes should not be empty
    public class UserDataDto : BaseUserDataDto
#pragma warning restore S2094 // Classes should not be empty
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
