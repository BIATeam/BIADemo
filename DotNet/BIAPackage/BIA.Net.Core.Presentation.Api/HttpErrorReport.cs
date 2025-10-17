namespace BIA.Net.Core.Presentation.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record class HttpErrorReport(int ErrorCode,  string ErrorMessage)
    {
    }
}
