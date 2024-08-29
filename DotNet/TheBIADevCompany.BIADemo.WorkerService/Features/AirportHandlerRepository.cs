namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    public class AirportHandlerRepository : PollingDatabaseHandlerRepository<Airport, int>
    {
        public AirportHandlerRepository(IServiceProvider serviceProvider)
            : base(serviceProvider, OnAirportChanged)
        {
        }

        public static void OnAirportChanged(Airport airport)
        {
            Debug.WriteLine($"Aiport {airport.Id} has changed");
        }
    }
}
