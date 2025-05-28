// <copyright file="IViewQueryCustomizer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.View.Entities;

    /// <summary>
    /// interface use to customize the request on Member entity.
    /// </summary>
    public interface IViewQueryCustomizer : IQueryCustomizer<View>
    {
    }
}
