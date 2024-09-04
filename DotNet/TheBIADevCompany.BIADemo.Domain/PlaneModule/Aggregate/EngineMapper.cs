// BIADemo only
// <copyright file="EngineMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for engine.
    /// </summary>
    public class EngineMapper : BaseMapper<EngineDto, Engine, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Engine> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<Engine>
                {
                    { HeaderName.Id, engine => engine.Id },
                    { HeaderName.Reference, engine => engine.Reference },
                    { HeaderName.LastMaintenanceDate, engine => engine.LastMaintenanceDate },
                    { HeaderName.SyncTime, engine => engine.SyncTime },
                    { HeaderName.Power, engine => engine.Power },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(EngineDto dto, Engine entity)
        {
            if (entity == null)
            {
                entity = new Engine();
            }

            entity.Id = dto.Id;
            entity.Reference = dto.Reference;
            entity.LastMaintenanceDate = dto.LastMaintenanceDate;
            entity.SyncTime = TimeSpan.Parse(dto.SyncTime, new CultureInfo("en-US"));
            entity.Power = dto.Power;

            // Mapping relationship 1-* : Site
            if (dto.PlaneId != 0)
            {
                entity.PlaneId = dto.PlaneId;
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Engine, EngineDto>> EntityToDto()
        {
            return entity => new EngineDto
            {
                Id = entity.Id,
                Reference = entity.Reference,
                LastMaintenanceDate = entity.LastMaintenanceDate,
                SyncTime = entity.SyncTime.ToString(@"hh\:mm\:ss"),
                Power = entity.Power,

                // Mapping relationship 1-* : Plane
                PlaneId = entity.PlaneId,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<EngineDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Reference, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Reference));
                        }

                        if (string.Equals(headerName, HeaderName.LastMaintenanceDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.LastMaintenanceDate));
                        }

                        if (string.Equals(headerName, HeaderName.SyncTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.SyncTime));
                        }

                        if (string.Equals(headerName, HeaderName.Power, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Power.Value));
                        }
                    }
                }

                return records.ToArray();
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Engine entity, EngineDto dto)
        {
            dto.Id = entity.Id;
            dto.PlaneId = entity.PlaneId;
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header Name Id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// Header Name Reference.
            /// </summary>
            public const string Reference = "reference";

            /// <summary>
            /// Header Name LastMaintenanceDate.
            /// </summary>
            public const string LastMaintenanceDate = "lastMaintenanceDate";

            /// <summary>
            /// Header Name SyncTime.
            /// </summary>
            public const string SyncTime = "syncTime";

            /// <summary>
            /// Header Name Power.
            /// </summary>
            public const string Power = "power";
        }
    }
}