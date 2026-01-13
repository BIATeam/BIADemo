// <copyright file="ViewMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.View.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.View;
    using BIA.Net.Core.Domain.View.Entities;
    using BIA.Net.Core.Domain.View.Models;

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
                ViewTeams = entity.ViewTeams.Select(x => new ViewTeamDto() { Id = x.Team.Id, TeamTitle = x.Team.Title, IsDefault = x.IsDefault }).ToList(),
            };
        }

        /// <summary>
        /// Mappers the add view.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="userId">The user identifier.</param>
        public static void MapperView(ViewDto dto, View entity, int userId)
        {
            if (dto != null)
            {
                if (entity == null)
                {
                    entity = new View();
                    entity.ViewTeams = [];
                }

                entity.Id = dto.Id;
                entity.TableId = dto.TableId;
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.Preference = dto.Preference;
                entity.ViewType = dto.ViewType == (int)ViewType.Team ? ViewType.Team : ViewType.User;
                if (entity.ViewType == ViewType.User && dto.Id == 0)
                {
                    entity.ViewUsers = [new ViewUser { IsDefault = false, ViewId = entity.Id, UserId = userId }];
                }

                if (entity.ViewType == ViewType.Team)
                {
                    if (entity.ViewTeams == null)
                    {
                        entity.ViewTeams = [];
                    }

                    foreach (var item in dto.ViewTeams)
                    {
                        if (dto.Id == 0 || item.DtoState == Dto.Base.DtoState.Added)
                        {
                            entity.ViewTeams.Add(new ViewTeam { IsDefault = item.IsDefault, ViewId = entity.Id, TeamId = item.Id });
                        }
                        else if (item.DtoState == Dto.Base.DtoState.Deleted)
                        {
                            var viewTeam = entity.ViewTeams.FirstOrDefault(x => x.TeamId == item.Id);
                            if (viewTeam != null)
                            {
                                entity.ViewTeams.Remove(viewTeam);
                            }
                        }
                    }
                }
            }
        }
    }
}