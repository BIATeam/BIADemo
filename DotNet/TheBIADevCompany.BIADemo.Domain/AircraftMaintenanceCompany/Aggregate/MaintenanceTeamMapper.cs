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
    using System.Security.Principal;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamMapper : BaseMapper<MaintenanceTeamDto, MaintenanceTeam, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public MaintenanceTeamMapper(IPrincipal principal)
        {
            this.UserRoleIds = (principal as BiaClaimsPrincipal).GetRoleIds();
            this.UserId = (principal as BiaClaimsPrincipal).GetUserId();
        }

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

        /// <summary>
        /// the user id.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// the user roles.
        /// </summary>
        private IEnumerable<int> UserRoleIds { get; set; }

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
            entity.Code = dto.Code;
            entity.IsActive = dto.IsActive;

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
                Code = entity.Code,
                IsActive = entity.IsActive,

                // Mapping relationship 1-* : AircraftMaintenanceCompany
                AircraftMaintenanceCompanyId = entity.AircraftMaintenanceCompanyId,

                // Should correspond to MaintenanceTeam_Update permission (but without use the roles *_Member that is not determined at list display)
                CanUpdate =
                    /* Begin Parent AircraftMaintenanceCompany */
                    this.UserRoleIds.Contains((int)RoleId.Supervisor) ||
                    /* End Parent AircraftMaintenanceCompany */
                    this.UserRoleIds.Contains((int)RoleId.Admin),

                // Should correspond to MaintenanceTeam_Member_List_Access (but without use the roles *_Member that is not determined at list display)
                CanMemberListAccess =
                    this.UserRoleIds.Contains((int)RoleId.Admin) ||
                    /* Begin Parent AircraftMaintenanceCompany */
                    entity.AircraftMaintenanceCompany.Members.Any(m => m.UserId == this.UserId) ||
                    /* End Parent AircraftMaintenanceCompany */
                    entity.Members.Any(m => m.UserId == this.UserId),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<MaintenanceTeamDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVString(x.Title),
                CSVString(x.Code),
                CSVBool(x.IsActive),
            });
        }
    }
}