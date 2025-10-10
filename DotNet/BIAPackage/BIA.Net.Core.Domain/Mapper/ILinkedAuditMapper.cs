namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILinkedAuditMapper
    {
        Type LinkedAuditEntityType { get; }
        string ReferenceEntityPropertyName { get; }
        string LinkedAuditEntityDisplayPropertyName { get; }
        string LinkedAuditEntityIdentifierPropertyName { get; }
    }
}
