// <copyright file="SidResolvedUser.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;

    /// <summary>
    /// SidResolvedUser.
    /// </summary>
    /// <typeparam name="TUserFromDirectory">type for user from directory.</typeparam>
    [Serializable]
    public class SidResolvedUser<TUserFromDirectory> : SidResolvedItem
    {
        /// <summary>
        /// the user.
        /// </summary>
        public TUserFromDirectory User { get; set; }
    }
}
