// <copyright file="UserExtendedMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class UserExtendedMapper(UserContext userContext) : UserMapper<UserExtendedDto, UserExtended>(userContext)
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<UserExtended> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<UserExtended>(base.ExpressionCollection)
                {
                    { HeaderNameExtended.Country, user => user.Country },
                };
            }
        }

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <param name="mapperMode">the mode for mapping.</param>
        /// <returns>The user DTO.</returns>
        public override Expression<Func<UserExtended, UserExtendedDto>> EntityToDto(string mapperMode)
        {
            return base.EntityToDto(mapperMode).CombineMapping(entity => new UserExtendedDto
            {
                Country = entity.Country,
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(UserExtendedDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderNameExtended.Country, () => CSVString(dto.Country) },
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<UserExtended, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.Roles,
            ];
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderNameExtended
        {
            /// <summary>
            /// header name LastName.
            /// </summary>
            public const string Country = "country";
        }
    }
}