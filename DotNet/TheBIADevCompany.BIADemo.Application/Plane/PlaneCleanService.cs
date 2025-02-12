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
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    public class PlaneCleanService : CleanServiceBase<Plane, int>
    {
        public PlaneCleanService(ITGenericRepository<Plane, int> repository) : base(repository)
        {
        }

        protected override Expression<Func<Plane, bool>> CleanRuleFilter()
        {
            return x => x.IsArchived == true;
        }
    }
}
