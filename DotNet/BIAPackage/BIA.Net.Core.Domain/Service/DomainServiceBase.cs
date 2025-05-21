// <copyright file="DomainServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for a service that need access to an <see cref="ITGenericRepository{TEntity, TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Primary key type for the entity.</typeparam>
    public abstract class DomainServiceBase<TEntity, TKey>
                where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainServiceBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected DomainServiceBase(ITGenericRepository<TEntity, TKey> repository)
        {
            this.Repository = repository;
            this.BiaNetSection = new BiaNetSection();
            var configuration = this.Repository.ServiceProvider.GetRequiredService<IConfiguration>();
            configuration?.GetSection("BiaNet").Bind(this.BiaNetSection);
        }

        /// <summary>
        /// The BIANet section from configuration.
        /// </summary>
        protected BiaNetSection BiaNetSection { get; private set; }

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
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>
        {
            TOtherMapper mapper = this.Repository.ServiceProvider.GetService<TOtherMapper>();

            return mapper;
        }
    }
}