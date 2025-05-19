// BIADemo only
// <copyright file="MaintenanceContractMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

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
                return new ExpressionCollection<MaintenanceContract>
                {
                    { HeaderName.AircraftMaintenanceCompany, x => x.AircraftMaintenanceCompany != null ? x.AircraftMaintenanceCompany.Title : null },
                    { HeaderName.ArchivedDate, x => x.ArchivedDate },
                    { HeaderName.ContractNumber, x => x.ContractNumber },
                    { HeaderName.Description, x => x.Description },
                    { HeaderName.FixedDate, x => x.FixedDate },
                    { HeaderName.Id, x => x.Id },
                    { HeaderName.IsArchived, x => x.IsArchived },
                    { HeaderName.Planes, x => x.Planes.Select(y => y.Msn).OrderBy(y => y) },
                    { HeaderName.Site, x => x.Site != null ? x.Site.Title : null },
                };
            }
        }

        /// <inheritdoc/>
        public override void DtoToEntity(MaintenanceContractDto dto, ref MaintenanceContract entity)
        {
            entity ??= new MaintenanceContract();

            entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompany?.Id;
            entity.ArchivedDate = dto.ArchivedDate;
            entity.ContractNumber = dto.ContractNumber;
            entity.Description = dto.Description;
            entity.FixedDate = dto.FixedDate;
            entity.Id = dto.Id;
            entity.IsArchived = dto.IsArchived;
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
            return entity => new MaintenanceContractDto
            {
                AircraftMaintenanceCompany = entity.AircraftMaintenanceCompany != null ?
                  new OptionDto { Id = entity.AircraftMaintenanceCompany.Id, Display = entity.AircraftMaintenanceCompany.Title } :
                  null,
                ArchivedDate = entity.ArchivedDate,
                ContractNumber = entity.ContractNumber,
                Description = entity.Description,
                FixedDate = entity.FixedDate,
                Id = entity.Id,
                IsArchived = entity.IsArchived,
                Planes = entity.Planes
                .Select(x => new OptionDto { Id = x.Id, Display = x.Msn })
                .OrderBy(x => x.Display)
                .ToList(),
                Site = entity.Site != null ?
                  new OptionDto { Id = entity.Site.Id, Display = entity.Site.Title } :
                  null,
            };
        }

        /// <inheritdoc/>
        public override Func<MaintenanceContractDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVDateTime(x.ArchivedDate),
                CSVString(x.ContractNumber),
                CSVString(x.Description),
                CSVDateTime(x.FixedDate),
                CSVNumber(x.Id),
                CSVBool(x.IsArchived),
            });
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
            /// Header Name ArchivedDate.
            /// </summary>
            public const string ArchivedDate = "archivedDate";

            /// <summary>
            /// Header Name ContractNumber.
            /// </summary>
            public const string ContractNumber = "contractNumber";

            /// <summary>
            /// Header Name Description.
            /// </summary>
            public const string Description = "description";

            /// <summary>
            /// Header Name FixedDate.
            /// </summary>
            public const string FixedDate = "fixedDate";

            /// <summary>
            /// Header Name Id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// Header Name IsArchived.
            /// </summary>
            public const string IsArchived = "isArchived";

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