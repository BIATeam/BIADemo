// <copyright file="BaseMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class BaseMapper<TDto, TEntity, TKey> : BaseEntityMapper<TEntity>
        where TDto : BaseDto<TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// The dto is fixable.
        /// </summary>
        private readonly bool isFixable;

        /// <summary>
        /// The dto is archivable.
        /// </summary>
        private readonly bool isArchivable;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMapper{TDto, TEntity, TKey}"/> class.
        /// </summary>
        protected BaseMapper()
            : base()
        {
            if (typeof(IEntityFixable<TKey>).IsAssignableFrom(typeof(TEntity)) && typeof(IFixableDto).IsAssignableFrom(typeof(TDto)))
            {
                this.isFixable = true;
            }

            if (typeof(IEntityArchivable<TKey>).IsAssignableFrom(typeof(TEntity)) && typeof(IArchivableDto).IsAssignableFrom(typeof(TDto)))
            {
                this.isArchivable = true;
            }
        }

        /// <inheritdoc cref="BaseEntityMapper{TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<TEntity> ExpressionCollection
        {
            get
            {
                var expression = new ExpressionCollection<TEntity>
                {
                    { BaseHeaderName.Id, entity => entity.Id },
                };

                if (this.isFixable)
                {
                    expression.Add(BaseHeaderName.IsFixed, entity => (entity as IEntityFixable<TKey>).IsFixed);
                    expression.Add(BaseHeaderName.FixedDate, entity => (entity as IEntityFixable<TKey>).FixedDate);
                }

                if (this.isArchivable)
                {
                    expression.Add(BaseHeaderName.IsArchived, entity => (entity as IEntityArchivable<TKey>).IsArchived);
                    expression.Add(BaseHeaderName.ArchivedDate, entity => (entity as IEntityArchivable<TKey>).ArchivedDate);
                }

                return expression;
            }
        }

        /// <summary>
        /// Update a collection of entities from a collection of dto depending the dtoState of the dto.
        /// </summary>
        /// <typeparam name="TEmbeddedDto">Embedded item DTO type.</typeparam>
        /// <typeparam name="TEmbeddedEntity">Embedded item Entity type.</typeparam>
        /// <param name="dtoCollection">Collection of DTO to use à source.</param>
        /// <param name="entityCollection">Collection of entity to modify.</param>
        /// <param name="mapper">mapper of TEmbeddedDto and TEmbeddedEntity.</param>
        public static void MapEmbeddedItemToEntityCollection<TEmbeddedDto, TEmbeddedEntity>(
            ICollection<TEmbeddedDto> dtoCollection,
            ICollection<TEmbeddedEntity> entityCollection,
            BaseMapper<TEmbeddedDto, TEmbeddedEntity, int> mapper)
            where TEmbeddedDto : BaseDto<int>
            where TEmbeddedEntity : class, IEntity<int>, new()
        {
            foreach (TEmbeddedDto dto in dtoCollection)
            {
                TEmbeddedEntity entity;
                switch (dto.DtoState)
                {
                    case DtoState.Added:
                        entity = default(TEmbeddedEntity);
                        mapper.DtoToEntity(dto, ref entity);
                        entityCollection.Add(entity);
                        break;
                    case DtoState.Modified:
                        entity = entityCollection.FirstOrDefault(e => e.Id == dto.Id);
                        mapper.DtoToEntity(dto, ref entity);
                        break;
                    case DtoState.Deleted:
                        entityCollection.Remove(entityCollection.FirstOrDefault(e => e.Id == dto.Id));
                        break;
                }
            }
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        /// <param name="context">The context.</param>
        public virtual void DtoToEntity(TDto dto, ref TEntity entity, string mapperMode, IUnitOfWork context)
        {
            this.DtoToEntity(dto, ref entity, mapperMode);
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual void DtoToEntity(TDto dto, ref TEntity entity, string mapperMode)
        {
            this.DtoToEntity(dto, ref entity);
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        public virtual void DtoToEntity(TDto dto, ref TEntity entity)
        {
            if (entity == null)
            {
                entity = new TEntity
                {
                    Id = dto.Id,
                };
            }
        }

        /// <summary>
        /// Create a DTO from an entity.
        /// </summary>
        /// <returns>The created DTO.</returns>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual Expression<Func<TEntity, TDto>> EntityToDto(string mapperMode)
        {
            return this.EntityToDto();
        }

        /// <summary>
        /// Create a DTO from an entity.
        /// </summary>
        /// <returns>The created DTO.</returns>
        public virtual Expression<Func<TEntity, TDto>> EntityToDto()
        {
            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            var newDto = Expression.New(typeof(TDto));

            List<MemberBinding> bindings = new List<MemberBinding>();

            if (this.isFixable)
            {
                /// IsFixed = (entity as IEntityFixable<TKey>).IsFixed,
                /// FixedDate = (entity as IEntityFixable<TKey>).FixedDate,

                var isFixedProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityFixable<TKey>)),
                    nameof(IEntityFixable<TKey>.IsFixed));

                var fixedDateProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityFixable<TKey>)),
                    nameof(IEntityFixable<TKey>.FixedDate));

                var bindIsFixed = Expression.Bind(typeof(TDto).GetProperty(nameof(IFixableDto.IsFixed)), isFixedProperty);
                var bindFixedDate = Expression.Bind(typeof(TDto).GetProperty(nameof(IFixableDto.FixedDate)), fixedDateProperty);
                bindings.Add(bindIsFixed);
                bindings.Add(bindFixedDate);
            }

            if (this.isArchivable)
            {
                /// IsArchived = (entity as IEntityArchivable<TKey>).IsArchived,
                /// ArchivedDate = (entity as IEntityArchivable<TKey>).ArchivedDate,

                var isArchivedProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityArchivable<TKey>)),
                    nameof(IEntityArchivable<TKey>.IsArchived));

                var archivedDateProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityArchivable<TKey>)),
                    nameof(IEntityArchivable<TKey>.ArchivedDate));

                var bindIsArchived = Expression.Bind(typeof(TDto).GetProperty(nameof(IArchivableDto.IsArchived)), isArchivedProperty);
                var bindArchivedDate = Expression.Bind(typeof(TDto).GetProperty(nameof(IArchivableDto.ArchivedDate)), archivedDateProperty);
                bindings.Add(bindIsArchived);
                bindings.Add(bindArchivedDate);
            }

            // Id = entity.Id,
            var idProperty = Expression.Property(
                Expression.Convert(entityParam, typeof(IEntity<TKey>)),
                nameof(IEntity<TKey>.Id));

            var bindId = Expression.Bind(typeof(TDto).GetProperty(nameof(BaseDto<TKey>.Id)), idProperty);
            bindings.Add(bindId);

            var memberInit = Expression.MemberInit(newDto, bindings);

            return Expression.Lambda<Func<TEntity, TDto>>(memberInit, entityParam);
        }

        /// <summary>
        /// Map the entity essential keys after add, update and before delete to the dto return by the modifier function.
        /// </summary>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual void MapEntityKeysInDto(TEntity entity, TDto dto, string mapperMode)
        {
            this.MapEntityKeysInDto(entity, dto);
        }

        /// <summary>
        /// Map the entity essential keys after add, update and before delete to the dto return by the modifier function.
        /// </summary>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="dto">The DTO to use.</param>
        public virtual void MapEntityKeysInDto(TEntity entity, TDto dto)
        {
            dto.Id = entity.Id;
            if (entity is VersionedTable versionedEntity)
            {
                dto.RowVersion = Convert.ToBase64String(versionedEntity.RowVersion);
            }
        }

        /// <summary>
        /// Defining the includes to use in the key mapping before deleting related entities.
        /// </summary>
        /// <param name="mapperMode">The mode of mapping.</param>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesBeforeDelete(string mapperMode)
        {
            return this.IncludesBeforeDelete();
        }

        /// <summary>
        /// Defining the includes to use in the key mapping before deleting related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesBeforeDelete()
        {
            return Array.Empty<Expression<Func<TEntity, object>>>();
        }

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesForUpdate(string mapperMode)
        {
            return this.IncludesForUpdate();
        }

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesForUpdate()
        {
            return Array.Empty<Expression<Func<TEntity, object>>>();
        }

        /// <summary>
        /// Create a record from a DTO.
        /// </summary>
        /// <param name="mapperMode">The mode of mapping.</param>
        /// <param name="headerNames">The list of header names.</param>
        /// <returns>Func.</returns>
        public virtual Func<TDto, object[]> DtoToRecord(string mapperMode, List<string> headerNames = null)
        {
            return this.DtoToRecord(headerNames);
        }

        /// <summary>
        /// Create a record from a DTO.
        /// </summary>
        /// <param name="headerNames">The list of header names.</param>
        /// <returns>Func.</returns>
        public virtual Func<TDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = [];

                if (headerNames != null && headerNames.Count > 0)
                {
                    foreach (string headerName in headerNames)
                    {
                        records.Add(this.DtoToCell(x, headerName));
                    }
                }

                return [.. records];
            };
        }

        /// <summary>
        /// Dto to cell.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>a string formated for csv.<returns>
        public virtual string DtoToCell(TDto dto, string headerName)
        {
            switch (headerName.ToLowerInvariant())
            {
                case BaseHeaderName.Id:
                    return CSVCell(dto.Id);
                case BaseHeaderName.IsFixed:
                    return CSVBool((dto as IFixableDto).IsFixed);
                case BaseHeaderName.FixedDate:
                    return CSVDate((dto as IFixableDto).FixedDate);
                case BaseHeaderName.IsArchived:
                    return CSVBool((dto as IArchivableDto).IsArchived);
                case BaseHeaderName.ArchivedDate:
                    return CSVDate((dto as IArchivableDto).ArchivedDate);
                default:
                    throw new FrontUserException("Unknow header " + headerName);
            }
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct BaseHeaderName
        {
            /// <summary>
            /// Header name for id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// Header name for is fixed.
            /// </summary>
            public const string IsFixed = "isFixed";

            /// <summary>
            /// Header name for fixed date.
            /// </summary>
            public const string FixedDate = "fixedDate";

            /// <summary>
            /// Header name for is archived.
            /// </summary>
            public const string IsArchived = "isArchived";

            /// <summary>
            /// Header name for archived date.
            /// </summary>
            public const string ArchivedDate = "archivedDate";
        }
    }
}