namespace BIA.Net.Core.Domain.Dto.Historic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;

    public class EntityHistoricEntryDto
    {
        public List<EntityHistoricEntryModification> EntryModifications { get; set; } = [];
        public EntityHistoricEntryType EntryType { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string EntryUserLogin { get; set; }
    }
}
