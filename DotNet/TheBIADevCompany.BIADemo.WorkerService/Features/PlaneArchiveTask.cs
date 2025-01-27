namespace TheBIADevCompany.BIADemo.WorkerService.Features
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

    internal class PlaneArchiveTask : EntityArchiveTask<Plane, int>
    {
        public PlaneArchiveTask(ITGenericRepository<Plane, int> repository, ILogger<PlaneArchiveTask> logger) : base(repository, logger)
        {
        }

        protected override Expression<Func<Plane, object>>[] Includes()
        {
            return
            [
                x => x.ConnectingAirports,
                x => x.CurrentAirport,
                x => x.Engines
            ];
        }

        protected override Expression<Func<Plane, bool>> Step1Predicate()
        {
            return x => x.Id == 9;
        }
    }
}
