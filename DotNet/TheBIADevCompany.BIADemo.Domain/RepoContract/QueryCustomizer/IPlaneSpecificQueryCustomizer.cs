// BIADemo only
// <copyright file="IPlaneSpecificQueryCustomizer.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer
{
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Interface Plane Specific Query Customizer.
    /// </summary>
    public interface IPlaneSpecificQueryCustomizer : IQueryCustomizer<Plane>
    {
    }
}
