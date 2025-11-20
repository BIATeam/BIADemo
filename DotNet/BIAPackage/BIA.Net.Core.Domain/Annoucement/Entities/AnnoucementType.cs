namespace BIA.Net.Core.Domain.Annoucement.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Translation.Entities;

    public class AnnoucementType : BaseEntityVersioned<BiaAnnoucementType>
    {
        /// <summary>
        /// Gets or sets the annoucement type translations.
        /// </summary>
        public virtual ICollection<AnnoucementTranslation> AnnoucementTypeTranslations { get; set; }
    }
}
