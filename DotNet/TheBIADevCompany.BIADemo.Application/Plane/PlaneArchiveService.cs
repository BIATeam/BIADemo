namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public class PlaneArchiveService : ArchiveServiceBase<Plane, int>, IArchiveService
    {
        private readonly ITGenericRepository<Plane, int> planeRepository;
        private readonly ITGenericRepository<Engine, int> engineRepository;

        public PlaneArchiveService(ILogger<PlaneArchiveService> logger, ITGenericRepository<Plane, int> planeRepository, ITGenericRepository<Engine, int> engineRepository) : base(logger)
        {
            this.planeRepository = planeRepository;
            this.engineRepository = engineRepository;
        }

        protected override async Task<IEnumerable<object>> GetStep1ItemsAsync()
        {
            var planes = await this.planeRepository.GetAllEntityAsync(
                filter: Step1Predicate(),
                includes: [p => p.CurrentAirport, p => p.ConnectingAirports]);

            var engines = await this.engineRepository.GetAllEntityAsync(includes: [e => e.InstalledEngineParts, e => e.PrincipalPart]);

            foreach(var plane in planes)
            {
               plane.Engines = engines.Where(e => e.PlaneId == plane.Id).ToList();
            }

            return planes;
        }

        protected override Expression<Func<Plane, bool>> Step1Predicate()
        {
            return p => p.Id == 9;
        }
    }
}
