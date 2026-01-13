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
    using BIA.Net.Core.Domain.Dto.Base.Interface;
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
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
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

        /// <summary>
        /// The plane repository.
        /// </summary>
        private readonly ITGenericRepository<Plane, int> planeRepository;

        // Begin BIADemo

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        // End BIADemo

        // BIAToolKit - Begin FixedChildrenRepositoryDefinitionEngine
        // BIAToolKit - End FixedChildrenRepositoryDefinitionEngine
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="planeRepository">The plane repository.</param>
        // Begin BIADemo
        /// <param name="configuration">The configuration.</param>
        // End BIADemo
        // BIAToolKit - Begin FixedChildrenRepositoryConstructorParamEngine
        // BIAToolKit - End FixedChildrenRepositoryConstructorParamEngine
        /// <param name="principal">The claims principal.</param>
        public EngineAppService(
            IEngineRepository repository,
            ITGenericRepository<Plane, int> planeRepository,
            // BIAToolKit - Begin FixedChildrenRepositoryInjectionEngine
            // BIAToolKit - End FixedChildrenRepositoryInjectionEngine
            // Begin BIADemo
            IConfiguration configuration,
            // End BIADemo
            IPrincipal principal)
            : base(repository)
        {
            this.repository = repository;
            this.planeRepository = planeRepository;
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Engine>(x => x.Plane.SiteId == this.currentAncestorTeamId));

            // BIAToolKit - Begin FixedChildrenRepositorySetEngine
            // BIAToolKit - End FixedChildrenRepositorySetEngine
            // Begin BIADemo
            this.configuration = configuration;

            // End BIADemo
        }
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

        // Begin BIADemo

        /// <inheritdoc />
        public async Task CheckToBeMaintainedAsync()
        {
            await this.repository.FillIsToBeMaintainedAsync(6);
        }

        /// <inheritdoc />
        public void LaunchJobManuallyExample()
        {
            string projectName = this.configuration["Project:Name"];
            RecurringJob.TriggerJob($"{projectName}.{typeof(EngineManageTask).Name}");
        }

        // End BIADemo

        /// <inheritdoc/>
        public override async Task<EngineDto> AddAsync(EngineDto dto, string mapperMode = null, bool autoCommit = true)
        {
            var planeParent = await this.planeRepository.GetEntityAsync(dto.PlaneId, isReadOnlyMode: true);
            if (planeParent.SiteId != this.currentAncestorTeamId)
            {
                throw new ForbiddenException("Can only add Engine on current parent Team.");
            }

            if (planeParent.IsFixed)
            {
                throw new FrontUserException("Plane parent is fixed");
            }

            return await base.AddAsync(dto, mapperMode, autoCommit: autoCommit);
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task ExecuteActionsOnUpdateFixedAsync(int entityUpdatedId, bool isFixed)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // BIAToolKit - Begin UpdateFixedChildrenEngine
            // BIAToolKit - End UpdateFixedChildrenEngine
        }

        /// <inheritdoc/>
        protected override Specification<Engine> GetFilterSpecification(IPagingFilterFormatDto filters)
        {
            var specification = base.GetFilterSpecification(filters);
            specification &= EngineSpecification.SearchGetAll(filters);
            return specification;
        }
    }
}