// <copyright file="AppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using System.Collections.Generic;

    /// <summary>
    /// The base class for all application service.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Primary key type for the entity.</typeparam>
    public abstract class AppServiceBase<TEntity, TKey>
                where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// The user context (culture and langue).
        /// </summary>
        protected UserContext userContext = null;

        /// <summary>
        /// The user id.
        /// </summary>
        protected int userId;

         /// <summary>
        /// The user id.
        /// </summary>
        protected IEnumerable<string> userPermissions;

       /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected AppServiceBase(ITGenericRepository<TEntity, TKey> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected ITGenericRepository<TEntity, TKey> Repository { get; }

        /// <summary>
        /// Init the mapper and the user context.
        /// </summary>
        /// <typeparam name="TOtherDto">Dto type.</typeparam>
        /// <typeparam name="TOtherMapper">Dto to entity mapper type.</typeparam>
        /// <returns>The mapper.</returns>
        protected virtual TOtherMapper InitMapper<TOtherDto, TOtherMapper>()
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>, new()
        {
            TOtherMapper mapper = new TOtherMapper();
            if (this.userContext != null)
            {
                mapper.UserContext = this.userContext;
            }

            mapper.UserId = this.userId;
            mapper.UserPermissions = this.userPermissions;

            return mapper;
        }
    }
}