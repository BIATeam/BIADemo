namespace BIA.Net.Core.Domain.Dto.Historic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class EntityHistoricEntryModification
    {
        public string ColumnName { get; set; }
        public string OldDisplay { get; set; }
        public string NewDisplay { get; set; }
    }
}
