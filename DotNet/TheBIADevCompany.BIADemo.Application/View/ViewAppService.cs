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
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.SiteView;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class ViewAppService : AppServiceBase<View>, IViewAppService
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
        public ViewAppService(ITGenericRepository<View> repository, IPrincipal principal, ILogger<ViewAppService> logger, IViewQueryCustomizer queryCustomizer)
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
            IEnumerable<string> currentUserRights = this.principal.GetUserRights();

            if (currentUserRights?.Any(x => x == Rights.Sites.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view => view.ViewType == ViewType.Site || (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
            else
            {
                return await this.Repository.GetAllResultAsync(
                    ViewMapper.EntityToDto(currentUserId),
                    filter: view =>
                    (view.ViewType == ViewType.Site && view.ViewSites.Any(viewSite => viewSite.Site.Members.Any(member => member.UserId == currentUserId))) ||
                    (view.ViewType == ViewType.User && view.ViewUsers.Any(viewUser => viewUser.UserId == currentUserId)));
            }
        }

        /// <inheritdoc cref="IViewAppService.RemoveSiteViewAsync"/>
        public async Task RemoveSiteViewAsync(int id)
        {
            View entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewSites);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            if (entity.ViewType != ViewType.Site)
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
            var entity = await this.Repository.GetEntityAsync(id: id, queryMode: QueryCustomMode.ModeUpdateViewUsers);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

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

                    this.Repository.Update(formerDefault);
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

            this.Repository.Update(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        /// <inheritdoc cref="IViewAppService.SetDefaultSiteViewAsync"/>
        public async Task SetDefaultSiteViewAsync(DefaultSiteViewDto dto)
        {
            if (dto != null && dto.Id > 0 && dto.SiteId > 0 && !string.IsNullOrWhiteSpace(dto.TableId))
            {
                View entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewSites);
                if (entity == null)
                {
                    this.logger.LogWarning($"View with id {dto.Id} not found.");
                    throw new ElementNotFoundException();
                }

                if (entity.ViewType != ViewType.Site)
                {
                    throw new BusinessException("Wrong view type: " + entity.ViewType);
                }

                IEnumerable<ViewDto> entities = await this.GetAllAsync();
                if (entities?.Any(x => x.Id == dto.Id) != true)
                {
                    throw new BusinessException("You don't have access rights.");
                }

                int currentSiteId = dto.SiteId;

                if (dto.IsDefault)
                {
                    View formerDefault = (await this.Repository.GetAllEntityAsync(
                            filter: w => w.TableId == dto.TableId && w.ViewSites.Any(a => a.IsDefault && a.SiteId == currentSiteId), queryMode: QueryCustomMode.ModeUpdateViewSites))
                        .FirstOrDefault();

                    ViewSite formerViewSite =
                        formerDefault?.ViewSites.FirstOrDefault(a => a.IsDefault && a.SiteId == currentSiteId);
                    if (formerViewSite != null)
                    {
                        if (formerDefault.ViewType == ViewType.Site)
                        {
                            formerViewSite.IsDefault = false;
                        }
                        else
                        {
                            formerDefault.ViewSites.Remove(formerViewSite);
                        }

                        this.Repository.Update(formerDefault);
                    }
                }

                ViewSite viewSite = entity.ViewSites?.FirstOrDefault(f => f.SiteId == currentSiteId);
                if (viewSite != null)
                {
                    viewSite.IsDefault = dto.IsDefault;
                }
                else
                {
                    entity.ViewSites = entity.ViewSites ?? new List<ViewSite>();
                    entity.ViewSites.Add(new ViewSite { IsDefault = true, ViewId = entity.Id, SiteId = currentSiteId });
                }

                this.Repository.Update(entity);

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

        /// <inheritdoc cref="IViewAppService.AddSiteViewAsync"/>
        public async Task<SiteViewDto> AddSiteViewAsync(SiteViewDto dto)
        {
            if (dto != null)
            {
                var entity = new View();
                ViewMapper.MapperAddSiteView(dto, entity);

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
                var entity = await this.Repository.GetEntityAsync(dto.Id, queryMode: QueryCustomMode.ModeUpdateViewSitesAndUsers);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                var currentUserId = this.principal.GetUserId();
                if (entity.ViewType == ViewType.User && entity.ViewUsers.All(a => a.UserId != currentUserId))
                {
                    this.logger.LogWarning($"The user {currentUserId} is trying to update the view {dto.Id} of an other user.");
                    throw new BusinessException("Can't update the view of other users !");
                }

                ViewMapper.MapperUpdateView(dto, entity);

                this.Repository.Update(entity);
                await this.Repository.UnitOfWork.CommitAsync();
            }

            return dto;
        }

        /// <inheritdoc cref="IViewAppService.AssignViewToSiteAsync"/>
        public async Task AssignViewToSiteAsync(AssignViewToSiteDto dto)
        {
            if (dto != null)
            {
                View entity = await this.Repository.GetEntityAsync(dto.ViewId, queryMode: QueryCustomMode.ModeUpdateViewSites);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                if (entity.ViewType != ViewType.Site)
                {
                    this.logger.LogWarning("Wrong view type: " + entity.ViewType);
                    throw new BusinessException("Wrong view type: " + entity.ViewType);
                }

                Func<ViewSite, bool> keysPredicate = x => x.ViewId == dto.ViewId && x.SiteId == dto.SiteId;
                bool hasChange = false;
                if (dto.IsAssign && entity.ViewSites?.Any(keysPredicate) != true)
                {
                    entity.ViewSites = entity.ViewSites ?? new List<ViewSite>();
                    entity.ViewSites.Add(new ViewSite() { ViewId = dto.ViewId, SiteId = dto.SiteId });
                    hasChange = true;
                }
                else if (!dto.IsAssign && entity.ViewSites?.Any(keysPredicate) == true)
                {
                    entity.ViewSites.Remove(entity.ViewSites?.First(keysPredicate));
                    hasChange = true;
                }

                if (hasChange)
                {
                    this.Repository.Update(entity);
                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }
    }
}