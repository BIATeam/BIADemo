namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    /// <summary>
    /// Represents a word in a document.
    /// </summary>
    public class DocumentWord
    {
        /// <summary>
        /// Text value.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// X position in document page (bottom left).
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// Y position in document page (bottom left).
        /// </summary>
        public double PositionY { get; set; }

        /// <summary>
        /// Text rotation.
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Text height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Text orientation.
        /// </summary>
        public TextOrientation Orientation { get; set; }
    }
}
