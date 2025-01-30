namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public interface IPlaneArchiveRepository : ITGenericArchiveRepository<Plane, int>
    {
    }
}
