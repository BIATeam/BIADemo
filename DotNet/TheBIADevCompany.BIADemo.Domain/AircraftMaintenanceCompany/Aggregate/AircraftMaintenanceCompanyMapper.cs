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
    using System.Security.Principal;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyMapper : BaseMapper<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int>
    {
        public AircraftMaintenanceCompanyMapper(IPrincipal principal)
        {
            this.UserRoleIds = (principal as BIAClaimsPrincipal).GetRoleIds();
            this.UserId = (principal as BIAClaimsPrincipal).GetUserId();
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<AircraftMaintenanceCompany> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<AircraftMaintenanceCompany>
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
        public override void DtoToEntity(AircraftMaintenanceCompanyDto dto, AircraftMaintenanceCompany entity)
        {
            if (entity == null)
            {
                entity = new AircraftMaintenanceCompany();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.TeamTypeId = (int)TeamTypeId.AircraftMaintenanceCompany;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<AircraftMaintenanceCompany, AircraftMaintenanceCompanyDto>> EntityToDto()
        {
            return entity => new AircraftMaintenanceCompanyDto
            {
                Id = entity.Id,
                Title = entity.Title,

                // Should correspond to AircraftMaintenanceCompany_Update permission (but without use the roles *_Member that is not determined at list display)
                CanUpdate = this.UserRoleIds.Contains((int)RoleId.Admin),

                // Should correspond to AircraftMaintenanceCompany_Member_List_Access (but without use the roles *_Member that is not determined at list display)
                CanMemberListAccess = this.UserRoleIds.Contains((int)RoleId.Admin) || entity.Members.Any(m => m.UserId == this.UserId),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<AircraftMaintenanceCompanyDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVString(x.Title),
            });
        }
    }
}