namespace BIA.Net.Core.Domain.Announcement.Entities
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

    public class AnnouncementType : BaseEntityVersioned<BiaAnnouncementType>
    {
        /// <summary>
        /// Gets or sets the announcement type translations.
        /// </summary>
        public virtual ICollection<AnnouncementTranslation> AnnouncementTypeTranslations { get; set; }
    }
}
