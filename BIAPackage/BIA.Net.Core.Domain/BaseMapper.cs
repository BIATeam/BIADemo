// <copyright file="BaseMapper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseMapper<TDto, TEntity, TKey> : BaseEntityMapper<TEntity>
        where TDto : BaseDto<TKey>
        where TEntity : class, IEntity<TKey>
    {

        public UserContext UserContext { get; set; }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        /// <param name="context">The context.</param>
        public virtual void DtoToEntity(TDto dto, TEntity entity, string mapperMode, IUnitOfWork context)
        {
            DtoToEntity(dto, entity, mapperMode);
        }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual void DtoToEntity(TDto dto, TEntity entity, string mapperMode)
        {
            DtoToEntity(dto, entity);
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
            return EntityToDto();
        }

        /// <summary>
        /// Create a DTO from an entity.
        /// </summary>
        /// <returns>The created DTO.</returns>
        public virtual Expression<Func<TEntity, TDto>> EntityToDto()
        {
            throw new NotImplementedException("This mapper is not build to create dto, or the implementation of EntityToDto is missing.");
            //return null;
        }

        /// <summary>
        /// Map the entity essential keys after add, update and before delete to the dto return by the modifier function.  
        /// </summary>
        /// <param name="entity">The entity to update with the DTO values.</param>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="mapperMode">The mode of mapping.</param>
        public virtual void MapEntityKeysInDto(TEntity entity, TDto dto, string mapperMode)
        {
            MapEntityKeysInDto(entity, dto);
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
            return IncludesBeforeDelete();
        }

        /// <summary>
        /// Defining the includes to use in the key mapping before deleting related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesBeforeDelete()
        {
            return null;
        }

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesForUpdate(string mapperMode)
        {
            return IncludesForUpdate();
        }

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesForUpdate()
        {
            return null;
        }

        /// <summary>
        /// Create a record from a DTO.
        /// </summary>
        /// <param name="mapperMode">The mode of mapping.</param>
        /// <param name="headerNames">The list of header names.</param>
        /// <returns>Func.</returns>
        public virtual Func<TDto, object[]> DtoToRecord(string mapperMode, List<string> headerNames = null)
        {
            return DtoToRecord(headerNames);
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

        public static string CSVString(string x)
        {
            return "\"=\"\"" + x?.Replace("\"", "\"\"\"\"") + "\"\"\"";
        }
        public static string CSVList(ICollection<OptionDto> x)
        {
            return CSVString(string.Join(" - ", x?.Select(ca => ca.Display).ToList()));
        }
        
        public static string CSVDate(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd");
        }
        public static string CSVTime(DateTime? x)
        {
            return x?.ToString("hh:mm");
        }
        public static string CSVTime(TimeSpan? x)
        {
            return x?.ToString("hh:mm");
        }
        public static string CSVTime(string x)
        {
            return x;
        }
        public static string CSVDateTime(DateTime? x)
        {
            return x?.ToString("yyyy-MM-dd hh:mm");
        }
        public static string CSVBool(bool x)
        {
            return x ? "X" : string.Empty;
        }
        public static string CSVNumber(int x)
        {
            return x.ToString();
        }
    }
}