// <copyright file="RoleOptionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapper used for role option.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RoleOptionMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class RoleOptionMapper(UserContext userContext) : BaseMapper<OptionDto, Role, int>
    {
        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; } = userContext;

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Role, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? entity.Label,
            });
        }
    }
}