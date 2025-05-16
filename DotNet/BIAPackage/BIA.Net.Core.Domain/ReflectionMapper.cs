// <copyright file="ReflectionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Reflection;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Base;
    using Microsoft.Identity.Client;

    /// <summary>
    /// The class used to define the base mapper with reflection to map some field.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public abstract class ReflectionMapper<TDto, TEntity, TKey> : BaseMapper<TDto, TEntity, TKey>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// The is fixable.
        /// </summary>
        private readonly bool isFixable;

        /// <summary>
        /// The is archivable.
        /// </summary>
        private readonly bool isArchivable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionMapper{TDto, TEntity, TKey}"/> class.
        /// </summary>
        protected ReflectionMapper()
            : base()
        {
            if (typeof(IEntityFixable<TKey>).IsAssignableFrom(typeof(TEntity)))
            {
                this.isFixable = true;
                Debug.Assert(
                    typeof(IFixableDto).IsAssignableFrom(typeof(TDto)),
                    "The dto " + typeof(TDto).ToString() + " should implement of IFixableDto");
            }

            if (typeof(IEntityArchivable<TKey>).IsAssignableFrom(typeof(TEntity)))
            {
                this.isArchivable = true;
                Debug.Assert(
                    typeof(IArchivableDto).IsAssignableFrom(typeof(TDto)),
                    "The dto " + typeof(TDto).ToString() + " should implement of IArchivableDto");
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<TEntity> ExpressionCollection
        {
            get
            {
                var expression = new ExpressionCollection<TEntity>
                {
                    { ReflectionHeaderName.Id, entity => entity.Id },
                };

                if (this.isFixable)
                {
                    expression.Add(ReflectionHeaderName.IsFixed, entity => (entity as IEntityFixable<TKey>).IsFixed);
                    expression.Add(ReflectionHeaderName.FixedDate, entity => (entity as IEntityFixable<TKey>).FixedDate);
                }

                if (this.isArchivable)
                {
                    expression.Add(ReflectionHeaderName.IsArchived, entity => (entity as IEntityArchivable<TKey>).IsArchived);
                    expression.Add(ReflectionHeaderName.ArchivedDate, entity => (entity as IEntityArchivable<TKey>).ArchivedDate);
                }

                return expression;
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(TDto dto, TEntity entity)
        {
            if (entity == null)
            {
                entity = new TEntity
                {
                    Id = dto.Id,
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<TEntity, TDto>> EntityToDto()
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

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<TDto, object[]> DtoToRecord(List<string> headerNames = null)
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
            if (string.Equals(headerName, ReflectionHeaderName.Id, StringComparison.OrdinalIgnoreCase))
            {
                return CSVCell(dto.Id);
            }

            if (string.Equals(headerName, ReflectionHeaderName.IsFixed, StringComparison.OrdinalIgnoreCase))
            {
                return CSVBool((dto as IFixableDto).IsFixed);
            }

            if (string.Equals(headerName, ReflectionHeaderName.FixedDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDate((dto as IFixableDto).FixedDate);
            }

            if (string.Equals(headerName, ReflectionHeaderName.IsArchived, StringComparison.OrdinalIgnoreCase))
            {
                return CSVBool((dto as IArchivableDto).IsArchived);
            }

            if (string.Equals(headerName, ReflectionHeaderName.ArchivedDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDate((dto as IArchivableDto).ArchivedDate);
            }

            return "Unknow header " + headerName;
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct ReflectionHeaderName
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