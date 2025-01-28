namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public class PlaneArchiveService : ArchiveServiceBase<Plane, int>, IArchiveService
    {
        private readonly ITGenericRepository<Engine, int> engineRepository;

        public PlaneArchiveService(IConfiguration configuration, ILogger<PlaneArchiveService> logger, ITGenericRepository<Plane, int> planeRepository, ITGenericRepository<Engine, int> engineRepository) : base(configuration, planeRepository, logger)
        {
            this.engineRepository = engineRepository;
        }

        protected override async Task<IEnumerable<Plane>> GetArchiveStepItemsAsync()
        {
            var planes = await this.entityRepository.GetAllEntityAsync(
                filter: ArchiveStepItemsSelector(),
                includes: [p => p.CurrentAirport, p => p.ConnectingAirports]);

            var engines = await this.engineRepository.GetAllEntityAsync(includes: [e => e.InstalledEngineParts, e => e.PrincipalPart]);

            foreach(var plane in planes)
            {
               plane.Engines = engines.Where(e => e.PlaneId == plane.Id).ToList();
            }

            return planes;
        }

        protected override Expression<Func<Plane, bool>> ArchiveStepItemsSelector()
        {
            return base.ArchiveStepItemsSelector().CombineMapping(p => p.Id == 9);
        }
    }
}
