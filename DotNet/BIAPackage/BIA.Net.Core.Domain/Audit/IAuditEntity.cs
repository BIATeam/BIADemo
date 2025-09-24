namespace BIA.Net.Core.Domain.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The entity audit interface.
    /// </summary>
    public interface IAuditEntity : IAudit
    {
        string EntityId { get; set; }
    }
}
