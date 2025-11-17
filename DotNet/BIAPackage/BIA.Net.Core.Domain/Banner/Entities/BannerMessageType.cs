namespace BIA.Net.Core.Domain.Banner.Entities
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

    public class BannerMessageType : BaseEntityVersioned<BiaBannerMessageType>
    {
        /// <summary>
        /// Gets or sets the banner message type translations.
        /// </summary>
        public virtual ICollection<BannerMessageTypeTranslation> BannerMessageTypeTranslations { get; set; }
    }
}
