// <copyright file="TargetedFeatureDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Domain.Dto.Base
{
    /// <summary>
    /// Targeted Feature Dto.
    /// </summary>
    public class TargetedFeatureDto
    {
        /// <summary>
        /// Feature Name.
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// Parent Key.
        /// </summary>
        public string ParentKey { get; set; }
    }
}
