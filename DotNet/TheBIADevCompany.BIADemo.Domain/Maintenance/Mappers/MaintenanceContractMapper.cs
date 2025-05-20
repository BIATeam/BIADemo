// BIADemo only
// <copyright file="MaintenanceContractMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The mapper used for MaintenanceContract.
    /// </summary>
    public class MaintenanceContractMapper : BaseMapper<MaintenanceContractDto, MaintenanceContract, int>
    {
        /// <inheritdoc/>
        public override ExpressionCollection<MaintenanceContract> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<MaintenanceContract>(base.ExpressionCollection)
                {
                    { HeaderName.AircraftMaintenanceCompany, x => x.AircraftMaintenanceCompany != null ? x.AircraftMaintenanceCompany.Title : null },
                    { HeaderName.ContractNumber, x => x.ContractNumber },
                    { HeaderName.Description, x => x.Description },
                    { HeaderName.Planes, x => x.Planes.Select(y => y.Msn).OrderBy(y => y) },
                    { HeaderName.Site, x => x.Site != null ? x.Site.Title : null },
                };
            }
        }

        /// <inheritdoc/>
        public override void DtoToEntity(MaintenanceContractDto dto, ref MaintenanceContract entity)
        {
            base.DtoToEntity(dto, ref entity);

            entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompany?.Id;
            entity.ContractNumber = dto.ContractNumber;
            entity.Description = dto.Description;
            if (dto.Planes != null && dto.Planes.Count != 0)
            {
                foreach (var optionDto in dto.Planes.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var entityToRemove = entity.Planes.FirstOrDefault(x => x.Id == optionDto.Id);
                    if (entityToRemove != null)
                    {
                        entity.Planes.Remove(entityToRemove);
                    }
                }

                entity.MaintenanceContractPlanes = entity.MaintenanceContractPlanes ?? new List<MaintenanceContractPlane>();
                foreach (var optionDto in dto.Planes.Where(x => x.DtoState == DtoState.Added))
                {
                    entity.MaintenanceContractPlanes.Add(new MaintenanceContractPlane
                    {
                        MaintenanceContractId = dto.Id,
                        PlaneId = optionDto.Id,
                    });
                }
            }

            entity.SiteId = dto.Site?.Id;
        }

        /// <inheritdoc/>
        public override Expression<Func<MaintenanceContract, MaintenanceContractDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new MaintenanceContractDto
            {
                AircraftMaintenanceCompany = entity.AircraftMaintenanceCompany != null ?
                  new OptionDto { Id = entity.AircraftMaintenanceCompany.Id, Display = entity.AircraftMaintenanceCompany.Title } :
                  null,
                ContractNumber = entity.ContractNumber,
                Description = entity.Description,
                Planes = entity.Planes
                .Select(x => new OptionDto { Id = x.Id, Display = x.Msn })
                .OrderBy(x => x.Display)
                .ToList(),
                Site = entity.Site != null ?
                  new OptionDto { Id = entity.Site.Id, Display = entity.Site.Title } :
                  null,
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCell"/>
        public override string DtoToCell(MaintenanceContractDto dto, string headerName)
        {
            if (string.Equals(headerName, HeaderName.ContractNumber, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.ContractNumber);
            }

            if (string.Equals(headerName, HeaderName.Description, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.Description);
            }

            return base.DtoToCell(dto, headerName);
        }

        /// <summary>
        /// Header Names.
        /// </summary>
        private struct HeaderName
        {
            /// <summary>
            /// Header Name AircraftMaintenanceCompany.
            /// </summary>
            public const string AircraftMaintenanceCompany = "aircraftMaintenanceCompany";

            /// <summary>
            /// Header Name ContractNumber.
            /// </summary>
            public const string ContractNumber = "contractNumber";

            /// <summary>
            /// Header Name Description.
            /// </summary>
            public const string Description = "description";

            /// <summary>
            /// Header Name Planes.
            /// </summary>
            public const string Planes = "planes";

            /// <summary>
            /// Header Name Site.
            /// </summary>
            public const string Site = "site";
        }
    }
}