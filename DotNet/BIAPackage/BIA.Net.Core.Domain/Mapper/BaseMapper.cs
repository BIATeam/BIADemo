// <copyright file="BaseMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity.Interface;
    using Microsoft.VisualBasic;

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
        /// The dto is versionned.
        /// </summary>
        private readonly bool isVersionned;

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
            if (typeof(IEntityFixable).IsAssignableFrom(typeof(TEntity)) && typeof(IDtoFixable).IsAssignableFrom(typeof(TDto)))
            {
                this.isFixable = true;
            }

            if (typeof(IEntityArchivable).IsAssignableFrom(typeof(TEntity)) && typeof(IDtoArchivable).IsAssignableFrom(typeof(TDto)))
            {
                this.isArchivable = true;
            }

            if (typeof(IEntityVersioned).IsAssignableFrom(typeof(TEntity)) && typeof(IDtoVersioned).IsAssignableFrom(typeof(TDto)))
            {
                this.isVersionned = true;
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
                    expression.Add(BaseHeaderName.IsFixed, entity => (entity as IEntityFixable).IsFixed);
                    expression.Add(BaseHeaderName.FixedDate, entity => (entity as IEntityFixable).FixedDate);
                }

                if (this.isArchivable)
                {
                    expression.Add(BaseHeaderName.IsArchived, entity => (entity as IEntityArchivable).IsArchived);
                    expression.Add(BaseHeaderName.ArchivedDate, entity => (entity as IEntityArchivable).ArchivedDate);
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
                        entity = default;
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

            if (this.isVersionned)
            {
                // RowVersion = Convert.ToBase64String(entity.RowVersion),
                var rowVersionProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityVersioned)),
                    nameof(IEntityVersioned.RowVersion));

                var toBase64StringMethod = typeof(Convert).GetMethod(nameof(Convert.ToBase64String), new[] { typeof(byte[]) });

                var rowVersion = Expression.Call(toBase64StringMethod, rowVersionProperty);

                var bindRowVersion = Expression.Bind(typeof(TDto).GetProperty(nameof(IDtoVersioned.RowVersion)), rowVersion);

                bindings.Add(bindRowVersion);
            }

            if (this.isFixable)
            {
                // IsFixed = (entity as IEntityFixable).IsFixed,
                // FixedDate = (entity as IEntityFixable).FixedDate,
                var isFixedProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityFixable)),
                    nameof(IEntityFixable.IsFixed));

                var fixedDateProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityFixable)),
                    nameof(IEntityFixable.FixedDate));

                var bindIsFixed = Expression.Bind(typeof(TDto).GetProperty(nameof(IDtoFixable.IsFixed)), isFixedProperty);
                var bindFixedDate = Expression.Bind(typeof(TDto).GetProperty(nameof(IDtoFixable.FixedDate)), fixedDateProperty);
                bindings.Add(bindIsFixed);
                bindings.Add(bindFixedDate);
            }

            if (this.isArchivable)
            {
                // IsArchived = (entity as IEntityArchivable).IsArchived,
                // ArchivedDate = (entity as IEntityArchivable).ArchivedDate,
                var isArchivedProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityArchivable)),
                    nameof(IEntityArchivable.IsArchived));

                var archivedDateProperty = Expression.Property(
                    Expression.Convert(entityParam, typeof(IEntityArchivable)),
                    nameof(IEntityArchivable.ArchivedDate));

                var bindIsArchived = Expression.Bind(typeof(TDto).GetProperty(nameof(IDtoArchivable.IsArchived)), isArchivedProperty);
                var bindArchivedDate = Expression.Bind(typeof(TDto).GetProperty(nameof(IDtoArchivable.ArchivedDate)), archivedDateProperty);
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
            if (this.isVersionned)
            {
                (dto as IDtoVersioned).RowVersion = Convert.ToBase64String((entity as IEntityVersioned).RowVersion);
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
            return dto =>
            {
                List<object> records = [];
                Dictionary<string, Func<string>> headerActions = this.DtoToCellMapping(dto);

                if (headerNames != null && headerNames.Count > 0)
                {
                    foreach (string headerName in headerNames)
                    {
                        records.Add(this.DtoToCell(dto, headerName, headerActions));
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
        /// <param name="headerActions">List of exprestion to translate dto in csv cell.</param>
        /// <returns>A string formatted for CSV.</returns>
        public virtual string DtoToCell(TDto dto, string headerName, Dictionary<string, Func<string>> headerActions)
        {
            if (headerActions.TryGetValue(headerName, out var action))
            {
                return action();
            }

            throw new FrontUserException("Unknown header " + headerName);
        }

        /// <summary>
        /// List of exprestion to translate dto in csv cell.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The list of expression.</returns>
        public virtual Dictionary<string, Func<string>> DtoToCellMapping(TDto dto)
        {
            return new Dictionary<string, Func<string>>
            {
                { BaseHeaderName.Id, () => CSVCell(dto.Id) },
                { BaseHeaderName.IsFixed, () => CSVBool((dto as IDtoFixable).IsFixed) },
                { BaseHeaderName.FixedDate, () => CSVDate((dto as IDtoFixable).FixedDate) },
                { BaseHeaderName.IsArchived, () => CSVBool((dto as IDtoArchivable).IsArchived) },
                { BaseHeaderName.ArchivedDate, () => CSVDate((dto as IDtoArchivable).ArchivedDate) },
            };
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