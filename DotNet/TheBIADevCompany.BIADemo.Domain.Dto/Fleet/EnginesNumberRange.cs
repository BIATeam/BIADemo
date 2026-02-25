// BIADemo only
// <copyright file="EnginesNumberRange.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Fleet
{
    /// <summary>
    /// The ranges of engine numbers for advanced filters.
    /// </summary>
    public enum EnginesNumberRange
    {
        /// <summary>
        /// Zero.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// One or two.
        /// </summary>
        OneOrTwo = 1,

        /// <summary>
        /// Three to five.
        /// </summary>
        ThreeToFive = 2,

        /// <summary>
        /// Six or more.
        /// </summary>
        SixOrMore = 3,
    }
}