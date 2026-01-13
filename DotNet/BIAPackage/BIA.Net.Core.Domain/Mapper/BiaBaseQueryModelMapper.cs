// <copyright file="BiaBaseQueryModelMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// Base class for mappers with intermediate model query.
    /// </summary>
    /// <typeparam name="TQueryModel">The query model type.</typeparam>
    /// <typeparam name="TDto">The dto type.</typeparam>
    /// <typeparam name="TDtoListItem">The dto list item type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity key type.</typeparam>
    /// <typeparam name="TMapper">The mapper between <typeparamref name="TEntity"/> and <typeparamref name="TDto"/>.</typeparam>
    public abstract class BiaBaseQueryModelMapper<TQueryModel, TDto, TDtoListItem, TEntity, TKey, TMapper> : BiaBaseMapper<TQueryModel, TEntity, TKey>
        where TQueryModel : BaseDto<TKey>
        where TDto : BaseDto<TKey>
        where TDtoListItem : BaseDto<TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
    {
        private readonly TMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaBaseQueryModelMapper{TQueryModel, TDto, TDtoListItem, TEntity, TKey, TMapper}"/> class.
        /// </summary>
        /// <param name="mapper">The mapper between <typeparamref name="TEntity"/> and <typeparamref name="TDto"/>.</param>
        protected BiaBaseQueryModelMapper(TMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public override ExpressionCollection<TEntity> ExpressionCollection => this.mapper.ExpressionCollection;

        /// <summary>
        /// Converts a query model instance to its corresponding data transfer object (DTO) representation.
        /// </summary>
        /// <param name="queryModel">The query model to convert. Cannot be null.</param>
        /// <returns>A DTO that represents the specified query model.</returns>
        public abstract TDto QueryModelToDto(TQueryModel queryModel);

        /// <summary>
        /// Converts a collection of query model objects to a sequence of DTO list items.
        /// </summary>
        /// <remarks>Each query model in the input collection is converted to a DTO list item using the
        /// <see cref="QueryModelToDtoListItem"/> method. The conversion is performed lazily as the returned sequence is
        /// enumerated.</remarks>
        /// <param name="queryModels">The collection of query model objects to convert. If null, an empty sequence is returned.</param>
        /// <returns>An enumerable collection of DTO list items corresponding to the provided query models. The sequence is empty
        /// if <paramref name="queryModels"/> is null.</returns>
        public virtual IEnumerable<TDtoListItem> QueryModelsToDtoListItems(IEnumerable<TQueryModel> queryModels)
        {
            if (queryModels == null)
            {
                yield break;
            }

            foreach (var queryModel in queryModels)
            {
                yield return this.QueryModelToDtoListItem(queryModel);
            }
        }

        /// <summary>
        /// Converts a query model instance to a corresponding data transfer object (DTO) list item.
        /// </summary>
        /// <param name="queryModel">The query model instance to convert. Cannot be null.</param>
        /// <returns>A DTO list item that represents the specified query model.</returns>
        protected abstract TDtoListItem QueryModelToDtoListItem(TQueryModel queryModel);
    }
}
