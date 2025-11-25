namespace BIA.Net.Core.Domain.Dto.Base.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPagingFilterFormatDto<TAdvancedFilter> : IPagingFilterFormatDto
    {
        /// <summary>
        /// Name of the property and her translation for file export.
        /// </summary>
        public TAdvancedFilter AdvancedFilter { get; set; }
    }

    public interface IPagingFilterFormatDto : ILazyLoadDto
    {
        /// <summary>
        /// Gets or sets the parent ids.
        /// </summary>
        public string[] ParentIds { get; set; }

        /// <summary>
        /// Name of the property and her translation for file export.
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }
    }
}
