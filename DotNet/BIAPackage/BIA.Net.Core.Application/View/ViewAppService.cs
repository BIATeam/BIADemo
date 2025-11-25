// <copyright file="ViewAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Dto.View;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.View.Entities;
    using BIA.Net.Core.Domain.View.Mappers;
    using BIA.Net.Core.Domain.View.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class ViewAppService : DomainServiceBase<View, int>, IViewAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ViewAppService> logger;

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
            this.principal = principal as BiaClaimsPrincipal;
            this.logger = logger;
            this.Repository.QueryCustomizer = queryCustomizer;
        }

        /// <inheritdoc />
        public async Task<ViewDto> GetAsync(int id)
        {
            int currentUserId = this.principal.GetUserId();
            IEnumerable<string> currentUserPermissions = this.principal.GetUserPermissions();

            if (currentUserPermissions?.Any(x => x == BiaRights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    id: id,
                    filter: view =>
                        view.ViewType == ViewType.System ||
                        view.ViewType == ViewType.Team ||
                        (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
            else
            {
                BaseUserDataDto userData = this.principal.GetUserData<BaseUserDataDto>();

                return await this.Repository.GetResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    id: id,
                    filter: view =>
                        view.ViewType == ViewType.System ||
                        (view.ViewType == ViewType.Team && view.ViewTeams.Any(viewTeam => userData.CurrentTeams.Select(ct => ct.TeamId).Contains(viewTeam.Team.Id) &&
                                                                                          viewTeam.Team.Members.Any(member => member.UserId == currentUserId))) ||
                        (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ViewDto>> GetAllAsync()
        {
            int currentUserId = this.principal.GetUserId();
            IEnumerable<string> currentUserPermissions = this.principal.GetUserPermissions();

            if (currentUserPermissions?.Any(x => x == BiaRights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view =>
                        view.ViewType == ViewType.System ||
                        view.ViewType == ViewType.Team ||
                        (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
            else
            {
                BaseUserDataDto userData = this.principal.GetUserData<BaseUserDataDto>();

                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view =>
                        view.ViewType == ViewType.System ||
                        (view.ViewType == ViewType.Team && view.ViewTeams.Any(viewTeam => userData.CurrentTeams.Select(ct => ct.TeamId).Contains(viewTeam.Team.Id) &&
                                                                                          viewTeam.Team.Members.Any(member => member.UserId == currentUserId))) ||
                        (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
        }

        /// <inheritdoc />
        public async Task RemoveTeamViewAsync(int id)
        {
            View entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewTeams);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            if (entity.ViewType != ViewType.Team)
            {
                var message = "Trying to delete the wrong view type: " + entity.ViewType;
                this.logger.LogWarning(message);
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

        /// <inheritdoc />
        public async Task RemoveUserViewAsync(int id)
        {
            var entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewUsers);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            if (entity.ViewType != ViewType.User)
            {
                var message = "Trying to delete the wrong view type: " + entity.ViewType;
                this.logger.LogWarning(message);
                throw new BusinessException("Wrong view type: " + entity.ViewType);
            }

            var currentUserId = this.principal.GetUserId();
            if (entity.ViewUsers.All(a => a.UserId != currentUserId))
            {
                var message = $"The user {currentUserId} is trying to delete the view {id} of an other user.";
                this.logger.LogWarning(message);
                throw new BusinessException("Can't delete the view of other users !");
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc />
        public async Task SetDefaultUserViewAsync(DefaultViewDto dto)
        {
            var entity = await this.Repository.GetEntityAsync(id: dto.Id, queryMode: QueryCustomMode.ModeUpdateViewUsers);
            if (entity == null)
            {
                var message = $"View with id {dto.Id} not found.";
                this.logger.LogWarning(message);
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

        /// <inheritdoc />
        public async Task SetDefaultTeamViewAsync(DefaultTeamViewDto dto)
        {
            if (dto != null && dto.Id > 0 && dto.TeamId > 0 && !string.IsNullOrWhiteSpace(dto.TableId))
            {
                View entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewTeams);
                if (entity == null)
                {
                    var message = $"View with id {dto.Id} not found.";
                    this.logger.LogWarning(message);
                    throw new ElementNotFoundException(message);
                }

                if (entity.ViewType != ViewType.Team)
                {
                    var message = "Wrong view type: " + entity.ViewType;
                    this.logger.LogWarning(message);
                    throw new BusinessException(message);
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
                    entity.ViewTeams = entity.ViewTeams ?? new List<ViewTeam>();
                    entity.ViewTeams.Add(new ViewTeam { IsDefault = true, ViewId = entity.Id, TeamId = currentTeamId });
                }

                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<ViewDto> AddTeamViewAsync(ViewDto dto)
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

        /// <inheritdoc />
        public async Task<ViewDto> UpdateViewAsync(ViewDto dto)
        {
            if (dto != null)
            {
                var entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewTeamsAndUsers);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                var currentUserId = this.principal.GetUserId();
                if (entity.ViewType == ViewType.User && entity.ViewUsers.All(a => a.UserId != currentUserId))
                {
                    var message = $"The user {currentUserId} is trying to update the view {dto.Id} of an other user.";
                    this.logger.LogWarning(message);
                    throw new BusinessException("Can't update the view of other users !");
                }

                ViewMapper.MapperUpdateView(dto, entity);

                // this.Repository.Update(entity)
                await this.Repository.UnitOfWork.CommitAsync();
            }

            return dto;
        }

        /// <inheritdoc />
        public async Task AssignViewToTeamAsync(AssignViewToTeamDto dto)
        {
            if (dto != null)
            {
                View entity = await this.Repository.GetEntityAsync(dto.ViewId, queryMode: QueryCustomMode.ModeUpdateViewTeams);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                if (entity.ViewType != ViewType.Team)
                {
                    var message = "Wrong view type: " + entity.ViewType;
                    this.logger.LogWarning(message);
                    throw new BusinessException(message);
                }

                Func<ViewTeam, bool> keysPredicate = x => x.ViewId == dto.ViewId && x.TeamId == dto.TeamId;
                bool hasChange = false;
                if (dto.IsAssign && entity.ViewTeams?.Any(keysPredicate) != true)
                {
                    entity.ViewTeams = entity.ViewTeams ?? new List<ViewTeam>();
                    entity.ViewTeams.Add(new ViewTeam() { ViewId = dto.ViewId, TeamId = dto.TeamId });
                    hasChange = true;
                }
                else if (!dto.IsAssign && entity.ViewTeams?.Any(keysPredicate) == true)
                {
                    entity.ViewTeams.Remove(entity.ViewTeams?.First(keysPredicate));
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