// <copyright file="UserAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Entities
{
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Dedicated audit entity for <see cref="User"/>.
    /// </summary>
    public class UserAudit : BaseUserAudit<User>
    {
    }
}
