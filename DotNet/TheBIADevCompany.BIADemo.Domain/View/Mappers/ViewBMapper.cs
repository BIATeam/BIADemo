// <copyright file="ViewMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.View.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.View;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;
    using TheBIADevCompany.BIADemo.Domain.View.Entities;

    /// <summary>
    /// The mapper used for View.
    /// </summary>
    public class ViewBMapper : BaseMapper<ViewBDto, ViewB, int>
    {
        /// <inheritdoc />
        public override ExpressionCollection<ViewB> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<ViewB>(base.ExpressionCollection)
                {
                    { HeaderName.Description, view => view.Description },
                    { HeaderName.Name, view => view.Name },
                    { HeaderName.Preference, view => view.Preference },
                    { HeaderName.TableId, view => view.TableId },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(ViewBDto dto, ref ViewB entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            entity.Preference = dto.Preference;
            entity.TableId = dto.TableId;
        }

        /// <inheritdoc />
        public override Expression<Func<ViewB, ViewBDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new ViewBDto
            {
                Description = entity.Description,
                Name = entity.Name,
                Preference = entity.Preference,
                TableId = entity.TableId,
                ViewType = (int)entity.ViewType,
                IsUserDefault = entity.ViewUsers.Any(a => a.UserId == userId && a.IsDefault),
                ViewTeams = entity.ViewTeams.Select(x => new ViewTeamDto() { TeamId = x.Team.Id, TeamTitle = x.Team.Title, IsDefault = x.IsDefault }).ToList(),
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(ViewBDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.Description, () => CSVString(dto.Description) },
                { HeaderName.Name, () => CSVString(dto.Name) },
                { HeaderName.Preference, () => CSVString(dto.Preference) },
                { HeaderName.TableId, () => CSVString(dto.TableId) },
            };
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for description.
            /// </summary>
            public const string Description = "description";

            /// <summary>
            /// Header name for name.
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// Header name for preference.
            /// </summary>
            public const string Preference = "preference";

            /// <summary>
            /// Header name for table id.
            /// </summary>
            public const string TableId = "tableId";
        }
    }
}
