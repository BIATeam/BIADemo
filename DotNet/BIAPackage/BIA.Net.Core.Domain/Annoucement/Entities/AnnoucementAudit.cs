// <copyright file="AnnoucementAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Annoucement.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Annoucement"/>.
    /// </summary>
    public sealed class AnnoucementAudit : AuditKeyedEntity<Annoucement, int, int>
    {
    }
}
