// BIADemo only
// <copyright file="MaintenanceTeamMapper.cs" company="TheBIADevCompany">
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
    public class MaintenanceTeamMapper : BaseMapper<MaintenanceTeamDto, MaintenanceTeam, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<MaintenanceTeam> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<MaintenanceTeam>
                {
                    { "Id", aircraftMaintenanceCompany => aircraftMaintenanceCompany.Id },
                    { "Title", aircraftMaintenanceCompany => aircraftMaintenanceCompany.Title },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(MaintenanceTeamDto dto, MaintenanceTeam entity)
        {
            if (entity == null)
            {
                entity = new MaintenanceTeam();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.TeamTypeId = (int)TeamTypeId.MaintenanceTeam;

            // Mapping relationship 1-* : AircraftMaintenanceCompany
            if (dto.AircraftMaintenanceCompanyId != 0)
            {
                entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompanyId;
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<MaintenanceTeam, MaintenanceTeamDto>> EntityToDto()
        {
            return entity => new MaintenanceTeamDto
            {
                Id = entity.Id,
                Title = entity.Title,

                // Mapping relationship 1-* : AircraftMaintenanceCompany
                AircraftMaintenanceCompanyId = entity.AircraftMaintenanceCompanyId,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<MaintenanceTeamDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVString(x.Title),
            });
        }
    }
}