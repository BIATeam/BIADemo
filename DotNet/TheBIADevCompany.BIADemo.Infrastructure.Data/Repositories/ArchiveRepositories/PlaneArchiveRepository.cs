namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.ArchiveRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    public class PlaneArchiveRepository : TGenericArchiveRepository<Plane, int>, IPlaneArchiveRepository
    {
        public PlaneArchiveRepository(IQueryableUnitOfWork dataContext) : base(dataContext)
        {
        }

        protected override IQueryable<Plane> GetAllWithIncludes()
        {
            return base.GetAllWithIncludes()
                .Include(p => p.ConnectingAirports)
                .Include(p => p.ConnectingPlaneAirports)
                .Include(p => p.PlaneType)
                .Include(p => p.SimilarPlaneType)
                .Include(p => p.Engines)
                .ThenInclude(e => e.InstalledParts)
                .Include(p => p.Engines)
                .ThenInclude(e => e.PrincipalPart);
        }
    }
}
