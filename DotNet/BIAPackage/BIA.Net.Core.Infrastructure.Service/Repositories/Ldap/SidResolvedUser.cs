// <copyright file="SidResolvedUser.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public class SidResolvedUser<TUserFromDirectory> : SidResolvedItem
    {
        public TUserFromDirectory user;
    }
}
