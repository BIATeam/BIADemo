namespace BIA.Net.Core.Domain.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEntityArchivable<TKey> : IEntity<TKey>
    {
        public ArchiveStateEnum ArchiveState { get; set; }
    }
}
