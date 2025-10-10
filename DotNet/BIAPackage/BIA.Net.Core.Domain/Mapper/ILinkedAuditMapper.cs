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
        bool IsJoinLinkedEntity { get; }
        Type LinkedEntityType { get; }
        string ReferenceEntityPropertyName { get; }
        string LinkedAuditEntityIdentifierPropertyName { get; }
        string LinkedAuditEntityDisplayPropertyName { get; }
    }
}
