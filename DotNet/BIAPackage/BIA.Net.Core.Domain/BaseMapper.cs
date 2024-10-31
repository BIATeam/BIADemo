// <copyright file="BaseMapper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
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
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// CSVs the string.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a string cell.</returns>
        public static string CSVString(string x)
        {
            return "\"=\"\"" + x?.Replace("\"", "\"\"\"\"") + "\"\"\"";
        }

        /// <summary>
        /// CSVs the list.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a list cell.</returns>
        public static string CSVList(ICollection<OptionDto> x)
        {
            return CSVString(string.Join(" - ", x?.Select(ca => ca.Display).ToList()));
        }

        /// <summary>
        /// CSVs the date.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date cell.</returns>
        public static string CSVDate(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// CSVs the date.
        /// </summary>
        /// <param name="x">The DateOnly.</param>
        /// <returns>A string for a date cell.</returns>
        public static string CSVDate(DateOnly? x)
        {
            return x?.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// CSVs the date time.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date and time cell.</returns>
        public static string CSVDateTime(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// CSVs the date time with seconds.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a date and time with seconds cell.</returns>
        public static string CSVDateTimeSeconds(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(DateTime? x)
        {
            return x?.ToString("HH:mm");
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The TimeSpan.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(TimeSpan? x)
        {
            return x?.ToString("HH:mm");
        }

        /// <summary>
        /// CSVs the time.
        /// </summary>
        /// <param name="x">The TimeOnly.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTime(TimeOnly? x)
        {
            return x?.ToString("HH:mm");
        }

        /// <summary>
        /// CSVs the time with seconds.
        /// </summary>
        /// <param name="x">The DateTime.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTimeSeconds(DateTime? x)
        {
            return x?.ToString("HH:mm:ss");
        }

        /// <summary>
        /// CSVs the time with seconds.
        /// </summary>
        /// <param name="x">The TimeSpan.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTimeSeconds(TimeSpan? x)
        {
            return x?.ToString("HH:mm:ss");
        }

        /// <summary>
        /// CSVs the time with seconds.
        /// </summary>
        /// <param name="x">The TimeOnly.</param>
        /// <returns>A string for a time cell.</returns>
        public static string CSVTimeSeconds(TimeOnly? x)
        {
            return x?.ToString("HH:mm:ss");
        }

        /// <summary>
        /// CSVs the bool.
        /// </summary>
        /// <param name="x">if set to <c>true</c> [x].</param>
        /// <returns>A string for a bool cell.</returns>
        public static string CSVBool(bool x)
        {
            return x ? "X" : string.Empty;
        }

        /// <summary>
        /// CSVs the number.
        /// </summary>
        /// <typeparam name="T">The type of number.</typeparam>
        /// <param name="x">The number.</param>
        /// <returns>A string for a number cell.</returns>
        public static string CSVNumber<T>(T? x)
            where T : struct, IFormattable
        {
            return x.HasValue ? x.Value.ToString(null, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// CSVs the number.
        /// </summary>
        /// <typeparam name="T">The type of number.</typeparam>
        /// <param name="x">The number.</param>
        /// <returns>A string for a number cell.</returns>
        public static string CSVNumber<T>(T x)
            where T : IFormattable
        {
            return x.ToString(null, CultureInfo.InvariantCulture);
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
                        entity = new TEmbeddedEntity();
                        mapper.DtoToEntity(dto, entity);
                        entityCollection.Add(entity);
                        break;
                    case DtoState.Modified:
                        entity = entityCollection.FirstOrDefault(e => e.Id == dto.Id);
                        mapper.DtoToEntity(dto, entity);
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
        public virtual void DtoToEntity(TDto dto, TEntity entity, string mapperMode, IUnitOfWork context)
        {
            this.DtoToEntity(dto, entity, mapperMode);
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual void DtoToEntity(TDto dto, TEntity entity, string mapperMode)
        {
            this.DtoToEntity(dto, entity);
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        public virtual void DtoToEntity(TDto dto, TEntity entity)
        {
            throw new NotImplementedException("This mapper is not build to manipulate entity, or the implementation of DtoToEntity is missing.");
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
            throw new NotImplementedException("This mapper is not build to create dto, or the implementation of EntityToDto is missing.");
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
            throw new NotImplementedException("This mapper is not build to generate records, or the implementation of DtoToRecord is missing.");
        }
    }
}