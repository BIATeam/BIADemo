// <copyright file="ViewAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class ViewAppService : AppServiceBase<ViewDto, View, int, ViewMapper>, IViewAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ViewAppService> logger;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IViewQueryCustomizer queryCustomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="queryCustomizer">The query customizer.</param>
        public ViewAppService(ITGenericRepository<View, int> repository, IPrincipal principal, ILogger<ViewAppService> logger, IViewQueryCustomizer queryCustomizer)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
            this.logger = logger;
            this.queryCustomizer = queryCustomizer;
            this.Repository.QueryCustomizer = queryCustomizer;
        }

        /// <inheritdoc cref="IViewAppService.GetAllAsync"/>
        public async Task<IEnumerable<ViewDto>> GetAllAsync()
        {
            int currentUserId = this.principal.GetUserId();
            IEnumerable<string> currentUserPermissions = this.principal.GetUserPermissions();

            if (currentUserPermissions?.Any(x => x == Rights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view => view.ViewType == ViewType.System || view.ViewType == ViewType.Team || (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
            else
            {
                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view =>
                    (view.ViewType == ViewType.System) ||
                    (view.ViewType == ViewType.Team && view.ViewTeams.Any(viewTeam => viewTeam.Team.Members.Any(member => member.UserId == currentUserId))) ||
                    (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
        }

        /// <inheritdoc cref="IViewAppService.RemoveTeamViewAsync"/>
        public async Task RemoveTeamViewAsync(int id)
        {
            View entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewTeams) ?? throw new ElementNotFoundException();
            if (entity.ViewType != ViewType.Team)
            {
                this.logger.LogWarning("Trying to delete the wrong view type: " + entity.ViewType);
                throw new BusinessException("Wrong view type: " + entity.ViewType);
            }

            IEnumerable<ViewDto> entities = await this.GetAllAsync();
            if (entities?.Any(x => x.Id == id) != true)
            {
                throw new BusinessException("You don't have access rights.");
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc cref="IViewAppService.RemoveUserViewAsync"/>
        public async Task RemoveUserViewAsync(int id)
        {
            View entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewUsers) ?? throw new ElementNotFoundException();
            if (entity.ViewType != ViewType.User)
            {
                this.logger.LogWarning("Trying to delete the wrong view type: " + entity.ViewType);
                throw new BusinessException("Wrong view type: " + entity.ViewType);
            }

            var currentUserId = this.principal.GetUserId();
            if (entity.ViewUsers.All(a => a.UserId != currentUserId))
            {
                this.logger.LogWarning($"The user {currentUserId} is trying to delete the view {id} of an other user.");
                throw new BusinessException("Can't delete the view of other users !");
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc cref="IViewAppService.SetDefaultUserViewAsync"/>
        public async Task SetDefaultUserViewAsync(DefaultViewDto dto)
        {
            var entity = await this.Repository.GetEntityAsync(id: dto.Id, queryMode: QueryCustomMode.ModeUpdateViewUsers);
            if (entity == null)
            {
                this.logger.LogWarning($"View with id {dto.Id} not found.");
                throw new ElementNotFoundException();
            }

            var currentUserId = this.principal.GetUserId();

            // if isDefault is true we have to remove others view set as default and update the
            // current one otherwise we remove the current view
            if (dto.IsDefault)
            {
                var formerDefault = (await this.Repository.GetAllEntityAsync(
                        specification: new DirectSpecification<View>(w => w.TableId == dto.TableId && w.ViewUsers.Any(a => a.IsDefault && a.UserId == currentUserId)),
                        queryMode: QueryCustomMode.ModeUpdateViewUsers))
                    .FirstOrDefault();

                var formerViewUser =
                    formerDefault?.ViewUsers.FirstOrDefault(a => a.IsDefault && a.UserId == currentUserId);
                if (formerViewUser != null)
                {
                    if (formerDefault.ViewType == ViewType.User)
                    {
                        formerViewUser.IsDefault = false;
                    }
                    else
                    {
                        formerDefault.ViewUsers.Remove(formerViewUser);
                    }

                    // this.Repository.Update(formerDefault)
                }

                if (entity.ViewType == ViewType.User)
                {
                    var viewUser =
                        entity.ViewUsers.FirstOrDefault(f => f.UserId == currentUserId);
                    if (viewUser != null)
                    {
                        viewUser.IsDefault = dto.IsDefault;
                    }
                }
                else
                {
                    entity.ViewUsers.Add(new ViewUser { IsDefault = true, ViewId = entity.Id, UserId = currentUserId });
                }
            }
            else
            {
                var viewUser =
                    entity.ViewUsers.FirstOrDefault(f => f.UserId == currentUserId);
                if (viewUser != null)
                {
                    if (entity.ViewType == ViewType.User)
                    {
                        viewUser.IsDefault = dto.IsDefault;
                    }
                    else
                    {
                        entity.ViewUsers.Remove(viewUser);
                    }
                }
            }

            // this.Repository.Update(entity)
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc cref="IViewAppService.SetDefaultTeamViewAsync"/>
        public async Task SetDefaultTeamViewAsync(DefaultTeamViewDto dto)
        {
            if (dto != null && dto.Id > 0 && dto.TeamId > 0 && !string.IsNullOrWhiteSpace(dto.TableId))
            {
                View entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewTeams);
                if (entity == null)
                {
                    this.logger.LogWarning($"View with id {dto.Id} not found.");
                    throw new ElementNotFoundException();
                }

                if (entity.ViewType != ViewType.Team)
                {
                    throw new BusinessException("Wrong view type: " + entity.ViewType);
                }

                IEnumerable<ViewDto> entities = await this.GetAllAsync();
                if (entities?.Any(x => x.Id == dto.Id) != true)
                {
                    throw new BusinessException("You don't have access rights.");
                }

                int currentTeamId = dto.TeamId;

                if (dto.IsDefault)
                {
                    View formerDefault = (await this.Repository.GetAllEntityAsync(
                            filter: w => w.TableId == dto.TableId && w.ViewTeams.Any(a => a.IsDefault && a.TeamId == currentTeamId), queryMode: QueryCustomMode.ModeUpdateViewTeams))
                        .FirstOrDefault();

                    ViewTeam formerViewTeam =
                        formerDefault?.ViewTeams.FirstOrDefault(a => a.IsDefault && a.TeamId == currentTeamId);
                    if (formerViewTeam != null)
                    {
                        if (formerDefault.ViewType == ViewType.Team)
                        {
                            formerViewTeam.IsDefault = false;
                        }
                        else
                        {
                            formerDefault.ViewTeams.Remove(formerViewTeam);
                        }

                        // this.Repository.Update(formerDefault)
                    }
                }

                ViewTeam viewTeam = entity.ViewTeams?.FirstOrDefault(f => f.TeamId == currentTeamId);
                if (viewTeam != null)
                {
                    viewTeam.IsDefault = dto.IsDefault;
                }
                else
                {
                    entity.ViewTeams ??= new List<ViewTeam>();
                    entity.ViewTeams.Add(new ViewTeam { IsDefault = true, ViewId = entity.Id, TeamId = currentTeamId });
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IViewAppService.AddUserViewAsync"/>
        public async Task<ViewDto> AddUserViewAsync(ViewDto dto)
        {
            if (dto != null)
            {
                var currentUserId = this.principal.GetUserId();

                var entity = new View();
                ViewMapper.MapperAddUserView(dto, entity, currentUserId);

                this.Repository.Add(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                dto.Id = entity.Id;
            }

            return dto;
        }

        /// <inheritdoc cref="IViewAppService.AddTeamViewAsync"/>
        public async Task<TeamViewDto> AddTeamViewAsync(TeamViewDto dto)
        {
            if (dto != null)
            {
                var entity = new View();
                ViewMapper.MapperAddTeamView(dto, entity);

                this.Repository.Add(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                dto.Id = entity.Id;
            }

            return dto;
        }

        /// <inheritdoc cref="IViewAppService.UpdateViewAsync"/>
        public async Task<ViewDto> UpdateViewAsync(ViewDto dto)
        {
            if (dto != null)
            {
                View entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewTeamsAndUsers) ?? throw new ElementNotFoundException();
                int currentUserId = this.principal.GetUserId();
                if (entity.ViewType == ViewType.User && entity.ViewUsers.All(a => a.UserId != currentUserId))
                {
                    this.logger.LogWarning($"The user {currentUserId} is trying to update the view {dto.Id} of an other user.");
                    throw new BusinessException("Can't update the view of other users !");
                }

                ViewMapper.MapperUpdateView(dto, entity);

                // this.Repository.Update(entity)
                await this.Repository.UnitOfWork.CommitAsync();
            }

            return dto;
        }

        /// <inheritdoc cref="IViewAppService.AssignViewToTeamAsync"/>
        public async Task AssignViewToTeamAsync(AssignViewToTeamDto dto)
        {
            if (dto != null)
            {
                View entity = await this.Repository.GetEntityAsync(dto.ViewId, queryMode: QueryCustomMode.ModeUpdateViewTeams) ?? throw new ElementNotFoundException();
                if (entity.ViewType != ViewType.Team)
                {
                    this.logger.LogWarning("Wrong view type: " + entity.ViewType);
                    throw new BusinessException("Wrong view type: " + entity.ViewType);
                }

                bool KeysPredicate(ViewTeam x) => x.ViewId == dto.ViewId && x.TeamId == dto.TeamId;
                bool hasChange = false;
                if (dto.IsAssign && entity.ViewTeams?.Any(KeysPredicate) != true)
                {
                    entity.ViewTeams ??= new List<ViewTeam>();
                    entity.ViewTeams.Add(new ViewTeam() { ViewId = dto.ViewId, TeamId = dto.TeamId });
                    hasChange = true;
                }
                else if (!dto.IsAssign && entity.ViewTeams?.Any(KeysPredicate) == true)
                {
                    entity.ViewTeams.Remove(entity.ViewTeams?.First(KeysPredicate));
                    if (entity.ViewTeams.Count == 0)
                    {
                        this.Repository.Remove(entity);
                    }

                    hasChange = true;
                }

                if (hasChange)
                {
                    // this.Repository.Update(entity)
                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }
    }
}