// <copyright file="UserOptionMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The mapper used for user option.
    /// </summary>
    public class UserOptionMapper : BaseMapper<OptionDto, User, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<User, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.LastName + " " + entity.FirstName + " (" + entity.Login + ")",
            };
        }
    }
}