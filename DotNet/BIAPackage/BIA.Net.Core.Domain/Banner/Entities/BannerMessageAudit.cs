// <copyright file="BannerMessageAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Banner.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="BannerMessage"/>.
    /// </summary>
    public sealed class BannerMessageAudit : AuditKeyedEntity<BannerMessage, int, int>
    {
    }
}
