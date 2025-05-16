// <copyright file="ReflectionMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>


namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Reflection;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Base;

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
        bool IsFixable;

        /// <summary>
        /// The is archivable.
        /// </summary>
        bool IsArchivable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionMapper{TDto, TEntity, TKey}"/> class.
        /// </summary>
        protected ReflectionMapper()
            : base()
        {
            Assembly asm = Assembly.GetAssembly(this.GetType());
            foreach (Type type in asm.GetTypes())
            {
                if (typeof(IEntityFixable<TKey>).IsAssignableFrom(type))
                {
                    this.IsFixable = true;
                }

                if (typeof(IEntityArchivable<TKey>).IsAssignableFrom(type))
                {
                    this.IsArchivable = true;
                }
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

                if (this.IsFixable)
                {
                    expression.Add(ReflectionHeaderName.IsFixed, entity => (entity as IEntityFixable<TKey>).IsFixed);
                    expression.Add(ReflectionHeaderName.FixedDate, entity => (entity as IEntityFixable<TKey>).FixedDate);
                }

                if (this.IsArchivable)
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
            if (this.IsFixable) {
                return entity => new TDto
                {
                    Id = entity.Id,
                    IsFixed = (entity as IEntityFixable<TKey>).IsFixed,
                };
            }

            return entity => new TDto
            {
                Id = entity.Id,
            };
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
        /// <returns>a string formated for csv/returns>
        protected virtual string DtoToCell(TDto dto, string headerName)
        {
            if (string.Equals(headerName, ReflectionHeaderName.Id, StringComparison.OrdinalIgnoreCase))
            {
                return CSVCell(dto.Id);
            }

            if (string.Equals(headerName, ReflectionHeaderName.IsFixed, StringComparison.OrdinalIgnoreCase))
            {
                return CSVBool(dto.IsFixed);
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