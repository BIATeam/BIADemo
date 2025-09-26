namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record class AuditLinkedEntityData(string EntityType, string IndexPropertyName, string IndexPropertyValue);
}
