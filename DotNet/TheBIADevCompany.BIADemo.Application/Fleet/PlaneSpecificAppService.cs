// BIADemo only
// <copyright file="PlaneSpecificAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Fleet.QueryModels;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneSpecificAppService :
        CrudAppServiceListAndItemBase<PlaneSpecificDto, PlaneDto, Plane, int, PagingFilterFormatDto, PlaneSpecificMapper, PlaneMapper>,
        IPlaneSpecificAppService
    {
        /// <summary>
        /// The current SiteId.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// The plane query model mapper.
        /// </summary>
        private readonly PlaneSpecificQueryModelMapper planeSpecificQueryModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneSpecificAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="planeSpecificQueryModelMapper">The plane query model mapper.</param>
        /// <param name="planeSpecificQueryCustomizer">The plane specific query customizer.</param>
        public PlaneSpecificAppService(ITGenericRepository<Plane, int> repository, IPrincipal principal, PlaneSpecificQueryModelMapper planeSpecificQueryModelMapper, IPlaneSpecificQueryCustomizer planeSpecificQueryCustomizer)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentSiteId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentSiteId));

            // Query model mapper and customizer
            this.planeSpecificQueryModelMapper = planeSpecificQueryModelMapper;
            this.Repository.QueryCustomizer = planeSpecificQueryCustomizer;
        }

        /// <inheritdoc/>
#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public override async Task<(IEnumerable<PlaneDto> Results, int Total)> GetRangeAsync(PagingFilterFormatDto filters = null, int id = 0, Specification<Plane> specification = null, Expression<Func<Plane, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false)
        {
            var (results, total) = await this.GetRangeGenericAsync<PlaneSpecificQueryModel, PlaneSpecificQueryModelMapper, PagingFilterFormatDto>(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
            return (this.planeSpecificQueryModelMapper.QueryModelsToDtoListItems(results), total);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<PlaneDto>> GetAllAsync(int id = 0, Specification<Plane> specification = null, Expression<Func<Plane, bool>> filter = null, QueryOrder<Plane> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<Plane, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false)
        {
            var result = await this.GetAllGenericAsync<PlaneSpecificQueryModel, PlaneSpecificQueryModelMapper>(id, specification, filter, queryOrder, firstElement, pageCount, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
            return this.planeSpecificQueryModelMapper.QueryModelsToDtoListItems(result);
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<PlaneDto>> GetAllAsync(Expression<Func<Plane, int>> orderByExpression, bool ascending, int id = 0, Specification<Plane> specification = null, Expression<Func<Plane, bool>> filter = null, int firstElement = 0, int pageCount = 0, Expression<Func<Plane, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false)
        {
            var result = await this.GetAllGenericAsync<PlaneSpecificQueryModel, PlaneSpecificQueryModelMapper>(orderByExpression, ascending, id, specification, filter, firstElement, pageCount, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
            return this.planeSpecificQueryModelMapper.QueryModelsToDtoListItems(result);
        }

        /// <inheritdoc/>
        public override async Task<PlaneSpecificDto> GetAsync(int id = 0, Specification<Plane> specification = null, Expression<Func<Plane, bool>> filter = null, Expression<Func<Plane, object>>[] includes = null, string accessMode = "Read", string queryMode = "Read", string mapperMode = "Item", bool isReadOnlyMode = false)
        {
            var result = await this.GetGenericAsync<PlaneSpecificQueryModel, PlaneSpecificQueryModelMapper>(id, specification, filter, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
            return this.planeSpecificQueryModelMapper.QueryModelToDto(result);
        }
#pragma warning restore S1006 // Method overrides should not change parameter defaults
    }
}