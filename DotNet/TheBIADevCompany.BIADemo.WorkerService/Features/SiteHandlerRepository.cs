namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    public class NewPlaneHandlerRepository : PollingDatabaseHandlerRepository<Plane, int>
    {
        public NewPlaneHandlerRepository(IQueryableUnitOfWorkReadOnly dataContext)
            : base(dataContext, OnPlaneChanged)
        {
        }

        public static void OnPlaneChanged(Plane plane)
        {
            Debug.WriteLine($"Plane {plane.Id} has changed");
        }
    }
}
