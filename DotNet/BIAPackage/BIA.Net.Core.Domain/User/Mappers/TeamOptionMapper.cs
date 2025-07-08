// <copyright file="TeamOptionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapper used for team option.
    /// </summary>
    public class TeamOptionMapper : BaseMapper<OptionDto, BaseEntityTeam, int>
    {
        /// <inheritdoc />
        public override Expression<Func<BaseEntityTeam, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.Title,
            });
        }
    }
}