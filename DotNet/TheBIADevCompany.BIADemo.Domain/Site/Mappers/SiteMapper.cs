// <copyright file="SiteMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Site.Mappers
{
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class SiteMapper : BaseTeamMapper<SiteDto, Site>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public SiteMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<Site> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Site>(base.ExpressionCollection)
                {
                    // Begin BIADemo
                    { SiteHeaderName.UniqueIdentifier, team => team.UniqueIdentifier },

                    // End BIADemo
                };
            }
        }

        /// <summary>
        /// Precise the Id of the type of team.
        /// </summary>
        public override int TeamType
        {
            get { return (int)TeamTypeId.Site; }
        }

        /// <summary>
        /// Create a site entity from a DTO.
        /// </summary>
        /// <param name="dto">The site DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(SiteDto dto, ref Site entity)
        {
            base.DtoToEntity(dto, ref entity);

            // Begin BIADemo
            entity.UniqueIdentifier = dto.UniqueIdentifier;

            // End BIADemo
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Site, SiteDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new SiteDto
            {
                // Begin BIADemo
                UniqueIdentifier = entity.UniqueIdentifier,

                // End BIADemo
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(SiteDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                // Begin BIADemo
                { SiteHeaderName.UniqueIdentifier, () => CSVString(dto.UniqueIdentifier) },

                // End BIADemo
            };
        }

        // Begin BIADemo

        /// <inheritdoc />
        public override Expression<Func<Site, object>>[] IncludesBeforeDelete()
        {
            return
            [
                x => x.MaintenanceContracts
            ];
        }

        // End BIADemo

        /// <summary>
        /// Header names.
        /// </summary>
        public struct SiteHeaderName
        {
            // Begin BIADemo

            /// <summary>
            /// Header name for unique identifier.
            /// </summary>
            public const string UniqueIdentifier = "uniqueIdentifier";

            // End BIADemo
        }
    }
}