namespace BIA.Net.Core.Domain.Dto.Historic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class EntityHistoricalEntryModification
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool IsLinkedProperty { get; set; }
    }
}
