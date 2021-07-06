// <copyright file="GenericLdapRepository.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Ldap;
    using Meziantou.Framework.Win32;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Helper to get information from Ldap.
    /// </summary>
    abstract public class GenericLdapRepository<TUserFromDirectory> : IUserDirectoryRepository<TUserFromDirectory>
        where TUserFromDirectory : class, IUserFromDirectory, new()


    {

        private const string CacheBeginKey = "BIAsid:";

        /// <summary>
        /// Groups cached.
        /// </summary>
        private static readonly Dictionary<string, string> CacheGroupName = new Dictionary<string, string>();

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<GenericLdapRepository<TUserFromDirectory>> logger;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomainsUsers;

        /// <summary>
        /// The sidResolver.
        /// </summary>
        private readonly LdapRepositoryHelper ldapRepositoryHelper;

        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        private readonly int LdapCacheGroupDuration;

        /// <summary>
        /// Duration of the cache for user property in ldap.
        /// </summary>
        private readonly int LdapCacheUserDuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLdapRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public GenericLdapRepository(ILogger<GenericLdapRepository<TUserFromDirectory>> logger, IOptions<BiaNetSection> configuration, ILdapRepositoryHelper ldapRepositoryHelper)
        {
            this.logger = logger;
            this.configuration = configuration.Value;
            this.ldapRepositoryHelper = (LdapRepositoryHelper) ldapRepositoryHelper;
            
            this.ldapDomains = this.configuration.Authentication.LdapDomains;
            this.ldapDomainsUsers = this.ldapDomains?.Where(l => l.ContainsUser == true);
            this.LdapCacheGroupDuration = configuration.Value.Authentication.LdapCacheGroupDuration;
            this.LdapCacheUserDuration = configuration.Value.Authentication.LdapCacheUserDuration;
        }

        /// <summary>
        /// Return the value of the property.
        /// </summary>
        /// <typeparam name="T">type of the value</typeparam>
        /// <param name="property">the property.</param>
        /// <returns>the value of the property.</returns>
        protected static T LdapGetValue<T>(PropertyValueCollection property)
        {
            object value = null;
            foreach (object tmpValue in property)
            {
                value = tmpValue;
            }

            return (T)value;
        }

        /// <summary>
        /// Return the domain of the Entity
        /// </summary>
        /// <param name="entry">the entry.</param>
        /// <returns>the </returns>
        protected static string LdapGetDomainName(DirectoryEntry entry)
        {
            if (entry == null || entry.Parent == null)
            {
                return null;
            }

            using DirectoryEntry parent = entry.Parent;
            if (LdapGetValue<string>(parent.Properties["objectClass"]) == "domainDNS")
            {
                return LdapGetValue<string>(parent.Properties["dc"]);
            }
            else
            {
                return LdapGetDomainName(parent);
            }
        }

        /// <summary>
        /// Convert the Ad entry in a UserInfoDirectory Object.
        /// </summary>
        /// <param name="entry">Entry from AD.</param>
        /// <param name="domainKey">Domain Name in config file where domain found.</param>
        protected abstract TUserFromDirectory ConvertToUserDirectory(DirectoryEntry entry, string domainKey);


        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.IsUserInGroup"/>
        private async Task<bool> IsUserSidInGroups(string sid, IEnumerable<LdapGroup> ldapGroups)
        {
            if (ldapGroups == null || ldapGroups.Count() == 0)
            {
                return false;
            }
            List<string> listUsersSid = new List<string>();
            List<string> listTreatedGroupSid = new List<string>();
            var getListTasks = new List<Task>();
            foreach (var ldapGroup in ldapGroups)
            {
                getListTasks.Add(GetAllUsersSidInLdapGroup(listUsersSid, listTreatedGroupSid, ldapGroup));
            }
            while (getListTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(getListTasks);
                if (listUsersSid.Any(usid => usid == sid)) return true;
                getListTasks.Remove(finishedTask);
            }
            return false;

        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.SearchUsers"/>
        public List<TUserFromDirectory> SearchUsers(string search, string ldapName = null)
        {
            int max = 10;
            List<TUserFromDirectory> usersInfo = new List<TUserFromDirectory>();
            if (string.IsNullOrEmpty(search))
            {
                return usersInfo;
            }

            if (!string.IsNullOrEmpty(ldapName))
            {
                LdapDomain ldapDomain = ldapDomainsUsers.FirstOrDefault(e => e.LdapName == ldapName);
                if (ldapDomain != null)
                {
                    return SearchUsersInDomain(search, ldapDomain).Take(max).ToList();
                }
            }

            foreach (var ldapDomain in ldapDomainsUsers)
            {
                var results = SearchUsersInDomain(search, ldapDomain);
                if (results != null)
                {
                    usersInfo.AddRange(results.Take(max - usersInfo.Count()));

                    if (usersInfo.Count() >= max)
                    {
                        break;
                    }
                }
            }

            return usersInfo;
        }

        private IEnumerable<TUserFromDirectory> SearchUsersInDomain(string search, LdapDomain domain)
        {
            if (domain == null || string.IsNullOrEmpty(domain.LdapName))
            {
                return null;
            }

            List<DirectoryEntry> usersMatches = new List<DirectoryEntry>();
            try
            {
                if (PrepareCredential(domain))
                {
                    using var entry = new DirectoryEntry($"LDAP://{domain.LdapName}", domain.LdapServiceAccount, domain.LdapServicePass);
                    using var searcher = new DirectorySearcher(entry)
                    {
                        Filter = $"(&(objectCategory=person)(objectClass=user)(|(givenname=*{search}*)(sn=*{search}*)(SAMAccountName=*{search}*)))",
                        SizeLimit = 10
                    };
                    usersMatches.AddRange(searcher.FindAll().Cast<SearchResult>().Select(s => s.GetDirectoryEntry()).ToList());
                }

            }
            catch (Exception e)
            {
                this.logger.LogError("Could not join Domain :" + domain, e);
            }

            return usersMatches.Select((um) => ConvertToUserDirectory(um, domain.Name));
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.AddUsersInGroup"/>
        public async Task<string> AddUsersInGroup(IEnumerable<IUserFromDirectory> usersFromDirectory, string roleLabel)
        {
            List<string> listGroupCacheSidToRemove = new List<string>();
            string errors = "";
            try
            {
                foreach (var user in usersFromDirectory)
                {
                    if (!AddUserInGroup(user, roleLabel, listGroupCacheSidToRemove))
                    {
                        if (errors != "")
                        {
                            errors += "<BR>";
                        }
                        errors += "Cannot add member " + user.Domain + "\\" + user.Login + " on ldap repository.";
                    }
                }
                foreach (var cacheSidToRemove in listGroupCacheSidToRemove)
                {
                    await this.ldapRepositoryHelper.distributedCache.Remove(CacheBeginKey + cacheSidToRemove);
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the users with GUID : " + string.Join(',', usersFromDirectory.Select(u => u.Domain + "\\" + u.Login + " (" + u.Guid + ")")));
            }
            return errors;
        }

        private bool AddUserInGroup(IUserFromDirectory user, string roleLabel, List<string> listGroupCacheSidToRemove)
        {

            try
            {
                PrincipalContext contextUser = PrepareDomainContext(user.Domain).Result;
                    
                    ;
                if (contextUser!= null)
                {
                    UserPrincipal userToAdd = UserPrincipal.FindByIdentity(contextUser, IdentityType.Guid, user.Guid.ToString());

                    if (userToAdd != null)
                    {
                        this.logger.LogInformation("User GUID " + user.Guid.ToString() + " found  in domain " + user.Domain);
                        string domainWhereUserFound = user.Domain;

                        GroupPrincipal group = PrepareGroupOfRoleForUser(domainWhereUserFound, roleLabel);
                        if (group == null)
                        {
                            // TODO : just add in db IF mode fake
                            return false;
                        }
                        try
                        {
                            group.Members.Add(userToAdd);
                            group.Save();

                            if (!listGroupCacheSidToRemove.Contains(group.Sid.Value))
                            {
                                listGroupCacheSidToRemove.Add(group.Sid.Value);
                            }


                            return true;
                        }
                        catch (Exception e)
                        {
                            this.logger.LogError(e, "[AddUserInGroup] Error when adding user " + userToAdd.UserPrincipalName + " in group " + group.UserPrincipalName);
                            return false;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "[AddUserInGroup] user GUID " + user.Guid.ToString() + " problem with domain " + user.Domain);
            }

            return false;
        }

        private GroupPrincipal PrepareGroupOfRoleForUser(string domainWhereUserFound, string roleLabel)
        {
            Role role = this.configuration.Roles.Where(w => w.Type == "Ldap" && w.Label == roleLabel).FirstOrDefault();
            if (role != null)
            {
                LdapGroup ldapGroup;
                PrincipalContext context;
                ldapGroup = role.LdapGroups.Where(g => g.AddUsersOfDomains.Any(d => d == domainWhereUserFound)).FirstOrDefault();
                if (ldapGroup == null)
                {
                    this.logger.LogError("[AddUserInGroup] LdapGroup not found for domain " + domainWhereUserFound + " of role " + role.Label);
                    return null;
                }
                context = PrepareDomainContext(ldapGroup.Domain).Result;
                if (context != null)
                {
                    var group = GroupPrincipal.FindByIdentity(context, ldapGroup.LdapName);
                    if (group == null)
                    {
                        this.logger.LogError("[AddUserInGroup] Cannot find group " + ldapGroup.LdapName + " in domain " + ldapGroup.Domain);
                    }
                    return group;
                }
            }

            return null;
        }

        Dictionary<string, PrincipalContext> PrincipalContextCache = new Dictionary<string, PrincipalContext>();
        SemaphoreSlim mutex = new SemaphoreSlim(1);

        internal async Task<PrincipalContext> PrepareDomainContext(string domain)
        {
            await mutex.WaitAsync().ConfigureAwait(false);
            try
            {
                if (PrincipalContextCache.ContainsKey(domain))
                {
                    return PrincipalContextCache[domain];
                }
                LdapDomain adDomain = ldapDomains.Where(d => d.Name == domain).FirstOrDefault();
                PrincipalContext pc = PrepareDomainContext(adDomain);
                PrincipalContextCache.Add(domain, pc);
                return pc;
            }
            finally
            {
                mutex.Release();
            }
        }

        private PrincipalContext PrepareDomainContext(LdapDomain domain)
        {
            try
            {
                if (PrepareCredential(domain))
                {
                    if (!string.IsNullOrEmpty(domain.LdapServiceAccount))
                    {
                        return new PrincipalContext(ContextType.Domain, domain.LdapName, domain.LdapServiceAccount, domain.LdapServicePass);
                    }

                    return new PrincipalContext(ContextType.Domain, domain.LdapName);
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Error when PrepareDomainContext for domain :" + domain.LdapName);
            }
            return null;
        }

        /// <summary>
        /// Extract credential from vault if requiered
        /// </summary>
        /// <param name="domain"></param>
        /// <returns>true if ok</returns>
        private bool PrepareCredential(LdapDomain domain)
        {
            if (string.IsNullOrEmpty(domain.LdapServiceAccount) && !string.IsNullOrEmpty(domain.CredentialKeyInWindowsVault))
            {

                try
                {
                    var cred = CredentialManager.ReadCredential(applicationName: domain.CredentialKeyInWindowsVault);
                    if (cred != null)
                    {
                        domain.LdapServiceAccount = cred.UserName;
                        domain.LdapServicePass = cred.Password;
                        return true;
                    }
                    this.logger.LogError("[PrepareCredential] Credential " + domain.CredentialKeyInWindowsVault + " not found in Vault");
                    return false;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "[PrepareCredential] Error when search credential " + domain.CredentialKeyInWindowsVault);
                    return false;
                }

            }
            return true;
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.RemoveUsersInGroup"/>
        public async Task<List<IUserFromDirectory>> RemoveUsersInGroup(List<IUserFromDirectory> usersFromRepositoryToRemove, string roleLabel)
        {
            List<string> listGroupCacheSidToRemove = new List<string>();

            List<IUserFromDirectory> notRemovedUser = new List<IUserFromDirectory>();

            try
            {

                foreach (var userToRemove in usersFromRepositoryToRemove)
                {
                    foreach (var domain in ldapDomainsUsers)
                    {
                        PrincipalContext contextUser = PrepareDomainContext(domain);
                        if (contextUser != null)
                        {
                            UserPrincipal userPrincipalToRemove = UserPrincipal.FindByIdentity(contextUser, IdentityType.Guid, userToRemove.Guid.ToString());

                            if (userPrincipalToRemove != null)
                            {
                                string domainWhereUserFound = domain.Name;
                                GroupPrincipal group = PrepareGroupOfRoleForUser(domainWhereUserFound, roleLabel);
                                if (group == null || !group.Members.Remove(userPrincipalToRemove/*context, IdentityType.Guid, userToRemove.Guid.ToString()*/))
                                {
                                    notRemovedUser.Add(userToRemove);
                                }
                                else
                                {
                                    group.Save();
                                }
                                if (!listGroupCacheSidToRemove.Contains(group.Sid.Value))
                                {
                                    listGroupCacheSidToRemove.Add(group.Sid.Value);
                                }
                                break;
                            }
                            this.logger.LogError("[RemoveUsersInGroup] user not find in all adDomains : ");
                        }
                    }
                }
                foreach (var cacheSidToRemove in listGroupCacheSidToRemove)
                {
                    await this.ldapRepositoryHelper.distributedCache.Remove(CacheBeginKey + cacheSidToRemove);
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "An error occured while adding the user with GUID : " + string.Join(',', usersFromRepositoryToRemove.Select(u => u.Domain + "\\" + u.Login + " (" + u.Guid + ")")));
            }

            return notRemovedUser;
        }


        /// <summary>
        /// Check if a user is in a group.
        /// </summary>
        /// <param name="ldapGroups">The groups to search in.</param>
        /// <param name="login">The user login.</param>
        /// <returns>A boolean indicating whether the user is in the group.</returns>
        public async Task<bool> IsSidInGroups(IEnumerable<LdapGroup> ldapGroups, string sid)
        {
            return await IsUserSidInGroups(sid, ldapGroups);
        }

        /// <inheritdoc cref="IUserDirectoryRepository<TUserDirectory>.GetAllUsersInGroup"/>
        public async Task<IEnumerable<string>> GetAllUsersSidInRoleToSync(string role)
        {
            var roleUser = this.configuration.Roles.Where(w =>( w.Type == "Ldap" || w.Type == "Synchro") && w.Label == role).FirstOrDefault();
            List<string> listUsersSid = new List<string>();
            if (roleUser.LdapGroups == null || roleUser.LdapGroups.Count() == 0)
            {
                return listUsersSid;
            }

            List<string> listTreatedGroupSid = new List<string>();
            var resolveTasks = new List<Task>();
            foreach (var ldapGroup in roleUser.LdapGroups)
            {
                resolveTasks.Add(GetAllUsersSidInLdapGroup(listUsersSid, listTreatedGroupSid, ldapGroup));
            }
            await Task.WhenAll(resolveTasks);
            return listUsersSid;
        }

        private async Task GetAllUsersSidInLdapGroup(List<string> listUsersSid, List<string> listTreatedGroupSid, LdapGroup ldapGroup)
        {
            GroupPrincipal groupPrincipal = null;
            try
            {
                PrincipalContext ctx = PrepareDomainContext(ldapGroup.Domain).Result;
                if (ctx != null)
                {
                    groupPrincipal = GroupPrincipal.FindByIdentity(ctx, ldapGroup.LdapName);
                }
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, "Could not join Domain :" + ldapGroup.Domain);
            }
            if (groupPrincipal != null)
            {
                this.logger.LogInformation("Group " + ldapGroup.LdapName + " found in domain " + ldapGroup.Domain);

                await this.GetAllUsersSidFromGroupRecursivelyAsync(groupPrincipal.Sid.Value, ldapGroup, listUsersSid, listTreatedGroupSid);
            }
        }

        /// <summary>
        /// Get all user from a group.
        /// </summary>
        /// <param name="groupPrincipalSid">The group sid to search in.</param>
        /// <param name="rootLdapGroup">the root ldapGroup to limite the scope of the search (ldap users and ldap for groups).</param>
        /// <param name="listUsers">The users found.</param>
        /// <param name="listTreatedGroups">The group already treated.</param>
        private async Task GetAllUsersSidFromGroupRecursivelyAsync(string groupPrincipalSid, LdapGroup rootLdapGroup, List<string> listUsersSid, List<string> listTreatedGroupSid)
        {
            SidResolvedGroup resolvedGroup = await ResolveGroupMember(groupPrincipalSid, rootLdapGroup);
            if (resolvedGroup != null)
            {
                foreach (string sid in resolvedGroup.MembersUserSid)
                {
                    if (!listUsersSid.Contains(sid))
                    {
                        listUsersSid.Add(sid);
                    }
                }
                
                var resolveTasks = new List<Task>();
                foreach (string sid in resolvedGroup.MembersGroupSid)
                {
                    if (listTreatedGroupSid.Contains(sid))
                    {
                        continue;
                    }
                    listTreatedGroupSid.Add(sid);

                    resolveTasks.Add(GetAllUsersSidFromGroupRecursivelyAsync(sid, rootLdapGroup, listUsersSid, listTreatedGroupSid));
                }
                await Task.WhenAll(resolveTasks);
            }
        }

        /// <summary>
        /// Get a user directory from a user principal.
        /// </summary>
        /// <param name="userPrincipal">The user principal.</param>
        /// <param name="domainKey">The domain Key (in config file).</param>
        /// <returns>The user AD.</returns>
        internal TUserFromDirectory GetUser(UserPrincipal userPrincipal, string domainKey)
        {
            return !(userPrincipal.GetUnderlyingObject() is DirectoryEntry entry) ? null : ConvertToUserDirectory(entry, domainKey);
        }

        /// <summary>
        /// Return the role from Ad and fake
        /// </summary>
        /// <param name="login">le login of the user</param>
        /// <returns>list of roles</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetUserRolesBySid(string sid)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var rolesSection = this.configuration.Roles;

            var adRoles = new List<string>();

            Parallel.ForEach(rolesSection, role =>
            {
                switch (role.Type)
                {
                    case "Fake":
                        adRoles.Add(role.Label);
                        break;

                    case "Ldap":
                        if (IsSidInGroups(role.LdapGroups, sid).Result)
                        {
                            adRoles.Add(role.Label);
                        }
                        break;
                }
            });

            return adRoles;
        }

        private async Task<SidResolvedGroup> ResolveGroupMember(string sid, LdapGroup rootLdapGroup)
        {
            SidResolvedGroup itemResolve;
            itemResolve = (SidResolvedGroup)await this.ldapRepositoryHelper.distributedCache.Get(CacheBeginKey + sid);
            if (itemResolve != null)
            {
                return itemResolve;
            }

            if (rootLdapGroup.RecursiveGroupsOfDomains == null || rootLdapGroup.RecursiveGroupsOfDomains.Count() ==0)
            {
                rootLdapGroup.RecursiveGroupsOfDomains = new string[] { rootLdapGroup.Domain };
                rootLdapGroup.ContainsOnlyUsers = true;
            }

            foreach (var groupDomain in rootLdapGroup.RecursiveGroupsOfDomains)
            {
                try
                {
                    PrincipalContext searchContext = PrepareDomainContext(groupDomain).Result;
                    if (searchContext!= null)
                    {
                        var subGroupPrincipal = GroupPrincipal.FindByIdentity(searchContext, IdentityType.Sid, sid);
                        if (subGroupPrincipal != null)
                        {
                            List<string> MembersGroupSid = new List<string>();
                            List<string> MembersUserSid = new List<string>();
                            DirectoryEntry de = (DirectoryEntry)subGroupPrincipal.GetUnderlyingObject();

                            List<string> listSDN = new List<string>();
                            foreach (string sDN in de.Properties["member"])
                            {
                                listSDN.Add(sDN);
                            }

                            Parallel.ForEach(listSDN, sDN =>
                                {
                                DirectoryEntry deMember = new DirectoryEntry("LDAP://" + sDN);
                                if (deMember != null)
                                {
                                    var itemSid = new SecurityIdentifier((byte[])deMember.Properties["objectSid"].Value, 0);
                                    bool isUser = true;
                                    bool isGroup = false;
                                    if (!rootLdapGroup.ContainsOnlyUsers)
                                    {
                                        var objectClass = deMember.Properties["objectClass"];
                                        // For group check
                                        isGroup = objectClass?.Contains("group") == true;
                                        // For user check
                                        isUser = objectClass?.Contains("user") == true;
                                        if ((!isGroup && !isUser) || (isGroup && isUser))
                                        {
                                            // Method for indeterminate group or user slower but work always.

                                            foreach (var groupTestDomain in rootLdapGroup.RecursiveGroupsOfDomains)
                                            {
                                                try
                                                {
                                                    PrincipalContext searchTestContext = PrepareDomainContext(groupTestDomain).Result;
                                                    if (searchTestContext != null)
                                                    {
                                                        var testIsGroup = GroupPrincipal.FindByIdentity(searchTestContext, IdentityType.Sid, itemSid.Value);
                                                        if (testIsGroup != null)
                                                        {
                                                            isGroup = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                catch (Exception)
                                                { }
                                            }
                                            isUser = !isGroup;
                                        }
                                    }

                                    if (isUser)
                                    {
                                        if (!MembersUserSid.Contains(itemSid.Value))
                                        {
                                            MembersUserSid.Add(itemSid.Value);
                                        }
                                    }
                                    else if (isGroup)
                                    {
                                        if (!MembersGroupSid.Contains(itemSid.Value))
                                        {
                                            MembersGroupSid.Add(itemSid.Value);
                                        }
                                    }
                                }
                           });

                            itemResolve = new SidResolvedGroup() { domainKey = groupDomain, MembersGroupSid = MembersGroupSid, MembersUserSid = MembersUserSid, type = SidResolvedItemType.Group };
                            await this.ldapRepositoryHelper.distributedCache.Add(CacheBeginKey + sid, itemResolve, this.LdapCacheGroupDuration);
                            return itemResolve;
                        }
                    }

                }
                catch(Exception ex)
                {
                    logger.LogError(ex, "Error when resolve on domain:" + groupDomain);
                }
            }
            // TODO ad cache unresolve item to reduce try

            return null;
        }

        public async Task<TUserFromDirectory> ResolveUserBySid(string sid)
        {
            TUserFromDirectory itemResolve;
            itemResolve = (TUserFromDirectory)await this.ldapRepositoryHelper.localCache.Get(CacheBeginKey + sid);
            if (itemResolve != null)
            {
                return itemResolve;
            }
            foreach (var userDomain in ldapDomainsUsers)
            {
                PrincipalContext searchContext = PrepareDomainContext(userDomain);
                if (searchContext != null)
                {
                    var userPrincipal = await Task.Run(() => UserPrincipal.FindByIdentity(searchContext, IdentityType.Sid, sid));
                    if (userPrincipal != null)
                    {
                        itemResolve = GetUser(userPrincipal, userDomain.Name); ;
                        await this.ldapRepositoryHelper.localCache.Add(CacheBeginKey + sid, itemResolve, this.LdapCacheUserDuration);
                        return itemResolve;
                    }
                }
            }
            return null;
        }

        public async Task<string> ResolveUserSidByLogin(string domain, string login)
        {
            var userDomain = ldapDomainsUsers.Where(d => d.Name == domain).FirstOrDefault();
            if (userDomain != null)
            {
                PrincipalContext searchContext = PrepareDomainContext(userDomain);
                if (searchContext != null)
                {
                    var userPrincipal = await Task.Run(() => UserPrincipal.FindByIdentity(searchContext, IdentityType.SamAccountName, login));
                    if (userPrincipal != null)
                    {
                        return userPrincipal.Sid.Value;
                    }
                }
            }
            return null;
        }
        public async Task<string> ResolveUserDomainByLogin(string login)
        {
            foreach (var userDomain in ldapDomainsUsers)
            {
                PrincipalContext searchContext = PrepareDomainContext(userDomain);
                if (searchContext != null)
                {
                    var userPrincipal = await Task.Run(() => UserPrincipal.FindByIdentity(searchContext, IdentityType.SamAccountName, login));
                    if (userPrincipal != null)
                    {
                        return userDomain.Name;
                    }
                }
            }
            return null;
        }
    }
}