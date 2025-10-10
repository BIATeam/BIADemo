namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AuditMapper<TAuditEntity> : IAuditMapper
    {
        public Type AuditType => typeof(TAuditEntity);
        public List<ILinkedAuditMapper> LinkedAuditMappers { get; set; }
    }
}
