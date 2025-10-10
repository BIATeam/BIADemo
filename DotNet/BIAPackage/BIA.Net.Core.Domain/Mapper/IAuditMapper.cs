namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAuditMapper
    {
        Type EntityType { get; }
        List<ILinkedAuditMapper> LinkedAuditMappers { get; }
        List<IAuditPropertyMapper> AuditPropertyMappers { get; }
    }
}
