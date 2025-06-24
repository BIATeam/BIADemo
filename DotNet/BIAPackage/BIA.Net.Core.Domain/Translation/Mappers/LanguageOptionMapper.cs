// <copyright file="LanguageOptionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Translation.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The mapper used for permission option.
    /// </summary>
    public class LanguageOptionMapper : BiaBaseMapper<OptionDto, Language, int>
    {
        /// <inheritdoc />
        public override Expression<Func<Language, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.Name,
            };
        }
    }
}