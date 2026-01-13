// BIADemo only
// <copyright file="IBiaDemoRoleApiRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Api.RolesForApp;

    /// <summary>
    /// The interface for BIADemo Role API Repository.
    /// </summary>
    public interface IBiaDemoRoleApiRepository : IRoleApiRepository<ApiRolesForApp>
    {
    }
}
