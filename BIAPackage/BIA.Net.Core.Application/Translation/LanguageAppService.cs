// <copyright file="LanguageAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Translation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The application service used for language.
    /// </summary>
    public class LanguageAppService : FilteredServiceBase<Language>, ILanguageAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userContext">The user context.</param>
        public LanguageAppService(ITGenericRepository<Language> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, LanguageOptionMapper>();
        }
    }
}