// <copyright file="AdditionalInfoDto.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// Adtitionnal Information send to the front. Can be customized through a partial class in the project.
    /// </summary>
    public class AdditionalInfoDto : BaseAdditionalInfoDto
    {
        // Begin BIADemo

        /// <summary>
        /// Example custom info.
        /// </summary>
        public string CustomInfo { get; set; }

        // End BIADemo
    }
}
