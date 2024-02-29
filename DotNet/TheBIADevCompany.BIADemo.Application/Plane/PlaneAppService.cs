// BIADemo only
// <copyright file="PlaneAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceBase<PlaneDto, Plane, int, PagingFilterFormatDto, PlaneMapper>, IPlaneAppService
    {
        /// <summary>
        /// The current SiteId.
        /// </summary>
        private readonly int currentSiteId;

        private readonly IPlaneAirportRepository planeAirportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="planeAirportRepository">The PlaneAirportRepository.</param>
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(ITGenericRepository<Plane, int> repository, IPlaneAirportRepository planeAirportRepository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentSiteId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentSiteId));
            this.planeAirportRepository = planeAirportRepository;
        }

        /// <inheritdoc/>
        public override async Task<PlaneDto> AddAsync(
            PlaneDto dto,
            string mapperMode = null)
        {
            if (dto != null)
            {
                PlaneMapper mapper = this.InitMapper<PlaneDto, PlaneMapper>();
                Plane plane = new();
                mapper.DtoToEntity(dto, plane, mapperMode, this.Repository.UnitOfWork);
                this.Repository.Add(plane);
                this.UpdatePlaneAirportRelationShip(dto, plane);
                await this.Repository.UnitOfWork.CommitAsync();
                mapper.MapEntityKeysInDto(plane, dto);
            }

            return dto;
        }

        /// <inheritdoc/>
        public override async Task<PlaneDto> UpdateAsync(
            PlaneDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            if (dto != null)
            {
                PlaneMapper mapper = this.InitMapper<PlaneDto, PlaneMapper>();

                Plane plane = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode);
                if (plane == null)
                {
                    throw new ElementNotFoundException();
                }

                mapper.DtoToEntity(dto, plane, mapperMode, this.Repository.UnitOfWork);
                this.UpdatePlaneAirportRelationShip(dto, plane);

                await this.Repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
                mapper.MapEntityKeysInDto(plane, dto);
            }

            return dto;
        }

        private void UpdatePlaneAirportRelationShip(PlaneDto dto, Plane plane)
        {
            // Mapping relationship *-* : ICollection<OptionDto> ConnectingAirports
            if (dto.ConnectingAirports != null && dto.ConnectingAirports?.Any() == true)
            {
                foreach (var airportDto in dto.ConnectingAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    this.planeAirportRepository.Remove(new PlaneAirport { AirportId = airportDto.Id, Plane = plane });
                }

                foreach (var airportDto in dto.ConnectingAirports.Where(w => w.DtoState == DtoState.Added))
                {
                    this.planeAirportRepository.Add(new PlaneAirport { AirportId = airportDto.Id, Plane = plane });
                }
            }
        }
    }
}