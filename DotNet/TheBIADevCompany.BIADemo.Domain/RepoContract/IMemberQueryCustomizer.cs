// <copyright file="IMemberQueryCustomizer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// interface use to customize the request on Member entity.
    /// </summary>
    public interface IMemberQueryCustomizer : IQueryCustomizer<Member>
    {
    }
}
