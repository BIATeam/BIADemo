// <copyright file="AnnoucementTypeOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Annoucement.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The mapper used for annoucement type option.
    /// </summary>
    public class AnnoucementOptionMapper : BaseMapper<TOptionDto<BiaAnnoucementType>, AnnoucementType, BiaAnnoucementType>
    {
        private readonly UserContext userContext;

        public AnnoucementOptionMapper(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override Expression<Func<AnnoucementType, TOptionDto<BiaAnnoucementType>>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new TOptionDto<BiaAnnoucementType>
            {
                Display = entity.AnnoucementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
            });
        }
    }
}
