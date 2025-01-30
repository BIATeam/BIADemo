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
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    public class PlaneArchiveService : ArchiveServiceBase<Plane, int>
    {
        public PlaneArchiveService(IConfiguration configuration, IPlaneArchiveRepository archiveRepository, ILogger<PlaneArchiveService> logger) : base(configuration, archiveRepository, logger, false)
        {
        }
    }
}
