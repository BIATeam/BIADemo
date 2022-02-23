// BIADemo only
// <copyright file="AircraftMaintenanceCompanyMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyMapper : BaseMapper<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<AircraftMaintenanceCompany> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<AircraftMaintenanceCompany>
                {
                    { "Id", aircraftMaintenanceCompany => aircraftMaintenanceCompany.Id },
                    { "Name", aircraftMaintenanceCompany1 => aircraftMaintenanceCompany1.Name },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(AircraftMaintenanceCompanyDto dto, AircraftMaintenanceCompany entity)
        {
            if (entity == null)
            {
                entity = new AircraftMaintenanceCompany();
            }

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<AircraftMaintenanceCompany, AircraftMaintenanceCompanyDto>> EntityToDto()
        {
            return entity => new AircraftMaintenanceCompanyDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<AircraftMaintenanceCompanyDto, object[]> DtoToRecord()
        {
            return x => (new object[]
            {
                CSVString(x.Name),
            });
        }

        ///// <inheritdoc/>
        //public override void MapEntityKeysInDto(AircraftMaintenanceCompany entity, AircraftMaintenanceCompanyDto dto)
        //{
        //    dto.Id = entity.Id;
        //}
    }
}