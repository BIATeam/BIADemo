namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record class AuditChange(
        string ColumnName,
        object OriginalValue,
        string OriginalDisplay,
        object NewValue,
        string NewDisplay)
    {
    }
}
