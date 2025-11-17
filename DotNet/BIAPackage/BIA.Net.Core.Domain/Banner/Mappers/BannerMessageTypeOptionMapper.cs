// <copyright file="BannerMessageTypeOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Banner.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The mapper used for banner message type option.
    /// </summary>
    public class BannerMessageTypeOptionMapper : BaseMapper<TOptionDto<BiaBannerMessageType>, BannerMessageType, BiaBannerMessageType>
    {
        private readonly UserContext userContext;

        public BannerMessageTypeOptionMapper(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override Expression<Func<BannerMessageType, TOptionDto<BiaBannerMessageType>>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new TOptionDto<BiaBannerMessageType>
            {
                Display = entity.BannerMessageTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
            });
        }
    }
}
