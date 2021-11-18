// <copyright file="AppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The base class for all application service.
    /// </summary>
    public abstract class AppServiceBase<TEntity>
                where TEntity : class, IEntity
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected UserContext userContext = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected AppServiceBase(ITGenericRepository<TEntity> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected ITGenericRepository<TEntity> Repository { get; }

        /// <summary>
        /// Init the mapper and the user context
        /// </summary>
        /// <typeparam name="TOtherDto"></typeparam>
        /// <typeparam name="TOtherMapper"></typeparam>
        /// <returns></returns>
        protected TOtherMapper InitMapper<TOtherDto, TOtherMapper>()
            where TOtherDto : BaseDto, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
        {
            TOtherMapper mapper = new TOtherMapper();
            if (this.userContext != null)
            {
                mapper.UserContext = this.userContext;
            }
            return mapper;
        }
    }
}