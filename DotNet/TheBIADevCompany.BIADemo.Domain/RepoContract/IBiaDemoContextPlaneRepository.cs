// <copyright file="IBiaDemoContextPlaneRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// Interface for implementations of BiaDemoContextPlaneRepository.
    /// </summary>
    public interface IBiaDemoContextPlaneRepository : ITGenericRepository<Plane, int>
    {
    }
}
