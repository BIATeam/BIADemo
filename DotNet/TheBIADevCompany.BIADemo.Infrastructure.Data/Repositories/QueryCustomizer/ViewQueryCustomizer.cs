// <copyright file="ViewQueryCustomizer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate;

    /// <summary>
    /// Class use to customize the EF request on Member entity.
    /// </summary>
    public class ViewQueryCustomizer : TQueryCustomizer<View>, IViewQueryCustomizer
    {
        /// <inheritdoc/>
        public override IQueryable<View> CustomizeAfter(IQueryable<View> objectSet, string queryMode)
        {
            if (queryMode == QueryCustomMode.ModeUpdateViewUsers)
            {
                return objectSet.Include(view => view.ViewUsers);
            }
            else if (queryMode == QueryCustomMode.ModeUpdateViewSites)
            {
                return objectSet.Include(view => view.ViewSites);
            }
            else if (queryMode == QueryCustomMode.ModeUpdateViewSitesAndUsers)
            {
                return objectSet.Include(view => view.ViewUsers).Include(view => view.ViewSites);
            }

            return objectSet;
        }
    }
}
