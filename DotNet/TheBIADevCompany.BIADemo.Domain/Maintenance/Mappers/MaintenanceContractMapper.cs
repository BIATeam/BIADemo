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
    using BIA.Net.Core.Domain.Mapper;
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
                    { HeaderName.ContractNumber, x => x.ContractNumber },
                    { HeaderName.Description, x => x.Description },
                    { HeaderName.Planes, x => x.Planes.Select(y => y.Msn).OrderBy(y => y) },
                };
            }
        }

        /// <inheritdoc />
        public override ExpressionCollection<MaintenanceContract> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<MaintenanceContract>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<MaintenanceContract>()
                    {
                        { HeaderName.Planes, x => x.Planes.Select(y => y.Id) },
                    });
            }
        }

        /// <inheritdoc/>
        public override void DtoToEntity(MaintenanceContractDto dto, ref MaintenanceContract entity)
        {
            var isCreation = entity == null;
            base.DtoToEntity(dto, ref entity);

            // Map parent relationship 1-* : AircraftMaintenanceCompanyId & SiteId
            if (isCreation && dto.AircraftMaintenanceCompanyId != 0 && dto.SiteId != 0)
            {
                entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompanyId;
                entity.SiteId = dto.SiteId;
            }

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
        }

        /// <inheritdoc/>
        public override Expression<Func<MaintenanceContract, MaintenanceContractDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new MaintenanceContractDto
            {
                AircraftMaintenanceCompanyId = entity.AircraftMaintenanceCompanyId,
                ContractNumber = entity.ContractNumber,
                Description = entity.Description,
                Planes = entity.Planes
                .Select(x => new OptionDto { Id = x.Id, Display = x.Msn })
                .OrderBy(x => x.Display)
                .ToList(),
                SiteId = entity.SiteId,
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(MaintenanceContractDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.ContractNumber, () => CSVString(dto.ContractNumber) },
                { HeaderName.Description, () => CSVString(dto.Description) },
            };
        }

        /// <summary>
        /// Header Names.
        /// </summary>
        private struct HeaderName
        {
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
        }
    }
}