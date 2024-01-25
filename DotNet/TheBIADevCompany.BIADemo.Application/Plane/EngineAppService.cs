// BIADemo only
// <copyright file="EngineAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using TheBIADevCompany.BIADemo.Application.Job;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class EngineAppService : CrudAppServiceBase<EngineDto, Engine, int, PagingFilterFormatDto, EngineMapper>, IEngineAppService
    {
        /// <summary>
        /// The current SiteId.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="configuration">The configuration.</param>
        public EngineAppService(ITGenericRepository<Engine, int> repository, IPrincipal principal, IConfiguration configuration)
            : base(repository)
        {
            this.configuration = configuration;
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentSiteId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Engine>(p => p.Plane.SiteId == this.currentSiteId));
        }

        /// <inheritdoc cref="IEngineAppService.CheckToBeMaintainedAsync"/>
        public async Task CheckToBeMaintainedAsync()
        {
            await (this.Repository as IEngineRepository).FillIsToBeMaintainedAsync(6);
        }

        /// <inheritdoc cref="IEngineAppService.LaunchJobManuallyExample"/>
        public void LaunchJobManuallyExample()
        {
            string projectName = this.configuration["Project:Name"];
            RecurringJob.TriggerJob($"{projectName}.{typeof(EngineManageTask).Name}");
        }
    }
}