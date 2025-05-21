// BIADemo only
// <copyright file="EngineAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;

    // Begin BIADemo
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using TheBIADevCompany.BIADemo.Application.Job;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Specifications;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for engine.
    /// </summary>
    public class EngineAppService : CrudAppServiceBase<EngineDto, Engine, int, PagingFilterFormatDto, EngineMapper>, IEngineAppService
    {
        /// <summary>
        /// The current AncestorTeamId.
        /// </summary>
        private readonly int currentAncestorTeamId;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IEngineRepository repository;

        // Begin BIADemo

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The plane repository.
        /// </summary>
        private readonly ITGenericRepository<Plane, int> planeRepository;

#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        // End BIADemo

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // Begin BIADemo
        /// <param name="planeRepository">The plane repository.</param>
        /// <param name="configuration">The configuration.</param>
        // End BIADemo
        /// <param name="principal">The claims principal.</param>
        public EngineAppService(
            IEngineRepository repository,
            // Begin BIADemo
            ITGenericRepository<Plane, int> planeRepository,
            IConfiguration configuration,
            // End BIADemo
            IPrincipal principal)
            : base(repository)
        {
            this.repository = repository;

            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Engine>(x => x.Plane.SiteId == this.currentAncestorTeamId));

            // Begin BIADemo
            this.planeRepository = planeRepository;
            this.configuration = configuration;

            // End BIADemo
        }

        // Begin BIADemo
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

        /// <inheritdoc cref="IEngineAppService.CheckToBeMaintainedAsync"/>
        public async Task CheckToBeMaintainedAsync()
        {
            await this.repository.FillIsToBeMaintainedAsync(6);
        }

        /// <inheritdoc cref="IEngineAppService.LaunchJobManuallyExample"/>
        public void LaunchJobManuallyExample()
        {
            string projectName = this.configuration["Project:Name"];
            RecurringJob.TriggerJob($"{projectName}.{typeof(EngineManageTask).Name}");
        }

        /// <inheritdoc/>
        public override async Task<EngineDto> AddAsync(EngineDto dto, string mapperMode = null)
        {
            var planeParent = await this.planeRepository.GetEntityAsync(dto.PlaneId, isReadOnlyMode: true);
            if (planeParent.IsFixed)
            {
                throw new FrontUserException("Plane parent is fixed");
            }

            return await base.AddAsync(dto, mapperMode);
        }

        // End BIADemo

        /// <inheritdoc/>
#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public override Task<(IEnumerable<EngineDto> Results, int Total)> GetRangeAsync(PagingFilterFormatDto filters = null, int id = default, Specification<Engine> specification = null, Expression<Func<Engine, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            specification ??= EngineSpecification.SearchGetAll(filters);
            return base.GetRangeAsync(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc/>
#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public override Task<byte[]> GetCsvAsync(PagingFilterFormatDto filters = null, int id = default, Specification<Engine> specification = null, Expression<Func<Engine, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            specification ??= EngineSpecification.SearchGetAll(filters);
            return base.GetCsvAsync(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }
    }
}