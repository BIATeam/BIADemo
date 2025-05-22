// <copyright file="ViewMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.View.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TheBIADevCompany.BIADemo.Domain.Bia.View.Entities;
    using TheBIADevCompany.BIADemo.Domain.Bia.View.Models;
    using TheBIADevCompany.BIADemo.Domain.Dto.Bia.View;

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
                ViewTeams = entity.ViewTeams.Select(x => new ViewTeamDto() { TeamId = x.Team.Id, TeamTitle = x.Team.Title, IsDefault = x.IsDefault }).ToList(),
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
        public static void MapperAddTeamView(TeamViewDto dto, View entity)
        {
            if (dto != null && dto.TeamId > 0)
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
                entity.ViewType = ViewType.Team;
                entity.ViewTeams = new List<ViewTeam>();
                entity.ViewTeams.Add(new ViewTeam { IsDefault = false, ViewId = entity.Id, TeamId = dto.TeamId });
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