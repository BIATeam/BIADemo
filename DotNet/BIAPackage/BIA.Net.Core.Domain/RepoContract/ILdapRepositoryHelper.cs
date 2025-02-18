// <copyright file="ILdapRepositoryHelper.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    /// <summary>
    /// ILdapRepositoryHelper.
    /// </summary>
    public interface ILdapRepositoryHelper
    {
        public bool IsLocalMachineDomain(string domain) { return false; }
    }
}
