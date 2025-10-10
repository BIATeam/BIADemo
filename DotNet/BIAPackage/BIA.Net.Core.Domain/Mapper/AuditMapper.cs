namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AuditMapper<TEntity> : IAuditMapper
    {
        public Type EntityType => typeof(TEntity);
        public List<ILinkedAuditMapper> LinkedAuditMappers { get; set; }
        public List<IAuditPropertyMapper> AuditPropertyMappers { get; set; }
    }
}
