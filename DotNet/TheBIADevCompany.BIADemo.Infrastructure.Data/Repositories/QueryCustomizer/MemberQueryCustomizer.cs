// <copyright file="MemberQueryCustomizer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// Class use to customize the EF request on Member entity.
    /// </summary>
    public class MemberQueryCustomizer : TQueryCustomizer<Member>, IMemberQueryCustomizer
    {
        /// <inheritdoc/>
        public override IQueryable<Member> CustomizeAfter(IQueryable<Member> objectSet, string queryMode)
        {
            if (queryMode == QueryMode.Update)
            {
                return objectSet.Include(member => member.MemberRoles);
            }

            return objectSet;
        }
    }
}
