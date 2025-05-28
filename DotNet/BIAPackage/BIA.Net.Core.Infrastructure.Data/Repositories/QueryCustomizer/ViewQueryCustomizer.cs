// <copyright file="ViewQueryCustomizer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.View.Entities;
    using Microsoft.EntityFrameworkCore;

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
            else if (queryMode == QueryCustomMode.ModeUpdateViewTeams)
            {
                return objectSet.Include(view => view.ViewTeams);
            }
            else if (queryMode == QueryCustomMode.ModeUpdateViewTeamsAndUsers)
            {
                return objectSet.Include(view => view.ViewUsers).Include(view => view.ViewTeams);
            }

            return objectSet;
        }
    }
}
