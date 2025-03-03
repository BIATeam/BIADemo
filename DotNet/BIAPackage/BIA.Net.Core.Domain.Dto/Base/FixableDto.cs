namespace BIA.Net.Core.Domain.Dto.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FixableDto<TKey> : BaseDto<TKey>
    {
        public bool IsFixed { get; set; }
        public DateTime? FixedDate { get; set; }
    }
}
