// <copyright file="IAuditFeature.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;

    public interface IAuditFeature
    {
        void UseAuditFeatures(IServiceProvider serviceProvider);
    }
}
