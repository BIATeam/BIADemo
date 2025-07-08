// <copyright file="BasePrincipalMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System.Security.Principal;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class BasePrincipalMapper<TDto, TEntity, TKey> : BaseMapper<TDto, TEntity, TKey>
        where TDto : BaseDto<TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePrincipalMapper{TDto, TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public BasePrincipalMapper(IPrincipal principal)
            : base()
        {
            this.UserId = (principal as BiaClaimsPrincipal).GetUserId();
        }

        /// <summary>
        /// the user id.
        /// </summary>
        protected int UserId { get; set; }
    }
}