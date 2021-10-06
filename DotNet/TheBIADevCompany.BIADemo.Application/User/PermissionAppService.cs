// <copyright file="PermissionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class PermissionAppService : FilteredServiceBase<Permission>, IPermissionAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        public PermissionAppService(ITGenericRepository<Permission> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.GetAllAsync<OptionDto, PermissionOptionMapper>();
        }

        /// <summary>
        /// Return list of ids of the translated permissions.
        /// </summary>
        /// <param name="permissions">the permission at string format.</param>
        /// <returns>List of id.</returns>
        public IEnumerable<int> GetPermissionsIds(List<string> permissions)
        {
            return this.GetAllAsync<OptionDto, PermissionOptionMapper>(filter: p => permissions.Contains(p.Code)).Result.Select(p => p.Id);
        }
    }
}