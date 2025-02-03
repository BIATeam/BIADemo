namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEntityFixable<TKey> : IEntity<TKey>
    {
        public bool IsFixed { get; set; }
        public DateTime? FixedDate { get; set; }
    }
}
