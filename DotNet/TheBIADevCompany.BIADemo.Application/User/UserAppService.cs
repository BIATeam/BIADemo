// <copyright file="UserAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    public class UserAppService : FilteredServiceBase<User>, IUserAppService
    {
        /// <summary>
        /// The user right domain service.
        /// </summary>
        private readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The user synchronize domain service.
        /// </summary>
        private readonly IUserSynchronizeDomainService userSynchronizeDomainService;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<UserAppService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userPermissionDomainService">The user right domain service.</param>
        /// <param name="userSynchronizeDomainService">The user synchronize domain service.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="logger">The logger.</param>
        public UserAppService(
            ITGenericRepository<User> repository,
            IUserPermissionDomainService userPermissionDomainService,
            IUserSynchronizeDomainService userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILogger<UserAppService> logger)
            : base(repository)
        {
            this.userPermissionDomainService = userPermissionDomainService;
            this.userSynchronizeDomainService = userSynchronizeDomainService;
            this.configuration = configuration.Value;
            this.userDirectoryHelper = userDirectoryHelper;
            this.logger = logger;

            this.filtersContext.Add(AccessMode.Read, new DirectSpecification<User>(u => u.IsActive));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync(string filter = null)
        {
            Specification<User> specification = null;
            if (!string.IsNullOrEmpty(filter))
            {
                specification = UserSpecification.Search(filter);
            }

            return this.GetAllAsync<OptionDto, UserOptionMapper>(specification: specification, queryOrder: new QueryOrder<User>().OrderBy(o => o.LastName).ThenBy(o => o.FirstName));
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetRangeAsync"/>
        public async Task<(IEnumerable<UserDto> Results, int Total)> GetRangeAsync(
            LazyLoadDto filters = null,
            int id = 0,
            Specification<User> specification = null,
            Expression<Func<User, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null)
        {
            return await this.GetRangeAsync<UserDto, UserMapper, LazyLoadDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc cref="IUserPermissionDomainService.GetPermissionsForUserAsync"/>
        public async Task<List<string>> GetUserDirectoryRolesAsync(string sid)
        {
            return await this.userDirectoryHelper.GetUserRolesBySid(sid);
        }

        /// <inheritdoc cref="IUserAppService.GetPermissionsForUserAsync"/>
        public async Task<List<string>> GetPermissionsForUserAsync(List<string> userDirectoryRoles, string sid, int siteId = 0, int roleId = 0)
        {
            return await this.userPermissionDomainService.GetPermissionsForUserAsync(userDirectoryRoles, sid, siteId, roleId);
        }

        /// <inheritdoc cref="IUserAppService.TranslateRolesInPermissions"/>
        public List<string> TranslateRolesInPermissions(List<string> roles)
        {
            return this.userPermissionDomainService.TranslateRolesInPermissions(roles);
        }

        /// <inheritdoc cref="IUserAppService.GetCreateUserInfoAsync"/>
        public async Task<UserInfoDto> GetCreateUserInfoAsync(string sid)
        {
            var userInfo =
                await this.Repository.GetResultAsync(UserSelectBuilder.SelectUserInfo(), filter: user => user.Sid == sid);

            if (userInfo != null)
            {
                userInfo.Language = this.configuration.Languages.Where(w => w.Country == userInfo.Country)
                    .Select(s => s.Code)
                    .FirstOrDefault();
                return userInfo;
            }

            // if user is not found in DB, try to synchronize from AD.
            UserFromDirectory userAD = await this.userDirectoryHelper.ResolveUserBySid(sid);

            if (userAD != null)
            {
                User user = new User();
                UserFromDirectory.UpdateUserFieldFromDirectory(user, userAD);

                this.Repository.Add(user);
                await this.Repository.UnitOfWork.CommitAsync();

                userInfo = new UserInfoDto
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                };
                userInfo.Language = this.configuration.Languages.Where(w => w.Country == userInfo.Country)
                    .Select(s => s.Code)
                    .FirstOrDefault();
            }

            return userInfo;
        }

        /// <inheritdoc cref="IUserAppService.GetUserProfileAsync"/>
        public async Task<UserProfileDto> GetUserProfileAsync(string login)
        {
            var profile = new UserProfileDto();

            var url = this.configuration.UserProfile.Url;
            if (!string.IsNullOrEmpty(url))
            {
                var parameters = new Dictionary<string, string> { { "login", login } };
                Dictionary<string, string> result;

                try
                {
                    result = await RequestHelper.GetAsync<Dictionary<string, string>>(url, parameters);
                }
                catch (Exception exception)
                {
                    result = new Dictionary<string, string>();
                    this.logger.LogError(exception, "An error occured while getting the user profile.");
                }

                foreach (var item in result)
                {
                    typeof(UserProfileDto).GetProperty(item.Key)?.SetValue(profile, item.Value);
                }
            }

            return profile;
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetAllLdapUsersDomains()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.configuration.Authentication.LdapDomains.Where(d => d.ContainsUser).Select(d => d.Name).ToList();
        }

        /// <inheritdoc cref="IUserAppService.GetAllADUserAsync"/>
        public async Task<IEnumerable<UserFromDirectoryDto>> GetAllADUserAsync(string filter, string ldapName = null)
        {
            return await Task.FromResult(this.userDirectoryHelper.SearchUsers(filter, ldapName).OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                .Select(UserFromDirectoryMapper.EntityToDto())
                .ToList());
        }

        /// <inheritdoc cref="IUserAppService.AddInGroupAsync"/>
        public async Task<List<string>> AddInGroupAsync(IEnumerable<UserFromDirectoryDto> users)
        {
            var ldapGroups = this.userDirectoryHelper.GetLdapGroupsForRole("User");
            List<string> errors = new List<string>();
            if (ldapGroups != null && ldapGroups.Count > 0)
            {
                errors = await this.userDirectoryHelper.AddUsersInGroup(users.Select(UserFromDirectoryMapper.DtoToEntity()).ToList(), "User");
                await this.SynchronizeWithADAsync();
            }
            else
            {
                foreach (var userFormDirectoryDto in users)
                {
                    try
                    {
                        var foundUser = (await this.Repository.GetAllEntityAsync(filter: x => x.Sid == userFormDirectoryDto.Sid)).FirstOrDefault();

                        await this.userSynchronizeDomainService.AddOrActiveUserFromAD(userFormDirectoryDto.Sid, foundUser);

                        await this.Repository.UnitOfWork.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        errors.Add(userFormDirectoryDto.Domain + "\\" + userFormDirectoryDto.Login);
                    }
                }
            }

            return errors;
        }

        /// <inheritdoc cref="IUserAppService.RemoveInGroupAsync"/>
        public async Task<string> RemoveInGroupAsync(int id)
        {
            var ldapGroups = this.userDirectoryHelper.GetLdapGroupsForRole("User");
            List<string> errors = new List<string>();
            var user = await this.Repository.GetEntityAsync(id: id);
            if (ldapGroups != null && ldapGroups.Count > 0)
            {
                if (user == null)
                {
                    return "User not found in database";
                }

                List<IUserFromDirectory> notRemovedUser = await this.userDirectoryHelper.RemoveUsersInGroup(new List<IUserFromDirectory>() { new UserFromDirectory() { Guid = user.Guid, Login = user.Login } }, "User");

                await this.SynchronizeWithADAsync();
                if (notRemovedUser.Count != 0)
                {
                    return "Not able to remove user. (Probably define in sub group)";
                }
            }
            else
            {
                this.userSynchronizeDomainService.DeactivateUser(user);
                await this.Repository.UnitOfWork.CommitAsync();
            }

            return string.Empty;
        }

        /// <inheritdoc cref="IUserAppService.SynchronizeWithADAsync"/>
        public async Task SynchronizeWithADAsync(bool fullSynchro = false)
        {
            await this.userSynchronizeDomainService.SynchronizeFromADGroupAsync(fullSynchro);
        }

        /// <inheritdoc cref="IUserAppService.UpdateLastLoginDateAndActivate"/>
        public async Task UpdateLastLoginDateAndActivate(int userId)
        {
            if (userId > 0)
            {
                User entity = await this.Repository.GetEntityAsync(id: userId, queryMode: "NoInclude");
                entity.LastLoginDate = DateTime.Now;
                entity.IsActive = true;
                this.Repository.Update(entity);
                await this.Repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IUserAppService.AddInDBAsync"/>
        public async Task AddInDBAsync(IEnumerable<UserFromDirectoryDto> users)
        {
            foreach (var user in users)
            {
                await this.GetCreateUserInfoAsync(user.Login);
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> ExportCSV(FileFiltersDto filters)
        {
            // We ignore paging to return all records
            filters.First = 0;
            filters.Rows = 0;

            var queryFilter = new LazyLoadDto
            {
                Filters = filters.Filters,
                GlobalFilter = filters.GlobalFilter,
                SortField = filters.SortField,
                SortOrder = filters.SortOrder,
            };

            var query = await this.GetRangeAsync(filters: queryFilter);

            List<object[]> records = query.Results.Select(user => new object[]
            {
                user.LastName,
                user.FirstName,
                user.Login,
            }).ToList();

            List<string> columnHeaders = null;
            if (filters is FileFiltersDto fileFilters)
            {
                columnHeaders = fileFilters.Columns.Select(x => x.Value).ToList();
            }

            StringBuilder csv = new StringBuilder();
            records.ForEach(line =>
            {
                csv.AppendLine(string.Join(BIAConstants.Csv.Separator, line));
            });

            string csvSep = $"sep={BIAConstants.Csv.Separator}\n";
            var buffer = Encoding.GetEncoding("iso-8859-1").GetBytes($"{csvSep}{string.Join(BIAConstants.Csv.Separator, columnHeaders ?? new List<string>())}\r\n{csv}");
            return buffer;
        }
    }
}