namespace BIA.Net.Core.Domain.Translation.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Entity;

    public class BannerMessageTypeTranslation : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        ///  Gets or sets the banner message type.
        /// </summary>
        public BannerMessageType BannerMessageType { get; set; }

        /// <summary>
        /// Gets or sets the banner message type id.
        /// </summary>
        public BiaBannerMessageType BannerMessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}
