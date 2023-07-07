// <copyright file="IUserIdentityKeyDomainService.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    public interface IUserIdentityKeyDomainService
    {
        Expression<Func<User, bool>> CheckDatabaseIdentityKey(string identityKey);

        Expression<Func<User, bool>> CheckDatabaseIdentityKey(List<string> identityKeys);

        Expression<Func<UserFromDirectory, bool>> CheckDirectoryIdentityKey(string identityKey);

        string GetDatabaseIdentityKey(User user);

        string GetDirectoryIdentityKey(UserFromDirectory userFromDirectory);

        string GetDirectoryIdentityKey(UserFromDirectoryDto userFromDirectory);
    }
}