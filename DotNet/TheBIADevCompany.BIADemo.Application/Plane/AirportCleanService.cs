namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Clean;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public class AirportCleanService : CleanServiceBase<Airport, int>
    {
        public AirportCleanService(ITGenericCleanRepository<Airport, int> repository, ILogger<AirportCleanService> logger) : base(repository, logger)
        {
        }

        protected override Expression<Func<Airport, bool>> CleanRuleFilter()
        {
            return x => x.Name == "TLS";
        }
    }
}
