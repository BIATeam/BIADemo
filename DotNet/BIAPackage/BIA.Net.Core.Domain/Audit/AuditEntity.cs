namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AuditEntity : BaseAudit, IAuditEntity
    {
        public string EntityId { get; set; }
        public string LinkedEntities { get; set; }
        public virtual void FillSpecificProperties<TEntity>(TEntity entity)
        {
        }
    }
}
