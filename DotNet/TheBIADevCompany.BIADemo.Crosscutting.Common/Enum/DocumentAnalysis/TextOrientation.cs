// BIADemo only
// <copyright file="TextOrientation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis
{
    /// <summary>
    /// The text orientations.
    /// </summary>
    public enum TextOrientation
    {
        /// <summary>
        /// Horizontal text, left to right.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Horizontal text, upside down.
        /// </summary>
        Rotated180,

        /// <summary>
        /// Vertical text, going down.
        /// </summary>
        Rotated90,

        /// <summary>
        /// Vertical text, going up.
        /// </summary>
        Rotated270,

        /// <summary>
        /// Other rotated text.
        /// </summary>
        Rotated,
    }
}
