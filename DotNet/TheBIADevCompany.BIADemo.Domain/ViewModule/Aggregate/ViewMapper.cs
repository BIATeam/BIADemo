// <copyright file="ViewMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TheBIADevCompany.BIADemo.Domain.Dto.SiteView;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public static class ViewMapper
    {
        /// <summary>
        /// Create a view DTO from an entity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The view DTO.
        /// </returns>
        public static Expression<Func<View, ViewDto>> EntityToDto(int userId)
        {
            return entity => new ViewDto
            {
                Id = entity.Id,
                Name = entity.Name,
                TableId = entity.TableId,
                ViewType = (int)entity.ViewType,
                Description = entity.Description,
                IsUserDefault = entity.ViewUsers.Any(a => a.UserId == userId && a.IsDefault),
                Preference = entity.Preference,
                ViewSites = entity.ViewSites.Select(x => new ViewSiteDto() { SiteId = x.Site.Id, SiteTitle = x.Site.Title, IsDefault = x.IsDefault }).ToList(),
            };
        }

        /// <summary>
        /// Mappers the add user view.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="userId">The user identifier.</param>
        public static void MapperAddUserView(ViewDto dto, View entity, int userId)
        {
            if (dto != null)
            {
                if (entity == null)
                {
                    entity = new View();
                }

                entity.Id = dto.Id;
                entity.TableId = dto.TableId;
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.Preference = dto.Preference;
                entity.ViewType = ViewType.User;
                entity.ViewUsers = new List<ViewUser>();
                entity.ViewUsers.Add(new ViewUser { IsDefault = false, ViewId = entity.Id, UserId = userId });
            }
        }

        /// <summary>
        /// Mappers the add site view.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="entity">The entity.</param>
        public static void MapperAddSiteView(SiteViewDto dto, View entity)
        {
            if (dto != null && dto.SiteId > 0)
            {
                if (entity == null)
                {
                    entity = new View();
                }

                entity.Id = dto.Id;
                entity.TableId = dto.TableId;
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.Preference = dto.Preference;
                entity.ViewType = ViewType.Site;
                entity.ViewSites = new List<ViewSite>();
                entity.ViewSites.Add(new ViewSite { IsDefault = false, ViewId = entity.Id, SiteId = dto.SiteId });
            }
        }

        /// <summary>
        /// Mappers the update view.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="entity">The entity.</param>
        public static void MapperUpdateView(ViewDto dto, View entity)
        {
            if (dto != null)
            {
                if (entity == null)
                {
                    entity = new View();
                }

                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.Preference = dto.Preference;
            }
        }
    }
}