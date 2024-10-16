namespace TheBIADevCompany.BIADemo.Domain.Utilities.DocumentAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum.DocumentAnalysis;

    public class DocumentWord
    {
        public string Text { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public TextOrientation TextOrientation { get; set; }
    }
}
