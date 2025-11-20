// <copyright file="AnnoucementAuditMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Annoucement.Mappers
{
    using System.Linq;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// Audit mapper for <see cref="Annoucement"/>.
    /// </summary>
    public class AnnoucementAuditMapper : AuditMapper<Annoucement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementAuditMapper"/> class.
        /// </summary>
        public AnnoucementAuditMapper()
        {
            this.AuditPropertyMappers =
                [
                    new AuditPropertyMapper<Annoucement, AnnoucementType>()
                    {
                        EntityProperty = x => x.Type,
                        EntityPropertyIdentifier = x => x.TypeId,
                        LinkedEntityPropertyDisplay = x => x.Id,
                    }
                ];
        }
    }
}
