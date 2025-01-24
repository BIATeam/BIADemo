namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.WorkerService.Features.Archive;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public class PlaneArchiveConfiguration : IEntityArchiveConfiguration
    {
        public Type EntityType => typeof(Plane);

        public Expression<Func<object, bool>> Step1Predicate()
        {
            return obj => obj.GetType() == typeof(Plane) && ((Plane)obj).Id == 2049987;
        }

        public IQueryable<TEntity> SetIncludes<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            if (query is IQueryable<Plane> queryEntity)
            {
                return (IQueryable<TEntity>)queryEntity
                    .Include(x => x.CurrentAirport)
                    .Include(x => x.ConnectingAirports)
                    .Include(x => x.Engines).ThenInclude(x => x.InstalledParts)
                    .Include(x => x.Engines).ThenInclude(x => x.PrincipalPart);
            }

            return query;
        }
    }
}
