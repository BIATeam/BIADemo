// <copyright file="TeamOptionMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The mapper used for team option.
    /// </summary>
    public class TeamOptionMapper : BaseMapper<OptionDto, Team, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Team, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.Title,
            };
        }
    }
}