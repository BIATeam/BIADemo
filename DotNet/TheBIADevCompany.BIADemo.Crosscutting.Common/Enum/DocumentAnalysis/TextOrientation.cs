namespace TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
